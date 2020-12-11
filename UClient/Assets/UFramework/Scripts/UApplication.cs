/*
 * @Author: fasthro
 * @Date: 2020-06-14 20:11:16
 * @Description: UFramework App
 */
using System;
using UFramework.Core;
using UnityEngine;

namespace UFramework
{
    public class UApplication
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
                if (Core.Serializer<AppConfig>.Instance.isDevelopmentVersion)
                    return IOPath.PathCombine(Application.dataPath, "Scripts/Automatic/Lua/PB");
                else return IOPath.PathCombine(Application.persistentDataPath, "Lua/9eb42865081803c085ef7ce653312ab8/PB");
            }
        }

        #endregion

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
    }
}
