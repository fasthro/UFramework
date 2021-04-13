// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-07-01 19:51:50
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework
{
    public enum AppEnvironmentType
    {
        Debug,
        Release,
    }

    public class AppConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Resources;

        #region version

        /// <summary>
        /// 当前版本
        /// </summary>
        public int version;

        /// <summary>
        /// 开发版本
        /// </summary>
        public bool isDevelopmentVersion = true;

        /// <summary>
        /// use Obb
        /// </summary>
        public bool useAPKExpansionFiles = false;

        /// <summary>
        /// App 环境类型
        /// </summary>
        public AppEnvironmentType appEnvironmentType = AppEnvironmentType.Debug;

        /// <summary>
        /// 版本远程基础URL
        /// </summary>
        public string versionBaseURL = "http://192.168.1.253/platform/uframework/";

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel logLevel = LogLevel.Debug;

        #endregion

        #region localization

        /// <summary>
        /// 是否使用系统语言
        /// </summary>
        public bool useSystemLanguage;

        /// <summary>
        /// 默认语言
        /// </summary>
        public SystemLanguage defaultLanguage = SystemLanguage.ChineseSimplified;

        /// <summary>
        /// 支持的语言列表
        /// </summary>
        public List<SystemLanguage> supportedLanguages = new List<SystemLanguage>();

        #endregion

        #region UI

        /// <summary>
        /// 使用 FairyGUI
        /// </summary>
        public bool useFairyGUI = true;

        /// <summary>
        /// 设计分辨率
        /// </summary>
        public int designResolutionX = 1920;

        public int designResolutionY = 1080;

        /// <summary>
        /// 字体列表
        /// </summary>
        public List<string> fonts = new List<string>();

        /// <summary>
        /// UI 所在目录
        /// </summary>
        public string uiDirectory = "Assets/Arts/UI";

        #endregion

        #region project

        /// <summary>
        /// 是否为URP项目
        /// </summary>
        public bool isURP = true;

        /// <summary>
        /// AssetBundle 构建Hash名称
        /// </summary>
        public bool isBuildAssetBunldeNameHash = true;

        /// <summary>
        /// 包 id
        /// </summary>
        public string package = "com.futureruler.uframework";

        #endregion

        #region win

        /// <summary>
        /// 全屏
        /// </summary>
        public FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;

        /// <summary>
        /// 目标分辨率X
        /// </summary>
        public int resolutionX = 1920;

        public int resolutionY = 1080;

        #endregion

        #region pool

        /// <summary>
        /// GoPool 优化间隔时间
        /// </summary>
        public int optimizeIntervalTime = 5;

        /// <summary>
        /// GoPool 完全回收检查间隔时间阈值，大于此值就会自动回收
        /// </summary>
        public int autoUnloadThresholdValue = 60;

        #endregion

        #region network

        /// <summary>
        /// 每帧处理网络包数量
        /// </summary>
        public int networkFrameProcessCount = 5;

        #endregion

        public void Serialize()
        {
            Serializer<AppConfig>.Serialize(this);
        }
    }
}