/*
 * @Author: fasthro
 * @Date: 2020-09-01 16:51:02
 * @Description: native utils
 */
using UnityEngine;

namespace UFramework.Native.Service
{
    public interface IUtils
    {
        /// <summary>
        /// 重启应用
        /// </summary>
        void Restart();
    }

    public class AndroidUtils : IUtils
    {
        static AndroidJavaClass _native;
        public static AndroidJavaClass native
        {
            get
            {
                if (_native == null)
                {
                    _native = new AndroidJavaClass(NativeAndroid.MAIN_PACKAGE + ".core.Utils");
                }
                return _native;
            }
        }

        public void Restart()
        {
            native.CallStatic("restart");
        }
    }

    public class IOSUtils : IUtils
    {
        public void Restart()
        {
            throw new System.NotImplementedException();
        }
    }

    public class UnknownUtils : IUtils
    {
        public void Restart()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Utils : IUtils
    {
        private UnknownUtils unknown = new UnknownUtils();
        private AndroidUtils android = new AndroidUtils();
        private IOSUtils ios = new IOSUtils();

        /// <summary>
        /// 重启应用
        /// </summary>
        public void Restart()
        {
            GetChannel().Restart();
        }

        IUtils GetChannel()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return android;
#elif !UNITY_EDITOR && UNITY_IOS
            return ios;
#else
            return unknown;
#endif
        }
    }
}