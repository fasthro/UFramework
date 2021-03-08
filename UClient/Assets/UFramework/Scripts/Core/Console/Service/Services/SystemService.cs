// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:18
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace UFramework.Consoles
{
    public class SystemService : BaseConsoleService
    {
        private readonly Dictionary<string, IList<InfoEntry>> _infoDictionary = new Dictionary<string, IList<InfoEntry>>();

        /// <summary>
        /// 创建报告
        /// </summary>
        /// <param name="includePrivate"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, object>> CreateReport(bool includePrivate = false)
        {
            if (_infoDictionary.Count <= 0)
                CreateSystemInfo();

            var dict = new Dictionary<string, Dictionary<string, object>>();

            foreach (var keyValuePair in _infoDictionary)
            {
                var category = new Dictionary<string, object>();

                foreach (var systemInfo in keyValuePair.Value)
                {
                    if (systemInfo.isPrivate && !includePrivate)
                    {
                        continue;
                    }

                    category.Add(systemInfo.title, systemInfo.Value);
                }

                dict.Add(keyValuePair.Key, category);
            }

            return dict;
        }

        /// <summary>
        /// 创建系统信息
        /// </summary>
        public void CreateSystemInfo()
        {
            _infoDictionary.Clear();

            _infoDictionary.Add("System", new[]
            {
                InfoEntry.Create("Operating System", SystemInfo.operatingSystem),
                InfoEntry.Create("Device Name", SystemInfo.deviceName, true),
                InfoEntry.Create("Device Type", SystemInfo.deviceType),
                InfoEntry.Create("Device Model", SystemInfo.deviceModel),
                InfoEntry.Create("CPU Type", SystemInfo.processorType),
                InfoEntry.Create("CPU Count", SystemInfo.processorCount),
                InfoEntry.Create("System Memory", Utils.FormatBytes(((long) SystemInfo.systemMemorySize) * 1024 * 1024))
            });

#if ENABLE_IL2CPP
            const string IL2CPP = "Yes";
#else
            const string IL2CPP = "No";
#endif

            _infoDictionary.Add("Unity", new[]
            {
                InfoEntry.Create("Version", Application.unityVersion),
                InfoEntry.Create("Debug", Debug.isDebugBuild),
                InfoEntry.Create("Unity Pro", Application.HasProLicense()),
                InfoEntry.Create("Genuine", "{0} ({1})".Fmt(Application.genuine ? "Yes" : "No", 
                    Application.genuineCheckAvailable ? "Trusted" : "Untrusted")),
                InfoEntry.Create("System Language", Application.systemLanguage),
                InfoEntry.Create("Platform", Application.platform),
                InfoEntry.Create("IL2CPP", IL2CPP),
                InfoEntry.Create("Application Version", Application.version),
            });

            _infoDictionary.Add("Display", new[]
            {
                InfoEntry.Create("Resolution", () => Screen.width + "x" + Screen.height),
                InfoEntry.Create("DPI", () => Screen.dpi),
                InfoEntry.Create("Fullscreen", () => Screen.fullScreen),
                InfoEntry.Create("Orientation", () => Screen.orientation)
            });

            _infoDictionary.Add("Runtime", new[]
            {
                InfoEntry.Create("Play Time", () => Time.unscaledTime),
                InfoEntry.Create("Level Play Time", () => Time.timeSinceLevelLoad),
                InfoEntry.Create("Current Level", () =>
                {
                    var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                    return "{0} (Index: {1})".Fmt(activeScene.name, activeScene.buildIndex);
                }),
                InfoEntry.Create("Quality Level",
                    () =>
                        QualitySettings.names[QualitySettings.GetQualityLevel()] + " (" +
                        QualitySettings.GetQualityLevel() + ")")
            });

            _infoDictionary.Add("Features", new[]
            {
                InfoEntry.Create("Location", SystemInfo.supportsLocationService),
                InfoEntry.Create("Accelerometer", SystemInfo.supportsAccelerometer),
                InfoEntry.Create("Gyroscope", SystemInfo.supportsGyroscope),
                InfoEntry.Create("Vibration", SystemInfo.supportsVibration)
            });

#if UNITY_IOS
            _info.Add("iOS", new[] {

         
                InfoEntry.Create("Generation", iPhone.generation),
                InfoEntry.Create("Ad Tracking", iPhone.advertisingTrackingEnabled),
            });

#endif
#pragma warning disable 618
            _infoDictionary.Add("Graphics", new[]
            {
                InfoEntry.Create("Device Name", SystemInfo.graphicsDeviceName),
                InfoEntry.Create("Device Vendor", SystemInfo.graphicsDeviceVendor),
                InfoEntry.Create("Device Version", SystemInfo.graphicsDeviceVersion),
                InfoEntry.Create("Max Tex Size", SystemInfo.maxTextureSize),

                InfoEntry.Create("NPOT Support", SystemInfo.npotSupport),
                InfoEntry.Create("Render Textures",
                    "{0} ({1})".Fmt(SystemInfo.supportsRenderTextures ? "Yes" : "No",
                        UnityEngine.SystemInfo.supportedRenderTargetCount)),
                InfoEntry.Create("3D Textures", SystemInfo.supports3DTextures),
                InfoEntry.Create("Compute Shaders", SystemInfo.supportsComputeShaders),

                InfoEntry.Create("Image Effects", SystemInfo.supportsImageEffects),
                InfoEntry.Create("Cubemaps", SystemInfo.supportsRenderToCubemap),
                InfoEntry.Create("Shadows", SystemInfo.supportsShadows),
                InfoEntry.Create("Stencil", SystemInfo.supportsStencil),
                InfoEntry.Create("Sparse Textures", SystemInfo.supportsSparseTextures)
            });
#pragma warning restore 618

            // Custom Infos
            _infoDictionary.Add("Project", new[]
            {
                InfoEntry.Create("Version", UApplication.Version),
            });
        }
    }
}