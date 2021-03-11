// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-29 18:00:13
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Core;
using UnityEditor;

namespace UFramework.Editor.Preferences.Projrect
{
    public class ProjectPage : IPage, IPageBar
    {
        public string menuName => "Projrect";

        static AppConfig config => Serializer<AppConfig>.Instance;

        /// <summary>
        /// 是否使用系统语言
        /// </summary>
        [BoxGroup("General")] [LabelText("    Universal RP Project")]
        public bool isURP;

        /// <summary>
        /// 是否使用系统语言
        /// </summary>
        [BoxGroup("UI Settings")] [LabelText("    Use FairyGUI")] [OnValueChanged("OnValueChanged_useFairyGUI")]
        public bool useFairyGUI;

        private void OnValueChanged_useFairyGUI()
        {
            UpdateFairyGUISymbols();
        }

        /// <summary>
        /// 设计分辨率
        /// </summary>
        [BoxGroup("UI Settings")] [LabelText("    Design Resolution X")]
        public int designResolutionX = 2048;

        /// <summary>
        /// 设计分辨率
        /// </summary>
        [BoxGroup("UI Settings")] [LabelText("    Design Resolution Y")]
        public int designResolutionY = 1152;

        /// <summary>
        /// UI 资源目录
        /// </summary>
        [BoxGroup("UI Settings")] [LabelText("    Asset Directory")] [FolderPath]
        public string uiDirectory;

        [BoxGroup("Font Settings")] [LabelText("  ")] [ListDrawerSettings(Expanded = true)]
        public List<string> fonts = new List<string>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            isURP = config.isURP;

            useFairyGUI = config.useFairyGUI;
            designResolutionX = config.designResolutionX;
            designResolutionY = config.designResolutionY;
            uiDirectory = config.uiDirectory;

            fonts = config.fonts;
        }

        public void OnPageBarDraw()
        {
        }

        public void OnSaveDescribe()
        {
            if (config == null) return;

            config.isURP = isURP;

            config.useFairyGUI = useFairyGUI;
            config.designResolutionX = designResolutionX;
            config.designResolutionY = designResolutionY;
            config.uiDirectory = uiDirectory;
            config.fonts = fonts;
            config.Serialize();
        }

        private void UpdateFairyGUISymbols()
        {
            var symbol = "FAIRYGUI_TOLUA";
            if (useFairyGUI)
            {
                Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbol);
                Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbol);
                Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbol);
            }
            else
            {
                Utils.RemoveScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbol);
                Utils.RemoveScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbol);
                Utils.RemoveScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbol);
            }
        }

        private void UpdateOtherSymbols()
        {
            var symbol = "LUAC_5_3";
            Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbol);
            Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbol);
            Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbol);
        }

        public static void InitProject()
        {
            var page = new ProjectPage();
            page.OnRenderBefore();
            // 更新FairyGUI符号
            page.UpdateFairyGUISymbols();
            // 更新其他符号
            page.UpdateOtherSymbols();
        }
    }
}