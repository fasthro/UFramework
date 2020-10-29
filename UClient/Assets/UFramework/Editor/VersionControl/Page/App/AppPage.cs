/*
 * @Author: fasthro
 * @Date: 2020-08-26 16:02:32
 * @Description: app page
 */
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class AppPage : IPage, IPageBar
    {
        public string menuName { get { return "Application"; } }
        static AppConfig describeObject;

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
        /// 版本远程URL
        /// </summary>
        [BoxGroup("URL Settings")]
        [LabelText("    Version Base Remote URL")]
        [OnValueChanged("OnSaveDescribe")]
        public string versionBaseURL;

        /// <summary>
        /// 日志
        /// </summary>
        [BoxGroup("Debug Settings")]
        [LabelText("    LogEnable")]
        [OnValueChanged("OnSaveDescribe")]
        public bool isLogEnable;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AppConfig>();
            isDevelopmentVersion = describeObject.isDevelopmentVersion;
            appEnvironmentType = describeObject.appEnvironmentType;
            versionBaseURL = describeObject.versionBaseURL;
            isLogEnable = describeObject.isLogEnable;
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            if (describeObject == null) return;

            // General
            describeObject.isDevelopmentVersion = isDevelopmentVersion;
            describeObject.appEnvironmentType = appEnvironmentType;
            describeObject.versionBaseURL = versionBaseURL;

            // Debug
            describeObject.isLogEnable = isLogEnable;

            describeObject.Save();
        }
    }
}