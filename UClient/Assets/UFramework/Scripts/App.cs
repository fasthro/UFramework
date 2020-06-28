/*
 * @Author: fasthro
 * @Date: 2020-06-14 20:11:16
 * @Description: UFramework App
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        #region config

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

        #region bundle

        private static string _bundlePlatformName = null;

        /// <summary>
        /// Bundle Platform Name
        /// </summary>
        /// <value></value>
        public static string BundlePlatformName
        {
            get
            {
                if (_bundlePlatformName == null)
                {
#if UNITY_ANDROID
                    _bundlePlatformName = "Android";
#elif UNITY_IOS
                    _bundlePlatformName = "iOS";
#elif UNITY_STANDALONE_WIN
                    _bundlePlatformName = "Windows";
#elif UNITY_STANDALONE_OSX
                    _bundlePlatformName = "OSX";
#endif
                }
                return _bundlePlatformName;
            }
        }

        public static string _bundleDirectory = null;

        /// <summary>
        /// Bundle 目录
        /// </summary>
        /// <returns></returns>
        public static string BundleDirectory
        {
            get
            {
                if (_bundleDirectory == null)
                {
                    if (!Application.isPlaying)
                    {
                        _bundleDirectory = IOPath.PathCombine(Application.streamingAssetsPath, BundlePlatformName);
                    }
                    _bundleDirectory = IOPath.PathCombine(DataDirectory, BundlePlatformName);
                }
                return _bundleDirectory;
            }
        }
        #endregion


        #region data

        private static string _dataDirectory = null;

        /// <summary>
        /// 数据根目录
        /// </summary>
        /// <returns></returns>
        public static string DataDirectory
        {
            get
            {
#if UNITY_EDITOR
                _dataDirectory = Application.streamingAssetsPath;
#else
                _dataDirectory = Application.persistentDataPath;
#endif
                return _dataDirectory;
            }
        }

        /// <summary>
        /// Raw 目录
        /// </summary>
        public static string DataRawDirectory()
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath + "/";
#else
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Application.dataPath + "/Raw/";
            }
            else return Application.streamingAssetsPath + "/";
#endif
        }

        #endregion

        /// <summary>
        /// 用户代码目录
        /// </summary>
        /// <returns></returns>
        public static string UserScriptDirectory()
        {
            return "Stripts";
        }

        /// <summary>
        /// Lua Wrap 目录
        /// </summary>
        /// <returns></returns>
        public static string ToLuaWrapDirectory()
        {
            return "Stripts/ToLuaWrap";
        }

        #endregion
    }
}
