// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-14 20:11:16
// * @Description:
// --------------------------------------------------------------------------------

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
        /// PB DataPath
        /// </summary>
        /// <value></value>
        public static string PBDirectory =>
            Serializer<AppConfig>.Instance.isDevelopmentVersion
                ? IOPath.PathCombine(Application.dataPath, "Scripts/Automatic/PB")
                : IOPath.PathCombine(Application.persistentDataPath, "Files/PB");

        /// <summary>
        /// Table DataPath
        /// </summary>
        /// <value></value>
        public static string TableDirectory =>
            Serializer<AppConfig>.Instance.isDevelopmentVersion
                ? IOPath.PathCombine(Application.dataPath, "UAssets/Table/Data")
                : IOPath.PathCombine(Application.persistentDataPath, "Files/Table");

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
                return pvc == -1 ? $"A{Application.version}_V{vc}_PE" : $"A{Application.version}_V{vc}_P{pvc}";
            }
        }
    }
}