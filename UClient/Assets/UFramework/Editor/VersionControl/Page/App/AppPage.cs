/*
 * @Author: fasthro
 * @Date: 2020-08-26 16:02:32
 * @Description: app page
 */
using Sirenix.OdinInspector;

namespace UFramework.Editor.VersionControl.App
{
    public class AppPage : IPage, IPageBar
    {
        public string menuName { get { return "Application"; } }
        static AppConfig Config { get { return Core.Serializer<AppConfig>.Instance; } }

        /// <summary>
        /// 开发版本
        /// </summary>
        [BoxGroup("General Settings")]
        [LabelText("    Editor Development")]
        [OnValueChanged("OnSaveDescribe")]
        public bool isDevelopmentVersion;

        /// <summary>
        /// App 环境类型
        /// </summary>
        [BoxGroup("General Settings")]
        [LabelText("    Environment")]
        [OnValueChanged("OnSaveDescribe")]
        public AppEnvironmentType appEnvironmentType;

        /// <summary>
        /// 日志等级
        /// </summary>
        [BoxGroup("General Settings")]
        [LabelText("    Log Level")]
        [OnValueChanged("OnSaveDescribe")]
        public LogLevel logLevel;

        /// <summary>
        /// 版本远程URL
        /// </summary>
        [BoxGroup("URL Settings")]
        [LabelText("    Version Base Remote URL")]
        [OnValueChanged("OnSaveDescribe")]
        public string versionBaseURL;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            isDevelopmentVersion = Config.isDevelopmentVersion;
            appEnvironmentType = Config.appEnvironmentType;
            versionBaseURL = Config.versionBaseURL;
            logLevel = Config.logLevel;
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            if (Config == null) return;

            // General
            Config.isDevelopmentVersion = isDevelopmentVersion;
            Config.appEnvironmentType = appEnvironmentType;
            Config.versionBaseURL = versionBaseURL;

            // Debug
            Config.logLevel = logLevel;

            Config.Serialize();
        }
    }
}