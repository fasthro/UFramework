/*
 * @Author: fasthro
 * @Date: 2020-08-31 11:28:06
 * @Description: google play obb downloader
 */
using UnityEngine;
using System;
namespace UFramework.Native.Service
{
    /// <summary>
    /// status
    /// </summary>
    public enum ObbDownloaderStatus
    {
        None = 0,

        /// <summary>
        /// 没有权限（外部存储权限不可用）
        /// </summary>
        PermissionsError,

        /// <summary>
        /// Obb 文件不存在
        /// </summary>
        ObbNotExist,
    }

    public class ObbDownloader
    {
        private static AndroidJavaClass _obbDownloaderNative;

        /// <summary>
        /// obb downloader
        /// </summary>
        public static AndroidJavaClass obbDownloaderNative
        {
            get
            {
                if (_obbDownloaderNative == null)
                {
                    _obbDownloaderNative = new AndroidJavaClass(NativeAndroid.MAIN_PACKAGE + ".obbdownloader.ObbDownloader");
                }
                return _obbDownloaderNative;
            }
        }

        /// <summary>
        /// android.os.Environment
        /// </summary>
        /// <returns></returns>
        private static AndroidJavaClass environmentClass = new AndroidJavaClass("android.os.Environment");

        private static string _expansionFileDirectory;

        /// <summary>
        /// 扩展文件目录
        /// </summary>
        /// <returns></returns>
        public static string ExpansionFileDirectory
        {
            get
            {
                if (environmentClass.CallStatic<string>("getExternalStorageState") != "mounted")
                {
                    _expansionFileDirectory = null;
                }
                else if (string.IsNullOrEmpty(_expansionFileDirectory))
                {
                    const string obbPath = "Android/obb";
                    using (var externalStorageDirectory = environmentClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
                    {
                        var externalRoot = externalStorageDirectory.Call<string>("getPath");
                        _expansionFileDirectory = string.Format("{0}/{1}/{2}", externalRoot, obbPath, NativeAndroid.PackageName);
                    }
                }
                return _expansionFileDirectory;
            }
        }

        private static string _mainObbPath;

        /// <summary>
        /// main obb path
        /// </summary>
        /// <value></value>
        public static string MainObbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_mainObbPath))
                {
                    _mainObbPath = GetOBBPath("main");
                }
                return _mainObbPath;
            }
        }

        private static string _patchObbPath;

        /// <summary>
        /// patch obb path
        /// </summary>
        /// <value></value>
        public static string PatchObbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_patchObbPath))
                {
                    _patchObbPath = GetOBBPath("patch");
                }
                return _patchObbPath;
            }
        }

        /// <summary>
        /// 扩展文件下载回调
        /// </summary>
        private static IObbDownloadListener obbDownloadListener;

        /// <summary>
        /// 扩展文件下载适配器
        /// </summary>
        private static ObbDownloadListenerAdapter obbDownloadListenerAdapter;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            obbDownloaderNative.CallStatic("initialize", NativeAndroid.Context, true);
            obbDownloaderNative.SetStatic("PUBLIC_KEY", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwINnSEZSgRJL+ASiU/iF/a2RSC/hzDG/PB2bQrxFWN0BURjhZH2UV6ysT7sjIv57CHulOq31VHyBjE5kQyivhcK2sOiHByyH8z3aLn2lxnAXAA38Cb0QbL3PV6Gifl29lyxtrGcAlO/fI5GxqLvwT312g/SeOncXroLQwVxFW/EPWjz9PrJ6R4BL5ivs1Us2/NW1eWh89d+aWp0LSbBXq0YUoiqV5Nju1k9toqOYH1Ztxp308RmhNx31iPowiwOlTU0WRUZzVifmz27BbjSW/ufWPYPWhBKafQCDIt5UdkZ82zsbkrf7/T8k+FSJxLMJBdq7muruXvF/QXBh8MpHkwIDAQAB");
            obbDownloaderNative.SetStatic("SALT", new byte[] { 1, 43, 256 - 12, 256 - 1, 54, 98, 256 - 100, 256 - 12, 43, 2, 256 - 8, 256 - 4, 9, 5, 256 - 106, 256 - 108, 256 - 33, 45, 256 - 1, 84 });
            obbDownloaderNative.SetStatic("MAIN_OBB_VERSION", NativeAndroid.VersionCode);
            // TODO Patch Obb
            // obbDownloaderNative.SetStatic("PATCH_OBB_VERSION", NativeAndroid.VersionCode);

            // obbDownloadListenerAdapter = new ObbDownloadListenerAdapter();
            // obbDownloaderNative.CallStatic("", )
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
                return ObbDownloaderStatus.PermissionsError;
            }
            // TODO Patch Obb
            // else if (string.IsNullOrEmpty(MainObbPath) || string.IsNullOrEmpty(PatchObbPath))
            else if (string.IsNullOrEmpty(MainObbPath))
            {
                return ObbDownloaderStatus.ObbNotExist;
            }
            return ObbDownloaderStatus.None;
        }

        /// <summary>
        /// 下载扩展文件
        /// </summary>
        public static void DownloadExpansion()
        {
            obbDownloaderNative.CallStatic("downloadExpansion");
        }

        /// <summary>
        /// Obb Path
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        static string GetOBBPath(string prefix)
        {
            if (string.IsNullOrEmpty(ExpansionFileDirectory))
                return null;

            var filePath = string.Format("{0}/{1}.{2}.{3}.obb", ExpansionFileDirectory, prefix, NativeAndroid.VersionCode, NativeAndroid.PackageName);
            return IOPath.FileExists(filePath) ? filePath : null;
        }

    }
}