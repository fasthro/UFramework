using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UFramework.Sample
{
    public class MainSample : MonoBehaviour
    {
        static string[] sceneNames = new string[] { "NativeSample", "ObbDownloaderSample" };

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
        }
    }
}
