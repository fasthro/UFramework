/*
 * @Author: fasthro
 * @Date: 2019-11-30 12:13:19
 * @Description: 下载文件(支持断点续传，支持多文件同时下载)
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Coroutine;
using UnityEngine.Networking;

namespace UFramework.Download
{
    [MonoSingletonPath("UFramework/Download")]
    public class UDownload : MonoSingleton<UDownload>
    {
        // 并行下载数量(默认支持2个同时下载)
        private static int multiDownloadCount = 2;
        public static int MultiDownloadCount
        {
            set
            {
                multiDownloadCount = value;
            }
        }

        // 当前下载任务数量
        private int m_downloadCount;
        // request map
        private Dictionary<string, UnityWebRequest> m_requestDictionary;
        // wait download map<url, DownloadHandlerPro>
        private Dictionary<string, UDownloadHandler> m_waitDictionary;
        private List<string> m_waitKeys;

        protected override void OnSingletonStart()
        {
            m_downloadCount = 0;
            m_requestDictionary = new Dictionary<string, UnityWebRequest>();
            m_waitDictionary = new Dictionary<string, UDownloadHandler>();
            m_waitKeys = new List<string>();
        }

        /// <summary>
        /// start download
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        public UDownloadHandler Download(string url, string savePath)
        {
            if (m_requestDictionary.ContainsKey(url))
                return m_requestDictionary[url].downloadHandler as UDownloadHandler;

            if (m_waitDictionary.ContainsKey(url))
                return m_waitDictionary[url];

            var handler = new UDownloadHandler(url, savePath);
            if (m_downloadCount < multiDownloadCount)
            {
                UFactoryCoroutine.CreateRun(DownloadAsync(handler));
            }
            else
            {
                m_waitDictionary.Add(url, handler);
                m_waitKeys.Add(url);
            }
            return handler;
        }

        /// <summary>
        /// cancel download
        /// </summary>
        public void CancelDownload(string url)
        {
            if (m_requestDictionary.ContainsKey(url))
                CancelDownload(m_requestDictionary[url].downloadHandler as UDownloadHandler);
        }

        /// <summary>
        /// cancel download
        /// </summary>
        /// <param name="handler"></param>
        public void CancelDownload(UDownloadHandler handler)
        {
            if (handler.downloading && !handler.isDone)
            {
                // TODO
                // 正在下载的文件，取消下载
            }
            else
            {
                // TODO
                // handler.eventCanceled();
                
                m_requestDictionary.Remove(handler.url);
                m_waitDictionary.Remove(handler.url);
                m_waitKeys.Remove(handler.url);
            }
        }

        /// <summary>
        /// async download
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        IEnumerator DownloadAsync(UDownloadHandler handler)
        {
            m_downloadCount++;
            var request = UnityWebRequest.Get(handler.url);
            request.chunkedTransfer = true;
            request.disposeDownloadHandlerOnDispose = true;
            request.SetRequestHeader("Range", handler.headerRangeValue);
            request.downloadHandler = handler;
            handler.Download();
            yield return request.SendWebRequest();
            DownloadCompleted(request);
        }

        /// <summary>
        /// download completed
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        private void DownloadCompleted(UnityWebRequest request)
        {
            var handler = request.downloadHandler as UDownloadHandler;
            if (request.isDone)
            {
                // TODO
                // handler.eventCompleted();

                m_requestDictionary.Remove(handler.url);
                handler.Dispose();
                request.Dispose();
            }

            m_downloadCount--;

            if (m_waitKeys.Count > 0)
            {
                var url = m_waitKeys[0];
                UFactoryCoroutine.Create(DownloadAsync(m_waitDictionary[url]));
                m_waitDictionary.Remove(url);
                m_waitKeys.RemoveAt(0);
            }
        }
    }
}