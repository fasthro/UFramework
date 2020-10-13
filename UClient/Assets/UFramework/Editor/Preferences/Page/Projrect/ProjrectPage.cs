/*
 * @Author: fasthro
 * @Date: 2020-09-29 18:00:13
 * @Description: 项目
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class ProjrectPage : IPage, IPageBar
    {
        public string menuName { get { return "Projrect"; } }

        static AppConfig describeObject;

        /// <summary>
        /// 是否使用系统语言
        /// </summary>
        [BoxGroup("UI Settings")]
        [LabelText("    Use FairyGUI")]
        public bool useFairyGUI;

        /// <summary>
        /// 设计分辨率
        /// </summary>
        [BoxGroup("UI Settings")]
        [LabelText("    Design Resolution X")]
        public int designResolutionX = 2048;

        /// <summary>
        /// 设计分辨率
        /// </summary>
        [BoxGroup("UI Settings")]
        [LabelText("    Design Resolution Y")]
        public int designResolutionY = 1152;

        /// <summary>
        /// UI 资源目录
        /// </summary>
        [BoxGroup("UI Settings")]
        [LabelText("    Asset Directory")]
        [FolderPath]
        public string uiDirectory;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AppConfig>();
            useFairyGUI = describeObject.useFairyGUI;
            designResolutionX = describeObject.designResolutionX;
            designResolutionY = describeObject.designResolutionY;
            uiDirectory = describeObject.uiDirectory;
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            if (describeObject == null) return;
            describeObject.useFairyGUI = useFairyGUI;
            describeObject.designResolutionX = designResolutionX;
            describeObject.designResolutionY = designResolutionY;
            describeObject.uiDirectory = uiDirectory;
            describeObject.Save();
        }
    }
}