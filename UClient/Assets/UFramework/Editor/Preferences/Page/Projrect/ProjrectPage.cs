/*
 * @Author: fasthro
 * @Date: 2020-09-29 18:00:13
 * @Description: 项目
 */

using Sirenix.OdinInspector;
using UFramework.Core;
using UnityEditor;

namespace UFramework.Editor.Preferences.Projrect
{
    public class ProjrectPage : IPage, IPageBar
    {
        public string menuName { get { return "Projrect"; } }
        static AppConfig Config { get { return Serializer<AppConfig>.Instance; } }

        /// <summary>
        /// 是否使用系统语言
        /// </summary>
        [BoxGroup("UI Settings")]
        [LabelText("    Use FairyGUI")]
        [OnValueChanged("OnValueChanged_useFairyGUI")]
        public bool useFairyGUI;

        private void OnValueChanged_useFairyGUI()
        {
            UpdateFairyGUISymbols();
        }

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
            useFairyGUI = Config.useFairyGUI;
            designResolutionX = Config.designResolutionX;
            designResolutionY = Config.designResolutionY;
            uiDirectory = Config.uiDirectory;
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            if (Config == null) return;
            Config.useFairyGUI = useFairyGUI;
            Config.designResolutionX = designResolutionX;
            Config.designResolutionY = designResolutionY;
            Config.uiDirectory = uiDirectory;
            Config.Serialize();
        }

        private void UpdateFairyGUISymbols()
        {
            string symbol = "FAIRYGUI_TOLUA";
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
            string symbol = "LUAC_5_3";
            Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbol);
            Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbol);
            Utils.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbol);
        }

        public static void InitProject()
        {
            var page = new ProjrectPage();
            page.OnRenderBefore();
            // 更新FairyGUI符号
            page.UpdateFairyGUISymbols();
            // 更新其他符号
            page.UpdateOtherSymbols();
        }
    }
}