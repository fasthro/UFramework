/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: version
 */

using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Assets;
using UFramework.Config;
using UFramework.VersionControl;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class VersionPage : IPage, IPageBar
    {
        public string menuName { get { return "Version"; } }

        static VersionControl_VersionConfig describeObject;

        /// <summary>
        /// 当前版本
        /// </summary>
        [ReadOnly]
        [BoxGroup("General Settings")]
        [LabelText("    Current Version")]
        public int version;

        /// <summary>
        /// 最低支持版本
        /// </summary>
        [BoxGroup("General Settings")]
        [LabelText("    Minimum Supported Version")]
        [OnValueChanged("OnMinVersionChange")]
        public int minVersion;

        [ShowInInspector, HideLabel]
        [TabGroup("Current Version Patch")]
        [TableList(AlwaysExpanded = true)]
        public List<VPatch> patchs = new List<VPatch>();

        [ShowInInspector, HideLabel, ReadOnly]
        [TabGroup("Support Versions")]
        [DictionaryDrawerSettings(KeyLabel = "Version Code", ValueLabel = "Patch", DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<int, VersionInfo> supportDictionary = new Dictionary<int, VersionInfo>();

        /// <summary>
        /// 版本信息列表
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [TabGroup("History Versions")]
        [DictionaryDrawerSettings(KeyLabel = "Version Code", ValueLabel = "Patch", DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<int, VersionInfo> historyDictionary = new Dictionary<int, VersionInfo>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<VersionControl_VersionConfig>();
            var pv = describeObject.GetPV();

            version = describeObject.version;
            minVersion = describeObject.minVersion;
            patchs = pv.patches;

            supportDictionary.Clear();
            foreach (var item in pv.supports)
                supportDictionary.Add(item.version, item);

            historyDictionary.Clear();
            foreach (var item in pv.historys)
                historyDictionary.Add(item.version, item);
        }

        public void OnSaveDescribe()
        {
            var pv = describeObject.GetPV();

            describeObject.version = version;
            describeObject.minVersion = minVersion;
            pv.patches = patchs;

            pv.supports.Clear();
            foreach (var item in supportDictionary)
                pv.supports.Add(item.Value);

            pv.historys.Clear();
            foreach (var item in historyDictionary)
                pv.historys.Add(item.Value);

            describeObject.Save();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("New Version")))
            {
                if (EditorUtility.DisplayDialog("New Version", "Are you sure to create a new version?", "Confirm", "No"))
                {
                    CreateNewVersion();
                }
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Remove Current Version")))
            {
                if (EditorUtility.DisplayDialog("Remove Version", "Are you sure to remove the current version?", "Confirm", "No"))
                {
                    RemoveCurrentVersion();
                }
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Remove All Version")))
            {
                if (EditorUtility.DisplayDialog("Remove All Version", "Are you sure to remove the all version?", "Confirm", "No"))
                {
                    RemoveAllVersion();
                }
            }
        }

        private void CreateNewVersion()
        {
            var info = new VersionInfo();
            info.version = version;
            info.patchs.Clear();
            info.patchs.AddRange(patchs);
            supportDictionary.Add(version, info);

            version++;
            patchs.Clear();
        }

        private void RemoveCurrentVersion()
        {
            var target = version - 1;
            if (target < 0) return;

            VersionInfo info = null;
            if (supportDictionary.ContainsKey(target))
            {
                info = supportDictionary[target];
                supportDictionary.Remove(target);
            }
            else if (historyDictionary.ContainsKey(target))
            {
                info = historyDictionary[target];
                historyDictionary.Remove(target);
            }

            if (info != null)
            {
                version = info.version;
                patchs.Clear();
                patchs.AddRange(info.patchs);
            }

            if (minVersion > version)
            {
                minVersion = version;
            }
        }

        private void RemoveAllVersion()
        {
            version = 0;
            minVersion = 0;
            patchs.Clear();

            supportDictionary.Clear();
            historyDictionary.Clear();
        }

        private void OnMinVersionChange()
        {
            if (minVersion > version) minVersion = version;
            if (minVersion < 0) minVersion = 0;

            List<int> _removes = new List<int>();
            foreach (KeyValuePair<int, VersionInfo> item in supportDictionary)
            {
                if (item.Key < minVersion)
                    _removes.Add(item.Key);
                historyDictionary.Remove(item.Key);
            }
            for (int i = _removes.Count - 1; i >= 0; i--)
            {
                var key = _removes[i];
                historyDictionary.Add(key, supportDictionary[key]);
                supportDictionary.Remove(_removes[i]);
            }

            for (int i = minVersion; i < version; i++)
            {
                if (!supportDictionary.ContainsKey(i))
                {
                    if (historyDictionary.ContainsKey(i))
                    {
                        supportDictionary.Add(i, historyDictionary[i]);
                        historyDictionary.Remove(i);
                    }
                }
            }
        }

        /// <summary>
        /// 构建版本信息文件
        /// </summary>
        public static void BuildVersion()
        {
            if (describeObject == null)
                describeObject = UConfig.Read<VersionControl_VersionConfig>();

            var pv = describeObject.GetPV();

            pv.files = GetVersionFiles();
            describeObject.Save();

            var ver = new Version();
            ver.version = describeObject.version;
            ver.minVersion = describeObject.minVersion;
            ver.timestamp = TimeUtils.UTCTimeStamps();
            ver.files.AddRange(pv.files);
            ver.versions.Clear();

            var vInfo = new VersionInfo();
            vInfo.version = describeObject.version;
            vInfo.patchs.Clear();
            vInfo.patchs.AddRange(pv.patches);
            ver.versions.Add(vInfo.version, vInfo);

            foreach (var item in pv.supports)
                ver.versions.Add(item.version, item);

            Version.VersionWrite(IOPath.PathCombine(App.TempDirectory, Version.FileName), ver);
        }

        /// <summary>
        /// 构建版本发布记录
        /// </summary>
        public static void BuildPublishFileRecords()
        {
            describeObject = UConfig.Read<VersionControl_VersionConfig>();
            var pv = describeObject.GetPV();

            bool _create = true;
            foreach (var item in pv.publishRecords)
            {
                if (item.version == describeObject.version)
                {
                    _create = false;
                    item.files = pv.files;
                    break;
                }
            }
            if (_create)
            {
                var rec = new PublishRecord();
                rec.version = describeObject.version;
                rec.files = pv.files;

                pv.publishRecords.Add(rec);
            }

            describeObject.Save();
        }

        /// <summary>
        /// 版本是否已经发布
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsPublishVersion(int version)
        {
            if (describeObject == null)
                describeObject = UConfig.Read<VersionControl_VersionConfig>();

            var pv = describeObject.GetPV();
            foreach (var item in pv.publishRecords)
                return item.version == version;
            return false;
        }

        /// <summary>
        /// 当前版本是否已经发布
        /// </summary>
        /// <returns></returns>
        public static bool IsPublishVersion()
        {
            return IsPublishVersion(UConfig.Read<VersionControl_VersionConfig>().version);
        }

        private static List<VFile> GetVersionFiles()
        {
            List<VFile> vfiles = new List<VFile>();

            // bundle
            var assetBundlePath = IOPath.PathCombine(App.TempDirectory, Platform.BuildTargetCurrentName);
            string[] files = IOPath.DirectoryGetFiles(assetBundlePath, "*" + Asset.Extension, SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var path = files[i];

                var vfile = new VFile();
                vfile.name = IOPath.FileName(path, true);
                using (var stream = File.OpenRead(path))
                {
                    vfile.length = stream.Length;
                    vfile.hash = HashUtils.GetCRC32Hash(stream);
                }
                vfiles.Add(vfile);
            }
            return vfiles;
        }
    }
}