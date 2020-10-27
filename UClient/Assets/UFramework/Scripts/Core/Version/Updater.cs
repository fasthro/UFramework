/*
 * @Author: fasthro
 * @Date: 2020-10-16 12:43:57
 * @Description: update(app/res patch)
 */
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UFramework.Config;
using UFramework.Panel.FairyGUI;
using UFramework.Coroutine;
using UFramework.Tools;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

namespace UFramework.VersionControl
{
    [MonoSingletonPath("UFramework/Updater")]
    public class Updater : MonoSingleton<Updater>
    {
        enum Step
        {
            Init,
            CheckCopy,
            Copy,
            RequestVersion,
            CheckVersion,
            CheckDownload,
            Download,
            Completed,
        }

        public float progress
        {
            get
            {
                return ((float)_value / (float)_maxValue);
            }
        }

        private UCoroutine _stepChecking;
        private Step _step;

        private int _maxValue;
        private int _value;

        private Version _newVersion;
        private Version _appVersion;

        private Tools.DownloadHandler _download;
        private List<Tools.DownloadHandler> _downloads = new List<Tools.DownloadHandler>();
        private List<VPatch> _downloadPatchs = new List<VPatch>();

        #region path

        static string streamingAssetsPath;
        static string assetBundlePath;
        static string versionPath;
        static string versionOriginalPath;
        static string versionStreamingPath;
        static string luaPath;

        static string baseURL;

        const string LUA_DIR_NAME = "Lua";

        #endregion

        protected override void OnSingletonAwake()
        {
            streamingAssetsPath = GetStreamingAssetsPath();
            assetBundlePath = IOPath.PathCombine(Application.persistentDataPath, Platform.RuntimePlatformCurrentName);
            versionPath = IOPath.PathCombine(Application.persistentDataPath, Version.FileName);
            versionOriginalPath = versionPath + "original";
            versionStreamingPath = IOPath.PathCombine(streamingAssetsPath, Version.FileName);
            luaPath = IOPath.PathCombine(Application.persistentDataPath, LUA_DIR_NAME);

            baseURL = UConfig.Read<AppConfig>().versionBaseURL;

            StartUpdate();
        }

        public void StartUpdate()
        {
            _step = Step.Init;
            if (_stepChecking != null)
                _stepChecking.Stop();

            _stepChecking = UFactoryCoroutine.CreateRun(StepChecking());
        }

        private IEnumerator StepChecking()
        {
            if (_step == Step.Init)
            {
                if (!IOPath.DirectoryExists(assetBundlePath))
                    IOPath.DirectoryCreate(assetBundlePath);

                _step = Step.CheckCopy;
            }

            if (_step == Step.CheckCopy)
                yield return CheckCopy();

            if (_step == Step.Copy)
            {
                yield return UpdateCopy();
                _step = Step.RequestVersion;
            }

            if (_step == Step.RequestVersion)
            {
                yield return RequestVersion();
                _step = Step.CheckVersion;
            }

            if (_step == Step.CheckVersion)
            {
                yield return CheckVersion();
                _step = Step.CheckDownload;
            }

            if (_step == Step.CheckDownload)
                yield return CheckDownloads();

            if (_step == Step.Download)
                _download.Download();
        }

        private IEnumerator CheckCopy()
        {
            _newVersion = Version.LoadVersion(versionPath);
            if (_newVersion == null)
            {
                var request = UnityWebRequest.Get(versionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(versionPath);
                yield return request.SendWebRequest();
                if (string.IsNullOrEmpty(request.error))
                {
                    var version = Version.LoadVersion(versionPath);
                    _maxValue = version.files.Length;
                    _value = 0;
                    _step = Step.Copy;

                    _newVersion = version;
                    _appVersion = version;
                }
                request.Dispose();
            }
            else
            {
                var request = UnityWebRequest.Get(versionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(versionOriginalPath);
                yield return request.SendWebRequest();
                if (string.IsNullOrEmpty(request.error))
                {
                    _appVersion = Version.LoadVersion(versionOriginalPath);
                    if (_appVersion.version > _newVersion.version)
                    {
                        _maxValue = _appVersion.files.Length + _appVersion.sFiles.Length;
                        _value = 0;
                        _step = Step.Copy;
                    }
                    else
                    {
                        _step = Step.RequestVersion;
                    }
                }
                else
                {
                    _step = Step.RequestVersion;
                }
                request.Dispose();
            }
        }

        private IEnumerator UpdateCopy()
        {
            for (var index = 0; index < _appVersion.files.Length; index++)
            {
                var item = _appVersion.files[index];
                var request = UnityWebRequest.Get(IOPath.PathCombine(streamingAssetsPath, Platform.RuntimePlatformCurrentName, item.name));
                request.downloadHandler = new DownloadHandlerFile(IOPath.PathCombine(assetBundlePath, item.name));
                yield return request.SendWebRequest();
                request.Dispose();
                _value++;
            }

            for (var index = 0; index < _appVersion.sFiles.Length; index++)
            {
                var item = _appVersion.sFiles[index];
                var filePath = IOPath.PathCombine(_appVersion.sDirs[item.dirIndex], item.name);
                var request = UnityWebRequest.Get(IOPath.PathCombine(streamingAssetsPath, LUA_DIR_NAME, filePath));
                request.downloadHandler = new DownloadHandlerFile(IOPath.PathCombine(luaPath, filePath));
                yield return request.SendWebRequest();
                request.Dispose();
                _value++;
            }
        }

        private IEnumerator RequestVersion()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                var mb = MessageBox.Allocate().Show("提示", "请检查网络连接状态", "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                }
                yield break;
            }

            var tp = versionPath + ".utemp";
            var request = UnityWebRequest.Get(baseURL + Version.FileName);
            request.downloadHandler = new DownloadHandlerFile(tp);
            yield return request.SendWebRequest();
            var error = request.error;
            request.Dispose();
            if (!string.IsNullOrEmpty(error))
            {
                var mb = MessageBox.Allocate().Show("提示", string.Format("获取服务器版本失败：{0}", error), "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                }
                yield break;
            }
            else
            {
                if (_newVersion == null)
                    _newVersion = Version.LoadVersion(versionPath);

                var v2 = Version.LoadVersion(tp);
                if (v2.timestamp < _newVersion.timestamp)
                {
                    var mb = MessageBox.Allocate().Show("提示", string.Format("获取服务器版本失败：联系开发人员更新远程版本文件", error), "重试", "退出");
                    yield return mb;
                    if (mb.isOk)
                    {
                        StartUpdate();
                    }
                    else
                    {
                        Quit();
                    }
                    yield break;
                }
                else
                {
                    IOPath.FileRename(tp, Version.FileName);
                    _newVersion = v2;
                    yield break;
                }
            }
        }

        private IEnumerator CheckVersion()
        {
            if (_appVersion == null)
                _appVersion = Version.LoadVersion(versionPath);

            if (_appVersion == null)
            {
                var request = UnityWebRequest.Get(versionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(versionOriginalPath);
                yield return request.SendWebRequest();
                if (string.IsNullOrEmpty(request.error))
                {
                    _appVersion = Version.LoadVersion(versionOriginalPath);
                }
            }
            if (_appVersion.version < _newVersion.minVersion)
            {
                var mb = MessageBox.Allocate().Show("提示", "有新版本发布，请更新到最新版本", "更新", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    // TODO 更新版本逻辑
                }
                else
                {
                    Quit();
                }
                yield break;
            }
        }

        private IEnumerator CheckDownloads()
        {
            if (_newVersion == null)
                _newVersion = Version.LoadVersion(versionPath);

            var versionInfo = _newVersion.GetVersionInfo(UConfig.Read<AppConfig>().version);

            _maxValue = _newVersion.files.Length + _newVersion.sFiles.Length;
            _value = 0;

            // files
            List<VFile> downloadFiles = new List<VFile>();
            for (int i = 0; i < _newVersion.files.Length; i++)
            {
                var file = _newVersion.files[i];
                var fp = IOPath.PathCombine(assetBundlePath, file.name);
                if (!IOPath.FileExists(fp))
                {
                    downloadFiles.Add(file);
                }
                else
                {
                    using (var stream = File.OpenRead(fp))
                    {
                        if (stream.Length != file.len)
                        {
                            downloadFiles.Add(file);
                        }
                        else if (!HashUtils.GetCRC32Hash(stream).Equals(file.hash, StringComparison.OrdinalIgnoreCase))
                        {
                            downloadFiles.Add(file);
                        }
                    }
                }
                yield return null;
                _value++;
            }

            // sfiles
            List<VScriptFile> downloadSFiles = new List<VScriptFile>();
            for (int i = 0; i < _newVersion.sFiles.Length; i++)
            {
                var file = _newVersion.sFiles[i];
                var fp = IOPath.PathCombine(luaPath, _newVersion.sDirs[file.dirIndex], file.name);
                if (!IOPath.FileExists(fp))
                {
                    downloadSFiles.Add(file);
                }
                else
                {
                    using (var stream = File.OpenRead(fp))
                    {
                        if (stream.Length != file.len)
                        {
                            downloadSFiles.Add(file);
                        }
                        else if (!HashUtils.GetCRC32Hash(stream).Equals(file.hash, StringComparison.OrdinalIgnoreCase))
                        {
                            downloadSFiles.Add(file);
                        }
                    }
                }
                yield return null;
                _value++;
            }

            Debug.Log("download res file count: " + downloadFiles.Count);
            Debug.Log("download script file count: " + downloadSFiles.Count);

            Dictionary<string, VPatch> map = new Dictionary<string, VPatch>();
            for (int i = 0; i < downloadFiles.Count; i++)
            {
                var dfile = downloadFiles[i];
                for (int k = 0; k < versionInfo.patchs.Length; k++)
                {
                    var patch = versionInfo.patchs[k];
                    var isChecked = false;
                    for (int n = 0; n < patch.files.Length; n++)
                    {
                        var pfile = patch.files[n];
                        if (dfile.name.Equals(pfile.name))
                        {
                            VPatch npatch = null;
                            if (map.TryGetValue(dfile.name, out npatch))
                            {
                                if (patch.timestamp > npatch.timestamp)
                                    map[dfile.name] = patch;
                            }
                            else map.Add(dfile.name, patch);
                            isChecked = true;
                            break;
                        }
                        if (isChecked) break;
                    }
                }
            }

            for (int i = 0; i < downloadSFiles.Count; i++)
            {
                var dfile = downloadSFiles[i];
                for (int k = 0; k < versionInfo.patchs.Length; k++)
                {
                    var patch = versionInfo.patchs[k];
                    var isChecked = false;
                    for (int n = 0; n < patch.sFiles.Length; n++)
                    {
                        var pfile = patch.sFiles[n];
                        if (dfile.key.Equals(pfile.key))
                        {
                            VPatch npatch = null;
                            if (map.TryGetValue(dfile.key, out npatch))
                            {
                                if (patch.timestamp > npatch.timestamp)
                                    map[dfile.key] = patch;
                            }
                            else map.Add(dfile.key, patch);
                            isChecked = true;
                            break;
                        }
                        if (isChecked) break;
                    }
                }
            }

            _downloads.Clear();
            _downloadPatchs.Clear();
            HashSet<int> dpVersions = new HashSet<int>();
            foreach (var item in map)
            {
                if (!dpVersions.Contains(item.Value.pVersion))
                {
                    _downloadPatchs.Add(item.Value);

                    var url = baseURL + item.Value.fileName;
                    var localPath = IOPath.PathCombine(Application.persistentDataPath, item.Value.fileName);
                    // _downloads.Add(Download.AddDownload(url, localPath, OnDownloadCompleted, OnDownloadProgress, null, OnDownloadFailed));
                }
            }

            if (_downloads.Count > 0) _download = _downloads[0];
            _step = _download == null ? Step.Completed : Step.Download;
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private string GetStreamingAssetsPath()
        {
            if (Application.platform == RuntimePlatform.Android)
                return Application.streamingAssetsPath;

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                return "file:///" + Application.streamingAssetsPath;

            return "file://" + Application.streamingAssetsPath;
        }
    }
}