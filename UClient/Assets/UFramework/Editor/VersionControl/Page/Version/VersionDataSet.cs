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
    public class ReleaseRecord
    {
        public int version;
        public List<VFile> files = new List<VFile>();
    }

    [System.Serializable]
    public class VEditorPatch
    {
        [ShowInInspector, ReadOnly, HideLabel]
        [HorizontalGroup]
        public int version;

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
            if (EditorUtility.DisplayDialog("Patch", "Do you want to release a patch?", "Push Release", "Cancel"))
            {
                isRelease = true;
                status = GetStatus();
            }
        }

        [HideInInspector]
        public bool isRelease;

        [HideInInspector]
        public Dictionary<string, VFile> files = new Dictionary<string, VFile>();

        public VEditorPatch()
        {
            status = GetStatus();
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
        /// files
        /// </summary>
        /// <typeparam name="FileInfo"></typeparam>
        /// <returns></returns>
        public List<VFile> files = new List<VFile>();

        /// <summary>
        /// 当前版本补丁列表
        /// </summary>
        /// <returns></returns>
        public List<VEditorPatch> patches = new List<VEditorPatch>();

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
        /// 发布版本文件记录
        /// </summary>
        /// <returns></returns>
        public List<ReleaseRecord> releaseRecords = new List<ReleaseRecord>();

        /// <summary>
        /// 获取下一个补丁版本
        /// </summary>
        /// <returns></returns>
        public int GetNextPatchVersion()
        {
            var nextVersion = 0;
            foreach (var item in patches)
                if (item.version > nextVersion) nextVersion = item.version;
            return nextVersion;
        }

        /// <summary>
        /// 有新的补丁版本需要构建
        /// </summary>
        /// <returns></returns>
        public bool HasNewPatchVersionWaitBuild()
        {
            bool isNew = false;
            foreach (var item in patches)
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
        public void UpdatePatch(VEditorPatch patch)
        {
            for (int i = 0; i < patches.Count; i++)
            {
                if (patches[i].version == patch.version)
                {
                    patches[i] = patch;
                    break;
                }
            }
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