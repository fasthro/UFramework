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
        private static AndroidJavaObject _context;

        /// <summary>
        /// context
        /// </summary>
        public static AndroidJavaObject context
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

        private static AndroidJavaClass _mainNative;

        /// <summary>
        /// mainNative
        /// </summary>
        public static AndroidJavaClass mainNative
        {
            get
            {
                if (_mainNative == null)
                {
                    _mainNative = new AndroidJavaClass("com.futureruler.uframework.MainNative");
                }
                return _mainNative;
            }
        }

        /// <summary>
        /// Google Play Service
        /// </summary>
        /// <returns></returns>
        public static GooglePlayService googlePlayService { get; private set; }


        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            mainNative.CallStatic("initialize", context);

            googlePlayService = new GooglePlayService();
            googlePlayService.Initialize();
        }
    }
}