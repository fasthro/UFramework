/*
 * @Author: fasthro
 * @Date: 2020-08-31 11:28:06
 * @Description: google play obb downloader
 */
using UnityEngine;
using System;
namespace UFramework.NativePlatform
{
    /// <summary>
    /// status
    /// </summary>
    public enum ObbDownloaderStatus
    {
        /// <summary>
        /// 不需要下载 obb 扩展文件
        /// </summary>
        NoDownloadRequired = 0,

        /// <summary>
        /// 需要下载 Obb 扩展文件
        /// </summary>
        DownloadRequired = 1,

        /// <summary>
        /// Error（外部存储权限不可用 or ...）
        /// </summary>
        Error = 2,
    }

    public class ObbDownloader
    {
        static AndroidJavaClass _ObbDownloaderNativeClass;

        /// <summary>
        /// obb downloader
        /// </summary>
        public static AndroidJavaClass ObbDownloaderNativeClass
        {
            get
            {
                if (_ObbDownloaderNativeClass == null)
                {
                    _ObbDownloaderNativeClass = new AndroidJavaClass(AndroidNative.MAIN_PACKAGE + ".obbdownloader.ObbDownloader");
                }
                return _ObbDownloaderNativeClass;
            }
        }

        /// <summary>
        /// android.os.Environment
        /// </summary>
        /// <returns></returns>
        static AndroidJavaClass EnvironmentClass = new AndroidJavaClass("android.os.Environment");

        static string _ExpansionFileDirectory;

        /// <summary>
        /// 扩展文件目录
        /// </summary>
        /// <returns></returns>
        public static string ExpansionFileDirectory
        {
            get
            {
                if (EnvironmentClass.CallStatic<string>("getExternalStorageState") != "mounted")
                {
                    _ExpansionFileDirectory = null;
                }
                else if (string.IsNullOrEmpty(_ExpansionFileDirectory))
                {
                    const string obbPath = "Android/obb";
                    using (var externalStorageDirectory = EnvironmentClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
                    {
                        var externalRoot = externalStorageDirectory.Call<string>("getPath");
                        _ExpansionFileDirectory = string.Format("{0}/{1}/{2}", externalRoot, obbPath, AndroidNative.PackageName);
                    }
                }
                return _ExpansionFileDirectory;
            }
        }

        static string _MainObbPath;

        /// <summary>
        /// main obb path
        /// </summary>
        /// <value></value>
        public static string MainObbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MainObbPath))
                {
                    _MainObbPath = GetOBBPath("main");
                }
                return _MainObbPath;
            }
        }

        static string _PatchObbPath;

        /// <summary>
        /// patch obb path
        /// </summary>
        /// <value></value>
        public static string PatchObbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_PatchObbPath))
                {
                    _PatchObbPath = GetOBBPath("patch");
                }
                return _PatchObbPath;
            }
        }

        /// <summary>
        /// 扩展文件下载回调适配器
        /// </summary>
        static ObbDownloadListenerAdapter Adapter;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            ObbDownloaderNativeClass.CallStatic("initialize", AndroidNative.Context, false);
            ObbDownloaderNativeClass.SetStatic("PUBLIC_KEY", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwINnSEZSgRJL+ASiU/iF/a2RSC/hzDG/PB2bQrxFWN0BURjhZH2UV6ysT7sjIv57CHulOq31VHyBjE5kQyivhcK2sOiHByyH8z3aLn2lxnAXAA38Cb0QbL3PV6Gifl29lyxtrGcAlO/fI5GxqLvwT312g/SeOncXroLQwVxFW/EPWjz9PrJ6R4BL5ivs1Us2/NW1eWh89d+aWp0LSbBXq0YUoiqV5Nju1k9toqOYH1Ztxp308RmhNx31iPowiwOlTU0WRUZzVifmz27BbjSW/ufWPYPWhBKafQCDIt5UdkZ82zsbkrf7/T8k+FSJxLMJBdq7muruXvF/QXBh8MpHkwIDAQAB");
            ObbDownloaderNativeClass.SetStatic("SALT", new byte[] { 1, 43, 256 - 12, 256 - 1, 54, 98, 256 - 100, 256 - 12, 43, 2, 256 - 8, 256 - 4, 9, 5, 256 - 106, 256 - 108, 256 - 33, 45, 256 - 1, 84 });
            ObbDownloaderNativeClass.SetStatic("MAIN_OBB_VERSION", AndroidNative.VersionCode);
            // TODO Patch Obb
            // obbDownloaderNative.SetStatic("PATCH_OBB_VERSION", AndroidNative.VersionCode);
        }

        /// <summary>
        /// connect
        /// </summary>
        public static void Connect()
        {
            ObbDownloaderNativeClass.CallStatic("connect");
        }

        /// <summary>
        /// disconnect
        /// </summary>
        public static void Disconnect()
        {
            ObbDownloaderNativeClass.CallStatic("disconnect");
        }

        /// <summary>
        /// 扩展文件状态
        /// </summary>
        /// <returns></returns>
        public static ObbDownloaderStatus ExpansionFileStatus()
        {
            // 外部存储不可用
            if (string.IsNullOrEmpty(ExpansionFileDirectory))
            {
                return ObbDownloaderStatus.Error;
            }
            // TODO Patch Obb
            // else if (string.IsNullOrEmpty(MainObbPath) || string.IsNullOrEmpty(PatchObbPath))
            else if (string.IsNullOrEmpty(MainObbPath))
            {
                return ObbDownloaderStatus.DownloadRequired;
            }
            return ObbDownloaderStatus.NoDownloadRequired;
        }

        #region 下载扩展文件文件

        /// <summary>
        /// 设置下载扩展文件回调
        /// </summary>
        /// <param name="listener"></param>
        public static void SetDownloadListener(IObbDownloadListener listener)
        {
            if (listener != null)
            {
                Adapter = new ObbDownloadListenerAdapter(listener);
                ObbDownloaderNativeClass.CallStatic("setDownloadListener", Adapter);
            }
        }

        /// <summary>
        /// 下载扩展文件
        /// </summary>
        /// <param name="listener"></param>
        public static void DownloadExpansion()
        {
            ObbDownloaderNativeClass.CallStatic("downloadExpansion");
        }

        /// <summary>
        /// 继续下载扩展文件
        /// </summary>
        public static void ContinueDownload()
        {
            ObbDownloaderNativeClass.CallStatic("continueDownload");
        }

        /// <summary>
        /// 暂停下载扩展文件
        /// </summary>
        public static void PauseDownload()
        {
            ObbDownloaderNativeClass.CallStatic("pauseDownload");
        }

        /// <summary>
        /// 取消下载扩展文件
        /// </summary>
        public static void AbortDownload()
        {
            ObbDownloaderNativeClass.CallStatic("abortDownload");
        }

        #endregion

        /// <summary>
        /// Obb Path
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        static string GetOBBPath(string prefix)
        {
            if (string.IsNullOrEmpty(ExpansionFileDirectory))
                return null;

            var filePath = string.Format("{0}/{1}.{2}.{3}.obb", ExpansionFileDirectory, prefix, AndroidNative.VersionCode, AndroidNative.PackageName);
            return IOPath.FileExists(filePath) ? filePath : null;
        }

    }
}