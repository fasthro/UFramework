/*
 * @Author: fasthro
 * @Date: 2020-09-23 11:01:36
 * @Description: download
 */
using System.Collections.Generic;

namespace UFramework.Core
{
    public enum DownloadState
    {
        Init,
        Donwloading,
        Donwloaded,
    }

    [MonoSingletonPath("UFramework/Downloader")]
    public class Downloader : MonoSingleton<Downloader>
    {
        // 并行最大下载数量
        const int MAX_DOWNLOAD_COUNT = 2;

        readonly static List<DownloadHandler> Downloads = new List<DownloadHandler>();

        /// <summary>
        /// 类似双向缓冲队列，不会阻塞downloads运行
        /// </summary>
        /// <typeparam name="TImerEntity"></typeparam>
        /// <returns></returns>
        readonly static List<DownloadHandler> DownloadsBuffer = new List<DownloadHandler>();

        readonly static List<int> Removes = new List<int>();

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
            DownloadsBuffer.Add(download);
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
            for (int i = 0; i < Downloads.Count; i++)
            {
                Downloads[i].Cancel();
            }
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void OnSingletonUpdate(float deltaTime)
        {
            Removes.Clear();
            if (DownloadsBuffer.Count > 0 && Downloads.Count < MAX_DOWNLOAD_COUNT)
            {
                for (int i = 0; i < MAX_DOWNLOAD_COUNT - Downloads.Count; i++)
                {
                    Downloads.Add(DownloadsBuffer[i].Download());
                    Removes.Add(i);
                }
                if (Removes.Count > 0)
                {
                    for (int i = 0; i < Removes.Count; i++)
                    {
                        DownloadsBuffer.RemoveAt(0);
                    }
                }
            }

            Removes.Clear();
            for (int i = 0; i < Downloads.Count; i++)
            {
                var download = Downloads[i];
                download.OnUpdate();
                if (download.downloadState == DownloadState.Donwloaded)
                {
                    Removes.Add(i);
                }
            }
            if (Removes.Count > 0)
            {
                for (int i = Removes.Count - 1; i >= 0; i--)
                {
                    var index = Removes[i];
                    // downloads[index].Recycle();
                    Downloads.RemoveAt(index);
                }
            }
        }

        protected override void OnSingletonDestory()
        {
            for (int i = 0; i < Downloads.Count; i++)
            {
                Downloads[i].Release();
            }
            Downloads.Clear();
        }
    }
}