/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: version
 */

using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl.Version
{
    public class VersionPage : IPage, IPageBar
    {
        public string menuName { get { return "Version"; } }
        static VersionControl_Version_Config Config { get { return Core.Serializer<VersionControl_Version_Config>.Instance; } }

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

        [ShowInInspector]
        [TabGroup("Current Version Patch")]
        [LabelText("Patchs")]
        [ListDrawerSettings(CustomAddFunction = "CustomAddFunction_Patchs", CustomRemoveIndexFunction = "CustomRemoveIndexFunction_Patchs")]
        public List<VEditorPatch> patchs = new List<VEditorPatch>();

        private void CustomAddFunction_Patchs()
        {
            foreach (var item in patchs)
                if (!item.isRelease) return;

            var patch = new VEditorPatch();
            patch.aVersion = Config.version;
            patch.pVersion = patchs.Count;
            patch.timestamp = TimeUtils.UTCTimeStamps();
            patch.UpdateDisplay();
            patchs.Add(patch);
            patchs.Sort((a, b) => b.pVersion.CompareTo(a.pVersion));

            var pv = Config.GetPlatformVersion();
            foreach (var item in pv.supports)
            {
                var _patch = new VEditorPatch();
                _patch.aVersion = Config.version;
                _patch.pVersion = item.patchs.Count;
                _patch.timestamp = TimeUtils.UTCTimeStamps();
                _patch.UpdateDisplay();

                item.patchs.Add(_patch);
                item.patchs.Sort((a, b) => b.pVersion.CompareTo(a.pVersion));
            }
        }

        private void CustomRemoveIndexFunction_Patchs(int index)
        {
            if (patchs[index].isRelease) return;
            patchs.RemoveAt(index);
            patchs.Sort((a, b) => b.pVersion.CompareTo(a.pVersion));
        }

        [ShowInInspector, HideLabel, ReadOnly]
        [TabGroup("Support Versions")]
        [DictionaryDrawerSettings(KeyLabel = "Version Code", ValueLabel = "Patch", DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<int, VEditorInfo> supportDictionary = new Dictionary<int, VEditorInfo>();

        /// <summary>
        /// 版本信息列表
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [TabGroup("History Versions")]
        [DictionaryDrawerSettings(KeyLabel = "Version Code", ValueLabel = "Patch", DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<int, VEditorInfo> historyDictionary = new Dictionary<int, VEditorInfo>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            var pv = Config.GetPlatformVersion();

            version = Config.version;
            minVersion = Config.minVersion;
            patchs = pv.patchs;
            patchs.Sort((a, b) => b.pVersion.CompareTo(a.pVersion));

            supportDictionary.Clear();
            foreach (var item in pv.supports)
                supportDictionary.Add(item.version, item);

            historyDictionary.Clear();
            foreach (var item in pv.historys)
                historyDictionary.Add(item.version, item);
        }

        public void OnSaveDescribe()
        {
            var pv = Config.GetPlatformVersion();

            Config.version = version;
            Config.minVersion = minVersion;
            pv.patchs = patchs;

            pv.supports.Clear();
            foreach (var item in supportDictionary)
                pv.supports.Add(item.Value);

            pv.historys.Clear();
            foreach (var item in historyDictionary)
                pv.historys.Add(item.Value);

            Config.Serialize();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("New Version")))
            {
                foreach (var item in patchs)
                {
                    if (!item.isRelease)
                    {
                        EditorUtility.DisplayDialog("Version", "当前版本中补丁尚未发布, 无法创建新版本.", "确定");
                        return;
                    }
                }

                if (EditorUtility.DisplayDialog("Version", "确定创建新版本?", "确定", "取消"))
                {
                    CreateNewVersion();
                }
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Remove Current Version")))
            {
                if (EditorUtility.DisplayDialog("Version", "确定移除当前版本?", "确定", "取消"))
                {
                    RemoveCurrentVersion();
                }
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Remove All Version")))
            {
                if (EditorUtility.DisplayDialog("Version", "确定移除所有版本?", "确定", "取消"))
                {
                    RemoveAllVersion();
                }
            }
        }

        private void CreateNewVersion()
        {
            var info = new VEditorInfo();
            info.version = version;
            info.patchs.Clear();
            info.patchs.AddRange(patchs);
            supportDictionary.Add(version, info);

            version++;
            patchs.Clear();
        }

        private void RemoveCurrentVersion()
        {
            patchs.Clear();

            var target = version - 1;
            if (target < 0) return;

            VEditorInfo info = null;
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

            Config.GetPlatformVersion().releaseVersionCodes.Clear();
            Config.Serialize();
        }

        private void OnMinVersionChange()
        {
            if (minVersion > version) minVersion = version;
            if (minVersion < 0) minVersion = 0;

            List<int> _removes = new List<int>();
            foreach (KeyValuePair<int, VEditorInfo> item in supportDictionary)
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
        /// <param name="path"></param>
        public static void BuildVersion(string path)
        {
            var pv = Config.GetPlatformVersion();

            var ver = new Core.Version();
            ver.version = Config.version;
            ver.minVersion = Config.minVersion;
            ver.timestamp = TimeUtils.UTCTimeStamps();
            CheckVersionFiles(out ver.files);
            CheckVersionScripts(out ver.sDirs, out ver.sFiles);
            ver.versionDict.Clear();

            var vInfo = new VInfo();
            vInfo.version = Config.version;
            vInfo.patchs = new VPatch[pv.patchs.Count];
            for (int i = 0; i < pv.patchs.Count; i++)
            {
                var item = pv.patchs[i];
                var patch = new VPatch();
                patch.aVersion = item.aVersion;
                patch.pVersion = item.pVersion;
                patch.timestamp = item.timestamp;
                patch.files = item.files.ToArray();
                patch.sFiles = item.sFiles.ToArray();
                patch.len = item.len;
                vInfo.patchs[i] = patch;
            }
            ver.versionDict.Add(vInfo.version, vInfo);

            foreach (var item in pv.supports)
            {
                var info = new VInfo();
                info.version = item.version;
                info.patchs = new VPatch[pv.patchs.Count];
                for (int i = 0; i < pv.patchs.Count; i++)
                {
                    var itemPatch = pv.patchs[i];
                    var patch = new VPatch();
                    patch.aVersion = itemPatch.aVersion;
                    patch.pVersion = itemPatch.pVersion;
                    patch.timestamp = itemPatch.timestamp;
                    patch.files = itemPatch.files.ToArray();
                    patch.sFiles = itemPatch.sFiles.ToArray();
                    patch.len = itemPatch.len;
                    info.patchs[i] = patch;
                }
                ver.versionDict.Add(item.version, info);
            }

            Core.Version.VersionWrite(path, ver);
        }

        /// <summary>
        /// 构建版本发布记录
        /// </summary>
        public static void BuildReleaseRecords()
        {
            var pv = Config.GetPlatformVersion();

            bool _create = true;
            foreach (var item in pv.releaseVersionCodes)
            {
                if (item == Config.version)
                {
                    _create = false;
                    break;
                }
            }
            if (_create)
                pv.releaseVersionCodes.Add(Config.version);

            Config.Serialize();
        }

        /// <summary>
        /// 版本是否已经发布
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsPublishVersion(int version)
        {
            var pv = Config.GetPlatformVersion();
            foreach (var item in pv.releaseVersionCodes)
                return item == version;
            return false;
        }

        /// <summary>
        /// 当前版本是否已经发布
        /// </summary>
        /// <returns></returns>
        public static bool IsPublishVersion()
        {
            return IsPublishVersion(Config.version);
        }

        /// <summary>
        /// 检查版本文件
        /// </summary>
        /// <param name="tFiles"></param>
        public static void CheckVersionFiles(out VFile[] tFiles)
        {
            List<VFile> vfiles = new List<VFile>();

            var assetBundlePath = IOPath.PathCombine(UApplication.TempDirectory, Platform.BuildTargetCurrentName);
            string[] files = IOPath.DirectoryGetFiles(assetBundlePath, "*" + Assets.Extension, SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var path = files[i];

                var vfile = new VFile();
                vfile.name = IOPath.FileName(path, true);
                using (var stream = File.OpenRead(path))
                {
                    vfile.len = stream.Length;
                    vfile.hash = HashUtils.GetCRC32Hash(stream);
                }
                vfiles.Add(vfile);
            }
            tFiles = vfiles.ToArray();
        }

        /// <summary>
        /// 检查版本脚本文件
        /// </summary>
        /// <param name="tDirs"></param>
        /// <param name="tFiles"></param>
        public static void CheckVersionScripts(out string[] tDirs, out VScriptFile[] tFiles)
        {
            List<string> dirs = new List<string>();
            List<VScriptFile> sFiles = new List<VScriptFile>();

            var tp = IOPath.PathCombine(UApplication.TempDirectory, "Lua");
            string[] files = IOPath.DirectoryGetFiles(tp, "*.lua", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var fp = files[i];
                var dir = IOPath.PathUnitySeparator(Path.GetDirectoryName(fp));
                var index = dirs.FindIndex(o => o.Equals(dir));
                if (index == -1)
                {
                    index = dirs.Count;
                    dirs.Add(dir);
                }

                var file = new VScriptFile() { name = Path.GetFileName(fp), dirIndex = index };
                using (var stream = File.OpenRead(fp))
                {
                    file.len = stream.Length;
                    file.hash = HashUtils.GetCRC32Hash(stream);
                }
                sFiles.Add(file);
            }

            for (int i = 0; i < dirs.Count; i++)
                dirs[i] = dirs[i].Replace(tp, "").TrimStart('/').TrimStart('\\');

            tDirs = dirs.ToArray();
            tFiles = sFiles.ToArray();
        }
    }
}