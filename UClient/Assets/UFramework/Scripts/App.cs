/*
 * @Author: fasthro
 * @Date: 2020-06-14 20:11:16
 * @Description: UFramework App
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UFramework.Config;
using UnityEngine;

namespace UFramework
{
    public class App
    {
        #region const
        /// <summary>
        /// Asset Bundle 文件扩展名
        /// </summary>
        readonly public static string AssetBundleExtension = ".unity3d";

        /// <summary>
        /// 版本文件名称
        /// </summary>
        readonly public static string VersionFileName = "version";

        #endregion

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
#if UNITY_EDITOR
                    _assetsDirectory = IOPath.PathUnitySeparator(IOPath.PathCombine(Application.dataPath, "UAssets"));
#else
                     _assetsDirectory = IOPath.PathUnitySeparator(IOPath.PathCombine("Assets", "UAssets"));
#endif
                }
                return _assetsDirectory;
            }
        }

        private static string _tempDirectory;

        /// <summary>
        /// 临时目录
        /// </summary>
        /// <value></value>
        public static string UTempDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tempDirectory))
                {
                    _tempDirectory = IOPath.PathCombine(IOPath.PathParent(Application.dataPath), "UTemp");
                }
                return _tempDirectory;
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

        static string _configDataDirectory;

        /// <summary>
        /// Config Data 目录
        /// </summary>
        /// <returns></returns>
        public static string ConfigDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_configDataDirectory))
                {
                    _configDataDirectory = IOPath.PathCombine(ConfigDirectory, FileAddress.Data.ToString());
                }
                return _configDataDirectory;
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

        public static string _bundleStreamingDirectory = null;

        /// <summary>
        /// Bundle streamingAssetsPath 目录
        /// </summary>
        /// <value></value>
        public static string BundleStreamingDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_bundleStreamingDirectory))
                {
                    _bundleStreamingDirectory = IOPath.PathCombine(Application.streamingAssetsPath, BundlePlatformName);
                }
                return _bundleStreamingDirectory;
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
                if (string.IsNullOrEmpty(_bundleDirectory))
                {
                    if (!Application.isPlaying)
                    {
                        _bundleDirectory = IOPath.PathCombine(UTempDirectory, BundlePlatformName);
                    }
                    else
                    {
                        if (UConfig.Read<AppConfig>().isDevelopmentVersion)
                            _bundleDirectory = IOPath.PathCombine(UTempDirectory, BundlePlatformName);
                        else _bundleDirectory = IOPath.PathCombine(DataDirectory, BundlePlatformName);
                    }
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
        #endregion

        #region lua

        private static string _luaTempDataDirectory = null;

        /// <summary>
        /// lua temp script 所在数据目录(非开发模式读取此目录)
        /// </summary>
        /// <value></value>
        public static string LuaTempDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_luaTempDataDirectory))
                {
                    _luaTempDataDirectory = IOPath.PathCombine(UTempDirectory, "Scripts");
                }
                return _luaTempDataDirectory;
            }
        }


        private static string _luaDataDirectory = null;

        /// <summary>
        /// lua script 所在数据目录(非开发模式读取此目录)
        /// </summary>
        /// <value></value>
        public static string LuaDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_luaDataDirectory))
                {
                    _luaDataDirectory = IOPath.PathCombine(DataDirectory, "Scripts");
                }
                return _luaDataDirectory;
            }
        }
        #endregion

        #region data

        /// <summary>
        /// 数据根目录
        /// </summary>
        /// <returns></returns>
        public static string DataDirectory
        {
            get { return Application.persistentDataPath; }
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

        #region Native

        private static string _androidNativeDirectory;

        /// <summary>
        /// Android Native Directory
        /// </summary>
        /// <value></value>
        public static string AndroidNativeDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_androidNativeDirectory))
                {
                    _androidNativeDirectory = IOPath.PathUnitySeparator(IOPath.PathCombine(IOPath.PathParent(Application.dataPath), "Native", "Android"));
                }
                return _androidNativeDirectory;
            }
        }

        #endregion

        #region version

        private static string _versionPath;

        /// <summary>
        /// 版本文件本地数据路径
        /// </summary>
        /// <value></value>
        public static string versionPath
        {
            get
            {
                if (string.IsNullOrEmpty(_versionPath))
                {
                    if (UConfig.Read<AppConfig>().isDevelopmentVersion)
                        _versionPath = IOPath.PathCombine(UTempDirectory, VersionFileName);
                    else _versionPath = IOPath.PathCombine(DataDirectory, VersionFileName);
                }
                return _versionPath;
            }
        }

        private static string _versionStreamingPath;

        /// <summary>
        /// 版本文件Streaming路径
        /// </summary>
        /// <value></value>
        public static string versionStreamingPath
        {
            get
            {
                if (string.IsNullOrEmpty(_versionStreamingPath))
                {
                    _versionStreamingPath = IOPath.PathCombine(Application.streamingAssetsPath, VersionFileName);
                }
                return _versionStreamingPath;
            }
        }
        #endregion

        /// <summary>
        /// 用户代码自动生成目录
        /// </summary>
        public static string UserScriptAutomaticDirectory = "Assets/Scripts/Automatic";

        /// <summary>
        /// 用户代码自动生成目录 - 本地化
        /// </summary>
        public static string UserScriptAutomaticLocalizationDirectory = "Assets/Scripts/Automatic/Localization";

        /// <summary>
        /// 用户代码自动生成目录 - Table
        /// </summary>
        public static string UserScriptAutomaticTableDirectory = "Assets/Scripts/Automatic/Table";

        /// <summary>
        /// 用户代码自动生成目录 - Lua Script
        /// </summary>
        public static string UserScriptAutomaticLuaDirectory = "Assets/Scripts/Automatic/Lua";

        /// <summary>
        /// 用户代码自动生成目录 - ToLua Wrap Source
        /// </summary>
        public static string UserScriptAutomaticToLuaDirectory = "Assets/Scripts/Automatic/WrapSource";

        #endregion

        #region manager
        static Dictionary<string, BaseManager> ManagerDictionary = new Dictionary<string, BaseManager>();
        static List<BaseManager> Managers = new List<BaseManager>();

        static void InitializeManager()
        {
            AddManager<LuaManager>();
            AddManager<UIManager>();
            AddManager<TCPManager>();
        }

        static T AddManager<T>() where T : BaseManager, new()
        {
            var type = typeof(T);
            var obj = new T();
            obj.Initialize();
            ManagerDictionary.Add(type.Name, obj);
            Managers.Add(obj);
            return obj;
        }

        public static T GetManager<T>() where T : class
        {
            var type = typeof(T);
            if (!ManagerDictionary.ContainsKey(type.Name))
            {
                return null;
            }
            return ManagerDictionary[type.Name] as T;
        }

        public static BaseManager GetManager(string managerName)
        {
            if (!ManagerDictionary.ContainsKey(managerName))
            {
                return null;
            }
            return ManagerDictionary[managerName];
        }

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            InitializeManager();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void Update(float deltaTime)
        {
            for (int i = 0; i < Managers.Count; i++)
            {
                Managers[i].Update(deltaTime);
            }
        }

        /// <summary>
        /// LateUpdate
        /// </summary>
        public static void LateUpdate()
        {
            for (int i = 0; i < Managers.Count; i++)
            {
                Managers[i].LateUpdate();
            }
        }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        public static void FixedUpdate()
        {
            for (int i = 0; i < Managers.Count; i++)
            {
                Managers[i].FixedUpdate();
            }
        }

        /// <summary>
        /// Destory
        /// </summary>
        public static void Destory()
        {
            for (int i = 0; i < Managers.Count; i++)
            {
                Managers[i].Dispose();
            }
        }
    }
}
