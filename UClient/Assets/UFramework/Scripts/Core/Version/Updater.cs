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

namespace UFramework.VersionControl
{
    [MonoSingletonPath("UFramework/Updater")]
    public class Updater : MonoSingleton<Updater>, IUnzip
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

        protected override void OnSingletonAwake()
        {
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
                if (!IOPath.DirectoryExists(App.BundleDirectory))
                    IOPath.DirectoryCreate(App.BundleDirectory);

                _step = Step.CheckCopy;
            }

            if (_step == Step.CheckCopy)
                yield return CheckCopy();

            if (_step == Step.Copy)
                yield return UpdateCopy();

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
                CheckDownload();

            if (_step == Step.Download)
            {
                Debug.Log("->>> Step.Download");
            }
        }

        private IEnumerator CheckCopy()
        {
            _newVersion = Version.LoadVersion(App.versionPath);
            if (_newVersion == null)
            {
                var request = UnityWebRequest.Get(App.versionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(App.versionPath);
                yield return request.SendWebRequest();
                if (string.IsNullOrEmpty(request.error))
                {
                    var version = Version.LoadVersion(App.versionPath);
                    _maxValue = version.GetVersionInfo(version.version).baseResCount;
                    _value = 0;
                    _step = Step.Copy;
                }
                request.Dispose();
            }
            else
            {
                var request = UnityWebRequest.Get(App.versionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(App.versionOriginalPath);
                yield return request.SendWebRequest();
                if (string.IsNullOrEmpty(request.error))
                {
                    _appVersion = Version.LoadVersion(App.versionOriginalPath);
                    if (_appVersion.version > _newVersion.version)
                    {
                        _maxValue = _appVersion.GetVersionInfo(_appVersion.version).baseResCount;
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
            var request = new UnityWebRequest(IOPath.PathCombine(App.DataRawDirectory(), "res.zip"));
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (string.IsNullOrEmpty(request.error))
            {
                UZip.Unzip(new MemoryStream(request.downloadHandler.data), App.DataDirectory, null, this);
            }
            else
            {
                _step = Step.RequestVersion;
            }
            request.Dispose();
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

            var tp = App.versionPath + ".utemp";
            var request = UnityWebRequest.Get(UConfig.Read<AppConfig>().versionBaseURL + App.VersionFileName);
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
                    _newVersion = Version.LoadVersion(App.versionPath);

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
                    IOPath.FileRename(tp, App.VersionFileName);
                    _newVersion = v2;
                    yield break;
                }
            }
        }

        private IEnumerator CheckVersion()
        {
            if (_appVersion == null)
                _appVersion = Version.LoadVersion(App.versionPath);

            if (_appVersion == null)
            {
                var request = UnityWebRequest.Get(App.versionStreamingPath);
                request.downloadHandler = new DownloadHandlerFile(App.versionOriginalPath);
                yield return request.SendWebRequest();
                if (string.IsNullOrEmpty(request.error))
                {
                    _appVersion = Version.LoadVersion(App.versionOriginalPath);
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

        private void CheckDownload()
        {
            if (_newVersion == null)
                _newVersion = Version.LoadVersion(App.versionPath);

            var patchs = Version.GetDownloadPatchs(_newVersion);
            if (patchs.Count > 0) _step = Step.Download;
            else _step = Step.Completed;
        }

        #region unzip

        public bool OnPreUnzip(ZipEntry entry)
        {
            return true;
        }

        public void OnPostUnzip(ZipEntry entry)
        {
            _value++;
        }

        public void OnUnzipFinished(bool result)
        {
            if (_step == Step.Copy) _step = Step.RequestVersion;
        }

        #endregion

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}