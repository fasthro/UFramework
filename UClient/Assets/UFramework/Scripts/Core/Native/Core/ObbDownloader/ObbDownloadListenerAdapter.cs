/*
 * @Author: fasthro
 * @Date: 2020-08-31 18:15:22
 * @Description: 扩展文件下载回调接口适配器
 */

using UnityEngine;

namespace UFramework.Natives
{
    public interface IObbDownloadListener
    {
        void OnProgress(int progress);
        void OnSuccess();
        void OnFailed();
        void OnPause(bool pause);
        void OnAbort();
        void OnError(int errorCode);
    }

    public class ObbDownloadListenerAdapter : AndroidJavaProxy
    {
        readonly IObbDownloadListener listener;

        public ObbDownloadListenerAdapter(IObbDownloadListener listener) : base(NativeAndroid.MAIN_PACKAGE + ".obbdownloader.core.download.ObbDownloadListener")
        {
            this.listener = listener;
        }

        void onProgress(int progress)
        {
            listener.OnProgress(progress);
        }

        void onSuccess()
        {
            listener.OnSuccess();
        }

        void onFailed()
        {
            listener.OnFailed();
        }

        void onPause(bool pause)
        {
            listener.OnPause(pause);
        }

        void onAbort()
        {
            listener.OnAbort();
        }

        void onError(int errorCode)
        {
            listener.OnError(errorCode);
        }
    }
}
