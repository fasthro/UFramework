using UFramework.Native.Service;
using UnityEngine;

namespace UFramework.Sample
{
    public class ObbDownloaderSample : SampleScene
    {

        protected override void OnRenderGUI()
        {
            if (GUILayout.Button("Info", GUILayout.Width(300), GUILayout.Height(100)))
            {
                Debug.Log("ExpansionFileDirectory: " + ObbDownloader.ExpansionFileDirectory);
                Debug.Log("MainObbPath: " + ObbDownloader.MainObbPath);
                Debug.Log("PatchObbPath: " + ObbDownloader.PatchObbPath); ;
                Debug.Log("ExpansionFileStatus: " + ObbDownloader.ExpansionFileStatus().ToString());
            }

            if (GUILayout.Button("Download", GUILayout.Width(300), GUILayout.Height(100)))
            {
                if (ObbDownloader.ExpansionFileStatus() == ObbDownloaderStatus.ObbNotExist)
                {
                    ObbDownloader.DownloadExpansion();
                }
            }
        }
    }
}