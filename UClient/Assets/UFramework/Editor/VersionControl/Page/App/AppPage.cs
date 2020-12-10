/*
 * @Author: fasthro
 * @Date: 2020-08-26 16:02:32
 * @Description: app page
 */
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class AppPage : IPage, IPageBar
    {
        public string menuName { get { return "Application"; } }
        static AppSerdata Serdata { get { return Serialize.Serializable<AppSerdata>.Instance; } }

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
            isDevelopmentVersion = Serdata.isDevelopmentVersion;
            appEnvironmentType = Serdata.appEnvironmentType;
            versionBaseURL = Serdata.versionBaseURL;
            logLevel = Serdata.logLevel;
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            if (Serdata == null) return;

            // General
            Serdata.isDevelopmentVersion = isDevelopmentVersion;
            Serdata.appEnvironmentType = appEnvironmentType;
            Serdata.versionBaseURL = versionBaseURL;

            // Debug
            Serdata.logLevel = logLevel;

            Serdata.Serialization();
        }
    }
}