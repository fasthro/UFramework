/*
 * @Author: fasthro
 * @Date: 2020-09-15 17:30:01
 * @Description: editor utils
 */
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor
{
    public static class Utils
    {
        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="title"></param>
        /// <param name="des"></param>
        /// <param name="value"></param>
        /// <param name="max"></param>
        public static void UpdateProgress(string title, string des, int value, int max)
        {
            value = value == 0 ? value + 1 : value;
            max = max <= 0 ? value : max;
            if (value > max) value = max;
            EditorUtility.DisplayProgressBar(title, des, (float)value / (float)max);
        }

        /// <summary>
        /// 隐藏进度条
        /// </summary>
        public static void HideProgress()
        {
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// GetAsset
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }

            return asset;
        }

        public static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup group, string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            var sbs = symbols.Split(';');
            for (int i = 0; i < sbs.Length; i++)
            {
                if (sbs[i].Equals(symbol))
                {
                    return;
                }
            }
            if (sbs.Length > 0)
                symbols += ";";
            symbols += symbol;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, symbols);
        }

        public static void RemoveScriptingDefineSymbolsForGroup(BuildTargetGroup group, string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            var sbs = symbols.Split(';');
            string ns = "";
            for (int i = 0; i < sbs.Length; i++)
            {
                if (!sbs[i].Equals(symbol))
                {
                    if (string.IsNullOrEmpty(ns))
                        ns += sbs[i];
                    else
                    {
                        ns += ";";
                        ns += sbs[i];
                    }
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, ns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="args"></param>
        /// <param name="workdir"></param>
        public static void ExecuteProcess(string proc, string args, string workdir)
        {
            Debug.Log(string.Format("execute process > {0}:{1}", proc, args));

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = proc;
            info.Arguments = args;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.WorkingDirectory = workdir;

            System.Diagnostics.Process pro = System.Diagnostics.Process.Start(info);
            pro.WaitForExit();

            string msg = pro.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(msg))
            {
                Debug.Log(msg);
            }
        }
    }
}