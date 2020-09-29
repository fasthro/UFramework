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
            uiDirectory = describeObject.uiDirectory;
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            if (describeObject == null) return;
            describeObject.useFairyGUI = useFairyGUI;
            describeObject.uiDirectory = uiDirectory;
            describeObject.Save();
        }
    }
}