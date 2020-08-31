/*
 * @Author: fasthro
 * @Date: 2020-07-30 17:20:38
 * @Description: android native
 */
using UFramework.Native.Service;
using UnityEngine;

namespace UFramework.Native
{
    public class NativeAndroid
    {
        /// <summary>
        /// main package
        /// </summary>
        public readonly static string MAIN_PACKAGE = "com.futureruler.uframework";

        private static AndroidJavaObject _context;

        /// <summary>
        /// context
        /// </summary>
        public static AndroidJavaObject Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _context;
            }
        }

        private static string _packageName;

        /// <summary>
        /// package name
        /// </summary>
        /// <value></value>
        public static string PackageName
        {
            get
            {
                if (string.IsNullOrEmpty(_packageName))
                {
                    _packageName = Context.Call<string>("getPackageName");
                }
                return _packageName;
            }
        }

        private static AndroidJavaObject _packageInfo;

        /// <summary>
        /// package info
        /// </summary>
        /// <value></value>
        public static AndroidJavaObject PackageInfo
        {
            get
            {
                if (_packageInfo == null)
                {
                    _packageInfo = Context.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getPackageInfo", PackageName, 0);
                }
                return _packageInfo;
            }
        }

        private static int _versionCode = -1;

        /// <summary>
        /// version code
        /// </summary>
        /// <value></value>
        public static int VersionCode
        {
            get
            {
                if (_versionCode == -1)
                {
                    _versionCode = PackageInfo.Get<int>("versionCode");
                }
                return _versionCode;
            }
        }

        private static AndroidJavaClass _appNative;

        /// <summary>
        /// app
        /// </summary>
        public static AndroidJavaClass AppNative
        {
            get
            {
                if (_appNative == null)
                {
                    _appNative = new AndroidJavaClass(MAIN_PACKAGE + ".App");
                }
                return _appNative;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            AppNative.CallStatic("initialize", Context);
            ObbDownloader.Initialize();
        }
    }
}