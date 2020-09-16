/*
 * @Author: fasthro
 * @Date: 2020-09-15 17:30:01
 * @Description: editor utils
 */

using UnityEditor;

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
    }
}