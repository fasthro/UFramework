/*
 * @Author: fasthro
 * @Date: 2020-09-23 14:15:06
 * @Description: download
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Sample
{
    public class DownloadSample : SampleScene
    {
        private DownloadHandler download;
        protected override void OnRenderGUI()
        {
            if (GUILayout.Button("Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                var local = IOPath.PathCombine(Application.streamingAssetsPath, "download-sample-file.zip");
                IOPath.FileDelete(local);
                if (download != null)
                {
                    download.Cancel();
                }
                download = Downloader.AddDownload("http://res.fairygui.com/FairyGUI-Editor_2020.2.1.zip",
                 IOPath.PathCombine(Application.streamingAssetsPath, "download-sample-file.zip"),
                 this.OnCompleted, this.OnProgress, this.OnCancelled, this.OnFailed);
            }

            if (GUILayout.Button("Cancel Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                if (download != null)
                {
                    download.Cancel();
                }
            }
        }

        private void OnCompleted()
        {
            Debug.Log("download completed");
        }

        private void OnProgress(float progress)
        {
            Debug.Log("download: " + progress);
        }

        private void OnCancelled()
        {
            Debug.Log("download canceled");
        }

        private void OnFailed(string error)
        {
            Debug.LogError("download error: " + error);
        }
    }
}
