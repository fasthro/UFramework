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
        readonly IObbDownloadListener _listener;

        public ObbDownloadListenerAdapter(IObbDownloadListener listener) : base(NativeAndroid.MAIN_PACKAGE + ".obbdownloader.core.download.ObbDownloadListener")
        {
            this._listener = listener;
        }

        void onProgress(int progress)
        {
            _listener.OnProgress(progress);
        }

        void onSuccess()
        {
            _listener.OnSuccess();
        }

        void onFailed()
        {
            _listener.OnFailed();
        }

        void onPause(bool pause)
        {
            _listener.OnPause(pause);
        }

        void onAbort()
        {
            _listener.OnAbort();
        }

        void onError(int errorCode)
        {
            _listener.OnError(errorCode);
        }
    }
}
