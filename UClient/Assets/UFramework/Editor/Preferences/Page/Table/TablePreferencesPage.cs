/*
 * @Author: fasthro
 * @Date: 2020-06-29 11:26:04
 * @Description: Table Setting Page
 */
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UFramework.Table;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class TablePreferencesPage : IPage, IPageBar
    {
        public string menuName { get { return "Table/Preferences"; } }

        /// <summary>
        /// 导出数据格式
        /// </summary>
        [InfoBox("数据表数据首选项")]
        public FormatOptions outFormatOptions = FormatOptions.CSV;

        static TableConfig describeObject;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<TableConfig>();
            outFormatOptions = describeObject.outFormatOptions;
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Apply")))
            {
                describeObject.outFormatOptions = outFormatOptions;
                describeObject.Save();
            }
        }
    }
}