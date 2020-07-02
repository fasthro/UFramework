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

        private static string _assetsDirectory;

        /// <summary>
        /// UAssets 目录
        /// </summary>
        /// <returns></returns>
        public static string AssetsDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_assetsDirectory))
                {
                    _assetsDirectory = IOPath.PathUnitySeparator(IOPath.PathCombine(Application.dataPath, "UAssets"));
                }
                return _assetsDirectory;
            }
        }

        #region config

        static string _configDirectory;

        /// <summary>
        /// Config 目录
        /// </summary>
        /// <returns></returns>
        public static string ConfigDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_configDirectory))
                {
                    _configDirectory = IOPath.PathRelativeAsset(IOPath.PathCombine(AssetsDirectory, "Config"));
                }
                return _configDirectory;
            }
        }

        static string _configResourceDirectory;

        /// <summary>
        /// Config Reource 目录
        /// </summary>
        /// <returns></returns>
        public static string ConfigResourceDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_configResourceDirectory))
                {
                    _configResourceDirectory = "Config";
                }
                return _configResourceDirectory;
            }
        }

        #endregion

        #region Table

        private static string _tableDirectory = null;

        /// <summary>
        /// Table Editor 目录
        /// </summary>
        /// <returns></returns>
        public static string TableDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tableDirectory))
                {
                    _tableDirectory = IOPath.PathCombine(AssetsDirectory, "Table");
                }
                return _tableDirectory;
            }
        }

        private static string _tableExcelDirectory = null;

        /// <summary>
        /// Table Excel 目录
        /// </summary>
        /// <returns></returns>
        public static string TableExcelDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tableExcelDirectory))
                {
                    _tableExcelDirectory = IOPath.PathCombine(TableDirectory, "Excel");
                }
                return _tableExcelDirectory;
            }
        }

        private static string _tableDataDirectory = null;

        /// <summary>
        /// Table Data 目录
        /// </summary>
        /// <returns></returns>
        public static string TableDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tableDataDirectory))
                {
                    _tableDataDirectory = IOPath.PathRelativeAsset(IOPath.PathCombine(TableDirectory, "Data"));
                }
                return _tableDataDirectory;
            }
        }

        private static string _tableObjectDirectory = null;

        /// <summary>
        /// Table TableObject 目录
        /// </summary>
        /// <returns></returns>
        public static string TableObjectDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tableDataDirectory))
                {
                    _tableDataDirectory = IOPath.PathCombine(TableDirectory, "TableObject");
                }
                return _tableDataDirectory;
            }
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
                    _languageDirectory = IOPath.PathCombine(AssetsDirectory, "Language");
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
                    _languageDataDirectory = IOPath.PathRelativeAsset(IOPath.PathCombine(LanguageDirectory, "Data"));
                }
                return _languageDataDirectory;
            }
        }

        static string _languageIndexDirectory;

        /// <summary>
        /// 语言Index目录
        /// </summary>
        /// <value></value>
        public static string LanguageIndexDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_languageIndexDirectory))
                {
                    _languageIndexDirectory = IOPath.PathCombine(LanguageDirectory, "Index");
                }
                return _languageIndexDirectory;
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
