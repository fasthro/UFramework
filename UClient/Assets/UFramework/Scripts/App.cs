/*
 * @Author: fasthro
 * @Date: 2020-06-14 20:11:16
 * @Description: UFramework App
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{
    public class App
    {
        #region path

        /// <summary>
        /// UAssets 目录
        /// </summary>
        /// <returns></returns>
        public static string AssetsDirectory()
        {
            return IOPath.PathCombine(Application.dataPath, "UAssets");
        }

        /// <summary>
        /// Config Editor 目录
        /// </summary>
        /// <returns></returns>
        public static string ConfigDirectory()
        {
            return IOPath.PathCombine(AssetsDirectory(), "Config");
        }

        /// <summary>
        /// Config Reource 目录
        /// </summary>
        /// <returns></returns>
        public static string ConfigResourceDirectory()
        {
            return "Config";
        }

        /// <summary>
        /// Config Data 目录
        /// </summary>
        /// <returns></returns>
        public static string ConfigDataDirectory()
        {
            return "";
        }

        #endregion
    }
}
