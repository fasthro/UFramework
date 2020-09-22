/*
 * @Author: fasthro
 * @Date: 2020-09-01 16:51:02
 * @Description: native utils
 */
using UnityEngine;

namespace UFramework.Natives
{
    public interface IUtils
    {
        /// <summary>
        /// 重启应用
        /// </summary>
        void Restart();

        /// <summary>
        /// 设置剪切板（Copy）
        /// </summary>
        /// <param name="text"></param>
        void SetClipBoard(string text);

        /// <summary>
        /// 从剪切板获取文本
        /// </summary>
        string GetClipBoard();
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

        public void SetClipBoard(string text)
        {
            native.CallStatic("setClipBoard", text);
        }

        public string GetClipBoard()
        {
            return native.CallStatic<string>("getClipBoard");
        }
    }

    public class IOSUtils : IUtils
    {
        public void Restart()
        {
            throw new System.NotImplementedException();
        }

        public void SetClipBoard(string text)
        {
            throw new System.NotImplementedException();
        }

        public string GetClipBoard()
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

        public void SetClipBoard(string text)
        {
            throw new System.NotImplementedException();
        }

        public string GetClipBoard()
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

        /// <summary>
        /// 设置剪切板（Copy）
        /// </summary>
        /// <param name="text"></param>
        public void SetClipBoard(string text)
        {
            GetChannel().SetClipBoard(text);
        }

        /// <summary>
        /// 从剪切板获取文本
        /// </summary>
        /// <returns></returns>
        public string GetClipBoard()
        {
            return GetChannel().GetClipBoard();
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