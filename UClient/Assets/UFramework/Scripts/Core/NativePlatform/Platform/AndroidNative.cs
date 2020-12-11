/*
 * @Author: fasthro
 * @Date: 2020-07-30 17:20:38
 * @Description: android native
 */
using UnityEngine;

namespace UFramework.NativePlatform
{
    public class AndroidNative
    {
        /// <summary>
        /// main package
        /// </summary>
        public readonly static string MAIN_PACKAGE = "com.futureruler.uframework";

        static AndroidJavaObject _Context;

        /// <summary>
        /// context
        /// </summary>
        public static AndroidJavaObject Context
        {
            get
            {
                if (_Context == null)
                {
                    _Context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _Context;
            }
        }

        static string _PackageName;

        /// <summary>
        /// package name
        /// </summary>
        /// <value></value>
        public static string PackageName
        {
            get
            {
                if (string.IsNullOrEmpty(_PackageName))
                {
                    _PackageName = Context.Call<string>("getPackageName");
                }
                return _PackageName;
            }
        }

        static AndroidJavaObject _PackageInfo;

        /// <summary>
        /// package info
        /// </summary>
        /// <value></value>
        public static AndroidJavaObject PackageInfo
        {
            get
            {
                if (_PackageInfo == null)
                {
                    _PackageInfo = Context.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getPackageInfo", PackageName, 0);
                }
                return _PackageInfo;
            }
        }

        static int _VersionCode = -1;

        /// <summary>
        /// version code
        /// </summary>
        /// <value></value>
        public static int VersionCode
        {
            get
            {
                if (_VersionCode == -1)
                {
                    _VersionCode = PackageInfo.Get<int>("versionCode");
                }
                return _VersionCode;
            }
        }

        private static AndroidJavaClass _NativeClass;
        public static AndroidJavaClass NativeClass
        {
            get
            {
                if (_NativeClass == null)
                {
                    _NativeClass = new AndroidJavaClass(MAIN_PACKAGE + ".App");
                }
                return _NativeClass;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            NativeClass.CallStatic("initialize", Context);
            ObbDownloader.Initialize();
        }
    }
}