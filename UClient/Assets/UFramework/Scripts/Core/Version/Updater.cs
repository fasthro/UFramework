// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-10-16 12:43:57
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections;
using System.IO;
using UFramework.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

namespace UFramework.Core
{
    public enum UpdaterStep
    {
        Init,
        CheckVersionCopy,
        CheckFileCopy,
        Copy,
        RequestVersion,
        CheckVersion,
        CheckDownload,
        OptionDownload,
        Download,
        Completed,
    }

    /// <summary>
    /// 更新器接口
    /// </summary>
    public interface IUpdater
    {
        /// <summary>
        /// 开始更新流程
        /// </summary>
        void OnStartUpdate();

        /// <summary>
        /// 更新状态切换
        /// </summary>
        /// <param name="step"></param>
        void OnStep(UpdaterStep step);

        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="progress"></param>
        void OnProgress(float progress);

        /// <summary>
        /// 更新完成
        /// </summary>
        void OnEndUpdate();
    }

    [MonoSingletonPath("UFramework/Updater")]
    public class Updater : MonoSingleton<Updater>
    {
        const string LUA_DIR_NAME = "Lua";
        const string BUILD_FILE_DIR_NAME = "Files";

        static string StreamingAssetsPath;
        static string AssetBundlePath;
        static string VersionPath;
        static string VersionOriginalPath;
        static string VersionStreamingPath;
        static string LuaPath;
        static string BuildFilePath;

        static string BaseURL;

        public float progress => ((float) _value / (float) _maxValue);

        public int versionCode
        {
            get
            {
                if (_newVersion != null) return _newVersion.version;
                else if (_appVersion != null) return _appVersion.version;
                else return Core.Serializer<AppConfig>.Instance.version;
            }
        }

        public int patchVersionCode
        {
            get
            {
                var v = _newVersion ?? _appVersion;
                if (v != null)
                {
                    var c = v.GetVersionInfo(v.version).patchs.Length;
                    if (c == 0) return -1;
                    return c - 1;
                }
                else return -1;
            }
        }

        private IUpdater _listener;
        private Action _onCompleted;
        private UpdaterStep _step;

        private AppConfig _app;

        private int _maxValue;
        private int _value;

        private Version _newVersion;
        private Version _appVersion;

        private List<VFile> _repairFiles = new List<VFile>();
        private List<VScriptFile> _repairSFiles = new List<VScriptFile>();
        private List<VBuildFile> _repairBFiles = new List<VBuildFile>();

        private PatchDownloader _downloader;

        protected override void OnSingletonAwake()
        {
            _app = Core.Serializer<AppConfig>.Instance;

            if (_app.isDevelopmentVersion)
                return;

            BaseURL = _app.versionBaseURL;

            StreamingAssetsPath = GetStreamingAssetsPath();
            AssetBundlePath = IOPath.PathCombine(Application.persistentDataPath, Platform.RuntimePlatformCurrentName);
            VersionPath = IOPath.PathCombine(Application.persistentDataPath, Version.FileName);
            VersionOriginalPath = VersionPath + ".original";
            VersionStreamingPath = IOPath.PathCombine(StreamingAssetsPath, Version.FileName, false);
            LuaPath = IOPath.PathCombine(Application.persistentDataPath, LUA_DIR_NAME);
            BuildFilePath = IOPath.PathCombine(Application.persistentDataPath, BUILD_FILE_DIR_NAME);

            _downloader = new PatchDownloader(BaseURL, Application.persistentDataPath, this.OnCompleted, this.OnPatchDownloadFailed);
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (_step == UpdaterStep.CheckFileCopy || _step == UpdaterStep.Copy || _step == UpdaterStep.Download)
                _listener?.OnProgress(progress);
        }

        /// <summary>
        /// 开始执行版本更新
        /// </summary>
        /// <param name="listener">更新接口</param>
        /// <param name="onCompleted">更新完成事件回调</param>
        public void StartUpdate(IUpdater listener = null, Action onCompleted = null)
        {
            _onCompleted = onCompleted;
            _listener = listener;
            _listener?.OnStartUpdate();

            if (_app.isDevelopmentVersion)
            {
                _listener?.OnEndUpdate();
                _onCompleted.InvokeGracefully();
            }
            else
            {
                StepChecking(UpdaterStep.Init);
                StepChecking(UpdaterStep.CheckVersionCopy);
            }
        }

        private void StepChecking(UpdaterStep step)
        {
            _listener?.OnStep(step);

            _step = step;
            switch (_step)
            {
                case UpdaterStep.Init:
                    if (!IOPath.DirectoryExists(AssetBundlePath))
                        IOPath.DirectoryCreate(AssetBundlePath);
                    break;
                case UpdaterStep.CheckVersionCopy:
                    Coroutine.Allocate(CheckVersionCopy());
                    break;
                case UpdaterStep.CheckFileCopy:
                    Coroutine.Allocate(CheckFileCopy());
                    break;
                case UpdaterStep.Copy:
                    Coroutine.Allocate(UpdateCopy());
                    break;
                case UpdaterStep.RequestVersion:
                    Coroutine.Allocate(RequestVersion());
                    break;
                case UpdaterStep.CheckVersion:
                    Coroutine.Allocate(CheckVersion());
                    break;
                case UpdaterStep.CheckDownload:
                    Coroutine.Allocate(CheckDownloads());
                    break;
                case UpdaterStep.OptionDownload:
                    Coroutine.Allocate(OptionDownloadPatch());
                    break;
            }
        }

        private IEnumerator CheckVersionCopy()
        {
            LoadNewVersion(true);

            var request = UnityWebRequest.Get(VersionStreamingPath);
            request.downloadHandler = new DownloadHandlerFile(VersionOriginalPath);
            yield return request.SendWebRequest();
            if (request.isDone && string.IsNullOrEmpty(request.error))
            {
                var version = Version.LoadVersion(VersionOriginalPath);
                _maxValue = version.files.Length + version.sFiles.Length + version.bFiles.Length;
                _value = 0;
                _step = UpdaterStep.Copy;

                _newVersion = _newVersion ?? version;
                _appVersion = version;
                request.Dispose();

                if (_appVersion.version > _newVersion.version) StepChecking(UpdaterStep.Copy);
                else StepChecking(UpdaterStep.CheckFileCopy);
            }
            else
            {
                var mb = MessageBox.Allocate().Show("提示", $"内部版本信息读取错误， 联系开发人员\n{request.error}", "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate(_listener, _onCompleted);
                }
                else
                {
                    Quit();
                }

                request.Dispose();
                yield break;
            }
        }

        private IEnumerator CheckFileCopy()
        {
            for (var index = 0; index < _appVersion.files.Length; index++)
            {
                var item = _appVersion.files[index];
                var file = IOPath.PathCombine(IOPath.PathCombine(AssetBundlePath, item.name));
                if (!IOPath.FileExists(file))
                    _repairFiles.Add(item);
                yield return null;
                _value++;
            }

            for (var index = 0; index < _appVersion.sFiles.Length; index++)
            {
                var item = _appVersion.sFiles[index];
                var file = IOPath.PathCombine(LuaPath, _appVersion.sDirs[item.dirIndex], item.name);
                if (!IOPath.FileExists(file))
                    _repairSFiles.Add(item);
                yield return null;
                _value++;
            }

            for (var index = 0; index < _appVersion.bFiles.Length; index++)
            {
                var item = _appVersion.bFiles[index];
                var file = IOPath.PathCombine(BuildFilePath, _appVersion.bDirs[item.dirIndex], item.name);
                if (!IOPath.FileExists(file))
                    _repairBFiles.Add(item);
                yield return null;
                _value++;
            }

            if (_repairFiles.Count > 0 || _repairSFiles.Count > 0 || _repairBFiles.Count > 0)
                _step = UpdaterStep.Copy;
            else _step = UpdaterStep.RequestVersion;

            StepChecking(_step);
        }

        /// <summary>
        /// 解压文件到持久化目录
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateCopy()
        {
            if (_repairFiles.Count == 0) _repairFiles.AddRange(_appVersion.files);
            if (_repairSFiles.Count == 0) _repairSFiles.AddRange(_appVersion.sFiles);
            if (_repairBFiles.Count == 0) _repairBFiles.AddRange(_appVersion.bFiles);

            _value = 0;
            _maxValue = _repairFiles.Count + _repairSFiles.Count + _repairBFiles.Count;
        
            // res files
            for (var index = 0; index < _repairFiles.Count; index++)
            {
                var item = _repairFiles[index];
                var request = UnityWebRequest.Get(IOPath.PathCombine(StreamingAssetsPath, Platform.RuntimePlatformCurrentName, item.name, false));
                request.downloadHandler = new DownloadHandlerFile(IOPath.PathCombine(AssetBundlePath, item.name));
                yield return request.SendWebRequest();
                request.Dispose();
                _value++;
            }
            
            // script files
            for (var index = 0; index < _repairSFiles.Count; index++)
            {
                var item = _repairSFiles[index];
                var filePath = IOPath.PathCombine(_appVersion.sDirs[item.dirIndex], item.name);
                var request = UnityWebRequest.Get(IOPath.PathCombine(StreamingAssetsPath, LUA_DIR_NAME, filePath, false));
                request.downloadHandler = new DownloadHandlerFile(IOPath.PathCombine(LuaPath, filePath));
                yield return request.SendWebRequest();
                request.Dispose();
                _value++;
            }
            
            // build files
            for (var index = 0; index < _repairBFiles.Count; index++)
            {
                var item = _repairBFiles[index];
                var filePath = IOPath.PathCombine(_appVersion.bDirs[item.dirIndex], item.name);
                var request = UnityWebRequest.Get(IOPath.PathCombine(StreamingAssetsPath, BUILD_FILE_DIR_NAME, filePath, false));
                request.downloadHandler = new DownloadHandlerFile(IOPath.PathCombine(BuildFilePath, filePath));
                yield return request.SendWebRequest();
                request.Dispose();
                _value++;
            }

            StepChecking(UpdaterStep.RequestVersion);
        }

        /// <summary>
        /// 请求远程版本信息
        /// </summary>
        /// <returns></returns>
        private IEnumerator RequestVersion()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                var mb = MessageBox.Allocate().Show("提示", "请检查网络连接状态", "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate(_listener, _onCompleted);
                }
                else
                {
                    Quit();
                }

                yield break;
            }

            var tp = VersionPath + ".utemp";
            var request = UnityWebRequest.Get(BaseURL + Version.FileName);
            request.downloadHandler = new DownloadHandlerFile(tp);
            yield return request.SendWebRequest();
            var error = request.error;
            request.Dispose();
            if (!string.IsNullOrEmpty(error))
            {
                var mb = MessageBox.Allocate().Show("提示", $"获取服务器版本失败：{error}", "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate(_listener, _onCompleted);
                }
                else
                {
                    Quit();
                }
            }
            else
            {
                LoadNewVersion();

                var v2 = Version.LoadVersion(tp);
                if (v2.timestamp < _newVersion.timestamp || v2.version < _appVersion.version)
                {
                    var mb = MessageBox.Allocate().Show("提示", string.Format("获取服务器版本失败：联系开发人员更新远程版本文件", error), "重试", "退出");
                    yield return mb;
                    if (mb.isOk)
                    {
                        StartUpdate(_listener, _onCompleted);
                    }
                    else
                    {
                        Quit();
                    }
                }
                else
                {
                    IOPath.FileRename(tp, Version.FileName);
                    _newVersion = v2;

                    StepChecking(UpdaterStep.CheckVersion);
                }
            }
        }

        /// <summary>
        /// 检查应用版本
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckVersion()
        {
            // app
            LoadAppVersion();

            if (_appVersion == null)
            {
                var request = UnityWebRequest.Get(VersionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(VersionOriginalPath);
                yield return request.SendWebRequest();
                if (request.isDone && string.IsNullOrEmpty(request.error))
                    LoadAppVersion(true);
            }

            // new
            LoadNewVersion();

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
            }
            else StepChecking(UpdaterStep.CheckDownload);
        }

        /// <summary>
        /// 检查本地资源是否需要更新
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckDownloads()
        {
            LoadNewVersion();

            var versionInfo = _newVersion.GetVersionInfo(_app.version);

            _maxValue = _newVersion.files.Length + _newVersion.sFiles.Length + _newVersion.bFiles.Length;
            _value = 0;

            // files
            var downloadFiles = new List<VFile>();
            for (var i = 0; i < _newVersion.files.Length; i++)
            {
                var file = _newVersion.files[i];
                var fp = IOPath.PathCombine(AssetBundlePath, file.name);
                if (!IOPath.FileExists(fp))
                {
                    Logger.Debug("add download file not exist: " + fp);
                    downloadFiles.Add(file);
                }
                else
                {
                    using (var stream = File.OpenRead(fp))
                    {
                        if (stream.Length != file.len)
                        {
                            Logger.Debug("add download file len: " + fp);
                            downloadFiles.Add(file);
                        }
                        else if (!HashUtils.GetCRC32Hash(stream).Equals(file.hash, StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Debug("add download file hash: " + fp);
                            downloadFiles.Add(file);
                        }
                    }
                }

                yield return null;
                _value++;
            }

            // sfiles
            var downloadSFiles = new List<VScriptFile>();
            for (var i = 0; i < _newVersion.sFiles.Length; i++)
            {
                var file = _newVersion.sFiles[i];
                var fp = IOPath.PathCombine(LuaPath, _newVersion.sDirs[file.dirIndex], file.name);
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

            // bfiles
            var downloadBFiles = new List<VBuildFile>();
            for (var i = 0; i < _newVersion.bFiles.Length; i++)
            {
                var file = _newVersion.bFiles[i];
                var fp = IOPath.PathCombine(BuildFilePath, _newVersion.bDirs[file.dirIndex], file.name);
                if (!IOPath.FileExists(fp))
                {
                    downloadBFiles.Add(file);
                }
                else
                {
                    using (var stream = File.OpenRead(fp))
                    {
                        if (stream.Length != file.len)
                        {
                            downloadBFiles.Add(file);
                        }
                        else if (!HashUtils.GetCRC32Hash(stream).Equals(file.hash, StringComparison.OrdinalIgnoreCase))
                        {
                            downloadBFiles.Add(file);
                        }
                    }
                }

                yield return null;
                _value++;
            }

            Logger.Debug("download res file count: " + downloadFiles.Count);
            Logger.Debug("download script file count: " + downloadSFiles.Count);
            Logger.Debug("download build file count: " + downloadBFiles.Count);

            var map = new Dictionary<string, VPatch>();
            for (var i = 0; i < downloadFiles.Count; i++)
            {
                var dfile = downloadFiles[i];
                for (var k = 0; k < versionInfo.patchs.Length; k++)
                {
                    var patch = versionInfo.patchs[k];
                    for (var n = 0; n < patch.files.Length; n++)
                    {
                        var pfile = patch.files[n];
                        if (dfile.name.Equals(pfile.name))
                        {
                            if (map.TryGetValue(dfile.name, out var npatch))
                            {
                                if (patch.timestamp > npatch.timestamp)
                                    map[dfile.name] = patch;
                            }
                            else map.Add(dfile.name, patch);
                            break;
                        }
                    }
                }
            }

            for (var i = 0; i < downloadSFiles.Count; i++)
            {
                var dfile = downloadSFiles[i];
                for (var k = 0; k < versionInfo.patchs.Length; k++)
                {
                    var patch = versionInfo.patchs[k];
                    for (var n = 0; n < patch.sFiles.Length; n++)
                    {
                        var pfile = patch.sFiles[n];
                        if (dfile.key.Equals(pfile.key))
                        {
                            if (map.TryGetValue(dfile.key, out var npatch))
                            {
                                if (patch.timestamp > npatch.timestamp)
                                    map[dfile.key] = patch;
                            }
                            else map.Add(dfile.key, patch);
                            break;
                        }
                    }
                }
            }

            for (var i = 0; i < downloadBFiles.Count; i++)
            {
                var dfile = downloadBFiles[i];
                for (var k = 0; k < versionInfo.patchs.Length; k++)
                {
                    var patch = versionInfo.patchs[k];
                    for (var n = 0; n < patch.sFiles.Length; n++)
                    {
                        var pfile = patch.sFiles[n];
                        if (dfile.key.Equals(pfile.key))
                        {
                            if (map.TryGetValue(dfile.key, out var npatch))
                            {
                                if (patch.timestamp > npatch.timestamp)
                                    map[dfile.key] = patch;
                            }
                            else map.Add(dfile.key, patch);
                            break;
                        }
                    }
                }
            }

            var dpVersions = new HashSet<string>();
            foreach (var item in map)
            {
                if (!dpVersions.Contains(item.Value.key))
                {
                    dpVersions.Add(item.Value.key);
                    _downloader.AddPatch(item.Value);

                    Logger.Debug("download patch: " + item.Value.fileName);
                }
            }

            dpVersions.Clear();
            map.Clear();
            downloadFiles.Clear();
            downloadSFiles.Clear();
            downloadBFiles.Clear();

            if (_downloader.count > 0) StepChecking(UpdaterStep.OptionDownload);
            else OnCompleted();
        }

        /// <summary>
        /// 确认是否下载补丁
        /// </summary>
        /// <returns></returns>
        private IEnumerator OptionDownloadPatch()
        {
            var mb = MessageBox.Allocate().Show("提示", $"有新资源更新，是否下载新资源\n资源大小：{_downloader.GetFormatDownloadSize()}", "更新", "退出");
            yield return mb;
            if (mb.isOk)
            {
                _step = UpdaterStep.Download;
                _downloader.StartDownload();
            }
            else Quit();

            yield break;
        }

        private IEnumerator UpdaterFailed()
        {
            var mb = MessageBox.Allocate().Show("提示", string.Format("版本更新失败，请联系开发人员", _downloader.GetFormatDownloadSize()), "重试", "退出");
            yield return mb;
            if (mb.isOk)
            {
                StartUpdate(_listener, _onCompleted);
            }
            else
            {
                Quit();
            }

            yield break;
        }

        /// <summary>
        /// 补丁下载失败
        /// </summary>
        private void OnPatchDownloadFailed()
        {
            Coroutine.Allocate(UpdaterFailed());
        }

        private void OnCompleted()
        {
            StepChecking(UpdaterStep.Completed);
            _listener?.OnEndUpdate();
            _onCompleted.InvokeGracefully();
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LoadNewVersion(bool force = false)
        {
            if (force || _newVersion == null)
                _newVersion = Version.LoadVersion(VersionPath);
        }

        private void LoadAppVersion(bool force = false)
        {
            if (force || _appVersion == null)
                _appVersion = Version.LoadVersion(VersionOriginalPath);
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