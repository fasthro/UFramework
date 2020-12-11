/*
 * @Author: fasthro
 * @Date: 2020-07-01 19:51:50
 * @Description: Application Config
 */
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
        public SerializableAssigned assigned { get { return SerializableAssigned.Resources; } }

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
        public int designResolutionX = 2048;
        public int designResolutionY = 1152;

        /// <summary>
        /// UI 所在目录
        /// </summary>
        public string uiDirectory = "Assets/Arts/UI";

        #endregion

        #region project

        /// <summary>
        /// 包 id
        /// </summary>
        public string package = "com.futureruler.uframework";

        #endregion

        public void Serialize()
        {
            Serializer<AppConfig>.Serialize(this);
        }
    }
}