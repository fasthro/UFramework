/*
 * @Author: fasthro
 * @Date: 2020-06-14 20:11:16
 * @Description: UFramework App
 */
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UFramework.Config;
using UFramework.VersionControl;
using UnityEngine;

namespace UFramework
{
    public class App
    {
        #region path

        private static string _assetsDirectory;
        private static string _buildDirectory;
        private static string _tempDirectory;

        /// <summary>
        /// UAssets
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

        /// <summary>
        /// Build
        /// </summary>
        /// <value></value>
        public static string BuildDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_buildDirectory))
                    _buildDirectory = IOPath.PathCombine(Environment.CurrentDirectory, "Build");
                return _buildDirectory;
            }
        }


        /// <summary>
        /// Temp
        /// </summary>
        /// <value></value>
        public static string TempDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tempDirectory))
                    _tempDirectory = IOPath.PathCombine(Environment.CurrentDirectory, "UTemp");
                return _tempDirectory;
            }
        }

        /// <summary>
        /// PB
        /// </summary>
        /// <value></value>
        public static string PBDirectory
        {
            get
            {
                var appConfig = UConfig.Read<AppConfig>();
                if (appConfig.isDevelopmentVersion)
                    return IOPath.PathCombine(Application.dataPath, "Scripts/Automatic/Lua/PB");
                else return IOPath.PathCombine(Application.persistentDataPath, "Lua/9eb42865081803c085ef7ce653312ab8/PB");
            }
        }

        #endregion

        #region manager
        static Dictionary<string, BaseManager> ManagerDictionary = new Dictionary<string, BaseManager>();
        static List<BaseManager> Managers = new List<BaseManager>();

        static void InitializeManager()
        {
            AddManager<LuaManager>();
            AddManager<NetManager>();
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

        #region other

        /// <summary>
        /// 显示版本
        /// </summary>
        /// <value></value>
        public static string Version
        {
            get
            {
                var pvc = Updater.Instance.patchVersionCode;
                var vc = Updater.Instance.versionCode;
                if (pvc == -1) return string.Format("A{0}_V{1}_PE", Application.version, vc);
                else return string.Format("A{0}_V{1}_P{2}", Application.version, vc, pvc);
            }
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
