/*
 * @Author: fasthro
 * @Date: 2020-10-14 11:57:48
 * @Description: config
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UFramework.VersionControl;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{

    [System.Serializable]
    public class VEditorPatch
    {
        [HideInInspector]
        public int aVersion;

        [HideInInspector]
        public int pVersion;

        [HideInInspector]
        public long timestamp;

        [HideInInspector]
        public long len;

        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup]
        public string displayName;
        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup]
        public string displayLen;
        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup]
        public string displayTime;

        [ShowInInspector, ReadOnly, HideLabel]
        [HorizontalGroup(150)]
        [GUIColor("GetStatusColor")]
        public string status;

        [ShowInInspector, HideLabel]
        [HorizontalGroup(120)]
        [Button("Push Release")]
        [DisableIf("isRelease")]
        public void Operation()
        {
            if (EditorUtility.DisplayDialog("Patch", "确定发布补丁?", "发布", "取消"))
            {
                var versionConfig = UConfig.Read<VersionControl_VersionConfig>();
                var pv = versionConfig.GetPV();
                var dataPath = IOPath.PathCombine(App.BuildDirectory, Platform.BuildTargetCurrentName, "data-v" + versionConfig.version);

                var patchFilePath = IOPath.PathCombine(dataPath, displayName, displayName + ".zip");
                var versionFilePath = IOPath.PathCombine(dataPath, displayName, Version.FileName);
                if (!IOPath.FileExists(patchFilePath) || !IOPath.FileExists(versionFilePath) || len == 0)
                {
                    EditorUtility.DisplayDialog("Patch", "补丁文件不存在, 请构建补丁后尝试发布.", "确定");
                    return;
                }
                isRelease = true;
                status = GetStatus();

                foreach (var item in pv.supports)
                {
                    foreach (var pitem in item.patchs)
                    {
                        if (pitem.aVersion == aVersion && pitem.pVersion == pVersion)
                        {
                            pitem.isRelease = true;
                            pitem.status = pitem.GetStatus();
                        }
                    }
                }
            }
        }

        [HideInInspector]
        public bool isRelease;

        [HideInInspector]
        public List<VFile> files = new List<VFile>();

        [HideInInspector]
        public List<VScriptFile> sFiles = new List<VScriptFile>();

        public VEditorPatch()
        {
            status = GetStatus();
        }

        public void UpdateDisplay()
        {
            displayName = string.Format("p{0}.{1}.{2}", aVersion, pVersion, timestamp);
            displayLen = EditorUtility.FormatBytes(len);
            displayTime = TimeUtils.UTCTimeStampsFormat(timestamp, "yyyy-MM-dd HH:mm:ss");
        }

        public string GetStatus()
        {
            if (isRelease) return "Released";
            else return "Pending";
        }

        private Color GetStatusColor()
        {
            if (isRelease) return Color.green;
            return Color.red;
        }
    }

    [System.Serializable]
    public class VEditorInfo
    {
        [HideInInspector]
        public int version;

        public List<VEditorPatch> patchs = new List<VEditorPatch>();
    }

    public class PlatformVersion
    {
        /// <summary>
        /// platform
        /// </summary>
        public int platform;

        /// <summary>
        /// 当前版本补丁列表
        /// </summary>
        /// <returns></returns>
        public List<VEditorPatch> patchs = new List<VEditorPatch>();

        /// <summary>
        /// 支持的版本信息列表
        /// </summary>
        /// <returns></returns>
        public List<VEditorInfo> supports = new List<VEditorInfo>();

        /// <summary>
        /// 历史版本信息列表
        /// </summary>
        /// <value></value>
        public List<VEditorInfo> historys = new List<VEditorInfo>();

        /// <summary>
        /// 发布版本号记录
        /// </summary>
        /// <returns></returns>
        public List<int> releaseVersionCodes = new List<int>();

        /// <summary>
        /// 获取下一个补丁版本
        /// </summary>
        /// <returns></returns>
        public int GetNextPatchVersion()
        {
            var nextVersion = 0;
            foreach (var item in patchs)
                if (item.pVersion > nextVersion) nextVersion = item.pVersion;
            return nextVersion;
        }

        /// <summary>
        /// 获取补丁版本
        /// </summary>
        /// <returns></returns>
        public VEditorPatch GetPatchVersion(int version)
        {
            foreach (var item in patchs)
                if (item.pVersion == version) return item;
            return null;
        }

        /// <summary>
        /// 有新的补丁版本需要构建
        /// </summary>
        /// <returns></returns>
        public bool HasNewPatchVersionWaitBuild()
        {
            bool isNew = false;
            foreach (var item in patchs)
            {
                if (!item.isRelease)
                {
                    isNew = true;
                    break;
                }
            }
            return isNew;
        }

        /// <summary>
        /// 更新补丁数据
        /// </summary>
        /// <param name="patch"></param>
        public VEditorPatch UpdatePatch(VEditorPatch patch)
        {
            VEditorPatch newPatch = null;
            for (int i = 0; i < patchs.Count; i++)
            {
                if (patchs[i].aVersion == patch.aVersion && patchs[i].pVersion == patch.pVersion)
                {
                    var data = patchs[i];
                    data.aVersion = patch.aVersion;
                    data.pVersion = patch.pVersion;
                    data.files = patch.files;
                    data.sFiles = patch.sFiles;
                    data.len = patch.len;
                    data.UpdateDisplay();

                    newPatch = data;
                    break;
                }
            }

            foreach (var item in supports)
            {
                for (int i = 0; i < item.patchs.Count; i++)
                {
                    if (item.patchs[i].aVersion == patch.aVersion && item.patchs[i].pVersion == patch.pVersion)
                    {
                        var data = item.patchs[i];
                        data.aVersion = patch.aVersion;
                        data.pVersion = patch.pVersion;
                        data.files = patch.files;
                        data.sFiles = patch.sFiles;
                        data.len = patch.len;
                        data.UpdateDisplay();
                        break;
                    }
                }
            }
            return newPatch == null ? patch : newPatch;
        }
    }

    public class VersionControl_VersionConfig : IConfigObject
    {
        public FileAddress address { get { return FileAddress.Editor; } }

        /// <summary>
        /// 当前版本
        /// </summary>
        public int version = 0;

        /// <summary>
        /// 最低支持版本
        /// </summary>
        public int minVersion = 0;

        /// <summary>
        /// 平台版本信息
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="PlatformVersion"></typeparam>
        /// <returns></returns>
        public List<PlatformVersion> platforms = new List<PlatformVersion>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PlatformVersion GetPV()
        {
            PlatformVersion pv = null;
            int platform = Platform.BuildTargetCurrent;
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].platform == platform)
                {
                    pv = platforms[i];
                    break;
                }
            }
            if (pv == null)
            {
                pv = new PlatformVersion();
                pv.platform = platform;
                platforms.Add(pv);
            }
            return pv;
        }

        public void Save()
        {
            UConfig.Write<VersionControl_VersionConfig>(this);
        }
    }
}