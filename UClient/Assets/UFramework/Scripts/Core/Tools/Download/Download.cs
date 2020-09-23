/*
 * @Author: fasthro
 * @Date: 2020-09-23 11:01:36
 * @Description: download
 */
using System.Collections.Generic;
using UFramework.Messenger;

namespace UFramework.Tools
{
    /// <summary>
    /// 下载状态
    /// </summary>
    public enum DownloadState
    {
        Init,
        Donwloading,
        Donwloaded,
    }

    /// <summary>
    /// download
    /// </summary>
    [MonoSingletonPath("UFramework/Download")]
    public class Download : MonoSingleton<Download>
    {
        // 并行最大下载数量
        readonly static int MAX_DOWNLOAD_COUNT = 2;

        readonly static List<DownloadHandler> downloads = new List<DownloadHandler>();

        /// <summary>
        /// 类似双向缓冲队列，不会阻塞downloads运行
        /// </summary>
        /// <typeparam name="TImerEntity"></typeparam>
        /// <returns></returns>
        readonly static List<DownloadHandler> downloadsBuffer = new List<DownloadHandler>();

        readonly static List<int> removes = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">下载链接</param>
        /// <param name="local">本地保存路径</param>
        /// <param name="onCompleted">完成事件</param>
        /// <param name="onProgress">下载进度事件</param>
        /// <param name="onCancelled">取消下载事件</param>
        /// <param name="onFailed">下载失败事件</param>
        /// <param name="length">文件长度(大于0时下载文件需要进行长度验证)</param>
        /// <param name="hash">文件hash(有值下载文件时需要进行hash验证)</param>
        /// <param name="hash">是否断点续传</param>
        /// <returns></returns>
        public static DownloadHandler AddDownload(string url, string local,
         UCallback onCompleted = null, UCallback<float> onProgress = null, UCallback onCancelled = null, UCallback<string> onFailed = null,
          long length = 0, string hash = null, bool isContinue = true)
        {
            var download = DownloadHandler.Allocate(url, local, onCompleted, onProgress, onCancelled, onFailed, length, hash, isContinue);
            downloadsBuffer.Add(download);
            return download;
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="download"></param>
        public static void CancelDownload(DownloadHandler download)
        {
            download.Cancel();
        }

        /// <summary>
        /// 取消全部下载
        /// </summary>
        /// <param name="download"></param>
        public static void CancelAllDownload()
        {
            for (int i = 0; i < downloads.Count; i++)
            {
                downloads[i].Cancel();
            }
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void OnSingletonUpdate(float deltaTime)
        {
            removes.Clear();
            if (downloadsBuffer.Count > 0 && downloads.Count < MAX_DOWNLOAD_COUNT)
            {
                for (int i = 0; i < MAX_DOWNLOAD_COUNT - downloads.Count; i++)
                {
                    downloads.Add(downloadsBuffer[i].Download());
                    removes.Add(i);
                }
                if (removes.Count > 0)
                {
                    for (int i = 0; i < removes.Count; i++)
                    {
                        downloadsBuffer.RemoveAt(0);
                    }
                }
            }

            removes.Clear();
            for (int i = 0; i < downloads.Count; i++)
            {
                var download = downloads[i];
                download.OnUpdate();
                if (download.downloadState == DownloadState.Donwloaded)
                {
                    removes.Add(i);
                }
            }
            if (removes.Count > 0)
            {
                for (int i = removes.Count - 1; i >= 0; i--)
                {
                    var index = removes[i];
                    // downloads[index].Recycle();
                    downloads.RemoveAt(index);
                }
            }
        }

        protected override void OnSingletonDestory()
        {
            for (int i = 0; i < downloads.Count; i++)
            {
                downloads[i].Release();
            }
            downloads.Clear();
        }
    }
}