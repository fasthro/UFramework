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
            // TODO
            return "";
        }

        #endregion

        #region Table

        /// <summary>
        /// Table Editor 目录
        /// </summary>
        /// <returns></returns>
        public static string TableDirectory()
        {
            return IOPath.PathCombine(AssetsDirectory(), "Table");
        }

        /// <summary>
        /// Table Editor Excel 目录
        /// </summary>
        /// <returns></returns>
        public static string TableExcelDirectory()
        {
            return IOPath.PathCombine(TableDirectory(), "Excel");
        }

        /// <summary>
        /// Table Editor Out Data 目录
        /// </summary>
        /// <returns></returns>
        public static string TableOutDataDirectory()
        {
            return IOPath.PathCombine(TableDirectory(), "Data");
        }

        /// <summary>
        /// Table Editor TableObject 目录
        /// </summary>
        /// <returns></returns>
        public static string TableObjectDirectory()
        {
            return IOPath.PathCombine(TableDirectory(), "TableObject");
        }

        /// <summary>
        /// Table Resource 目录
        /// </summary>
        /// <returns></returns>
        public static string TableReourceDirectory()
        {
            return "Table";
        }

        /// <summary>
        /// Table Data 目录
        /// </summary>
        /// <returns></returns>
        public static string TableDataDirectory()
        {
            // TODO
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

        #region Language
        static string _languageDirectory;

        /// <summary>
        /// 语言根目录
        /// </summary>
        /// <value></value>
        public static string LanguageDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_languageDirectory))
                {
                    _languageDirectory = IOPath.PathCombine(AssetsDirectory(), "Language");
                }
                return _languageDirectory;
            }
        }

        static string _languageExcelDirectory;

        /// <summary>
        /// 语言Excel目录
        /// </summary>
        /// <value></value>
        public static string LanguageExcelDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_languageExcelDirectory))
                {
                    _languageExcelDirectory = IOPath.PathCombine(LanguageDirectory, "Excel");
                }
                return _languageExcelDirectory;
            }
        }

        static string _languageDataDirectory;

        /// <summary>
        /// 语言Txt Data目录
        /// </summary>
        /// <value></value>
        public static string LanguageDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_languageDataDirectory))
                {
                    _languageDataDirectory = IOPath.PathCombine(LanguageDirectory, "Data");
                }
                return _languageDataDirectory;
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
