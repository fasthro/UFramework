using UFramework.Native;
using UFramework.Native.Service;
using UnityEngine;

namespace UFramework.Sample
{
    public class ObbMainSample : MonoBehaviour, IObbDownloadListener
    {
        static string[] sceneNames = new string[] { "NativeSample" };

        void OnGUI()
        {
            for (int i = 0; i < sceneNames.Length; i++)
            {
                var sceneName = sceneNames[i];
                if (GUILayout.Button(sceneName, GUILayout.Width(300), GUILayout.Height(100)))
                {
                    SampleScene.LoadScene(sceneName);
                }
            }

            OnRenderGUI();
        }
        void OnRenderGUI()
        {
            if (GUILayout.Button("Info", GUILayout.Width(300), GUILayout.Height(100)))
            {
                Debug.Log("ExpansionFileDirectory: " + ObbDownloader.ExpansionFileDirectory);
                Debug.Log("MainObbPath: " + ObbDownloader.MainObbPath);
                Debug.Log("PatchObbPath: " + ObbDownloader.PatchObbPath); ;
                Debug.Log("ExpansionFileStatus: " + ObbDownloader.ExpansionFileStatus().ToString());
            }

            if (GUILayout.Button("Delete Obb", GUILayout.Width(300), GUILayout.Height(100)))
            {
                IOPath.FileDelete(ObbDownloader.MainObbPath);
            }

            if (GUILayout.Button("Set Download Listener", GUILayout.Width(300), GUILayout.Height(100)))
            {
                ObbDownloader.SetDownloadListener(this);
            }

            if (GUILayout.Button("Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                if (ObbDownloader.ExpansionFileStatus() == ObbDownloaderStatus.DownloadRequired)
                {
                    ObbDownloader.DownloadExpansion();
                }
            }

            if (GUILayout.Button("Continue Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                ObbDownloader.ContinueDownload();
            }

            if (GUILayout.Button("Pause Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                ObbDownloader.PauseDownload();
            }

            if (GUILayout.Button("Abort Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                ObbDownloader.AbortDownload();
            }

            if (GUILayout.Button("Restart App", GUILayout.Width(300), GUILayout.Height(100)))
            {
                UNative.utils.Restart();
            }
        }

        public void OnProgress(int progress)
        {
            Debug.Log("Download OnProgress: " + progress.ToString());
        }

        public void OnSuccess()
        {
            Debug.Log("Download OnSuccess.");
        }

        public void OnFailed()
        {
            Debug.Log("Download OnFailed.");
        }

        public void OnPause(bool pause)
        {
            Debug.Log("Download OnPause. [" + pause + "]");
        }

        public void OnAbort()
        {
            Debug.Log("Download OnAbort.");
        }

        public void OnError(int errorCode)
        {
            Debug.Log("Download OnError. [ErrorCode: " + (ObbDownloaderStatus)errorCode + "]");
        }
    }
}