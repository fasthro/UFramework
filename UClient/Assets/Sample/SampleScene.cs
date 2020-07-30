using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UFramework.Sample
{
    public abstract class SampleScene : MonoBehaviour
    {
        /// <summary>
        /// 上一个场景名称
        /// </summary>
        static string preSceneName;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName)
        {
            preSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 返回上一个场景
        /// </summary>
        protected void BackScene()
        {
            if (!string.IsNullOrEmpty(preSceneName))
                LoadScene(preSceneName);
        }

        void OnGUI()
        {
            if (GUILayout.Button("Back Scene", GUILayout.Width(300), GUILayout.Height(100)))
            {
                BackScene();
            }
            OnRenderGUI();
        }

        protected virtual void OnRenderGUI()
        {

        }
    }
}