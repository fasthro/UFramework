/*
 * @Author: fasthro
 * @Date: 2020-08-31 18:15:22
 * @Description: 扩展文件下载回调接口适配器
 */

using UnityEngine;

namespace UFramework.Native.Service
{
    public interface IObbDownloadListener
    {
        void OnProgress(int progress);
        void OnSuccess();
        void OnFailed(int errorCode);
    }

    public class ObbDownloadListenerAdapter : AndroidJavaProxy
    {
        readonly IObbDownloadListener listener;

        public ObbDownloadListenerAdapter(IObbDownloadListener listener) : base(NativeAndroid.MAIN_PACKAGE +  ".obbdownloader.UObbDownloadListener")
        {
            this.listener = listener;
        }

        void onProgress(int progress)
        {
            listener.OnProgress(progress);
        }
        void onSuccess(string name)
        {
            listener.OnSuccess();
        }
        void onFailed(int errorCode)
        {
            listener.OnFailed(errorCode);
        }
    }
}
