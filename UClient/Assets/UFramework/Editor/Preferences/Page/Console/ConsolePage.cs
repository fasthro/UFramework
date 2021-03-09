// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/03 16:32
// * @Description:
// --------------------------------------------------------------------------------

using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using UFramework.Consoles;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.Consoles
{
    public class ConsolePage : BasePage, IPage, IPageBar, IPageGUI
    {
        public string menuName => "Console";
        static AppConfig appConfig => Core.Serializer<AppConfig>.Instance;
        static ConsoleConfig consoleConfig => Core.Serializer<ConsoleConfig>.Instance;


        #region trigger

        [Title("Trigger")] [HorizontalGroup("Trigger1", 550)] [LabelWidth(200)]
        public TriggerAlignment triggerAlignment;

        [HorizontalGroup("Trigger2", 550)] [UnityEngine.Range(1, 10)] [LabelWidth(202)] [LabelText(" Trigger Time")]
        public int triggerTime;

        [HorizontalGroup("Trigger3", 550)] [UnityEngine.Range(0, 1)] [LabelWidth(202)] [LabelText(" Trigger HScreen Aspect")]
        public float triggerHScreenAspect = 0.1f;

        [HorizontalGroup("Trigger4", 550)] [UnityEngine.Range(0, 1)] [LabelWidth(202)] [LabelText(" Trigger VScreen Aspect")]
        public float triggerVScreenAspect = 0.1f;

        #endregion

        #region Log

        [Title("Log")] [HorizontalGroup("Log1", 550)] [LabelWidth(200)]
        public bool collapseLogEntries = true;

        [HorizontalGroup("Log2", 550)] [LabelWidth(202)] [UnityEngine.Range(0, 6000)] [LabelText(" Max Log Entries")]
        public int maxLogEntries = 2000;

        #endregion

        #region writer File

        [Title("Writer Log File")] [HorizontalGroup("Writer", 550)] [LabelWidth(202)] [LabelText("Enabled Writer File")]
        public bool enabledWriteFile = true;

        [HorizontalGroup("Writer1", 550)] [LabelWidth(202)] [LabelText(" Writer Interval Time(s)")] [UnityEngine.Range(1, 600)]
        public int writeIntervalTime = 1;

        #endregion

        #region profiler

        [Title("Profiler")] [HorizontalGroup("Profiler", 550)] [LabelWidth(202)] [LabelText("Enabled Memory")]
        public bool enabledMemory = true;

        [HorizontalGroup("Profiler1", 550)] [LabelWidth(202)] [LabelText(" Memory Refresh Interval Time(s)")] [UnityEngine.Range(1, 600)]
        public int memoryIntervalTime = 1;

        #endregion

        public ConsolePage(OdinMenuWindow window) : base(window)
        {
        }

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            triggerAlignment = consoleConfig.triggerAlignment;
            triggerTime = consoleConfig.triggerTime;
            triggerHScreenAspect = consoleConfig.triggerHScreenAspect / 100f;
            triggerVScreenAspect = consoleConfig.triggerVScreenAspect / 100f;

            collapseLogEntries = consoleConfig.collapseLogEntries;
            maxLogEntries = consoleConfig.maxLogEntries;

            enabledWriteFile = consoleConfig.enabledWriteFile;
            writeIntervalTime = consoleConfig.writeIntervalTime;

            enabledMemory = consoleConfig.enabledMemory;
            memoryIntervalTime = consoleConfig.memoryIntervalTime;
        }

        public void OnPageBarDraw()
        {
        }

        public void OnGUI()
        {
            OnDrawTrigger();
        }

        public void OnSaveDescribe()
        {
            consoleConfig.triggerAlignment = triggerAlignment;
            consoleConfig.triggerTime = triggerTime;
            consoleConfig.triggerHScreenAspect = Mathf.RoundToInt(triggerHScreenAspect * 100f);
            consoleConfig.triggerVScreenAspect = Mathf.RoundToInt(triggerVScreenAspect * 100f);

            consoleConfig.collapseLogEntries = collapseLogEntries;
            consoleConfig.maxLogEntries = maxLogEntries;

            consoleConfig.enabledWriteFile = enabledWriteFile;
            consoleConfig.writeIntervalTime = writeIntervalTime;
            
            consoleConfig.enabledMemory = enabledMemory;
            consoleConfig.memoryIntervalTime = memoryIntervalTime;

            consoleConfig.Serialize();
        }

        #region trigger

        private void OnDrawTrigger()
        {
            var boxWidth = 535;
            var boxHeight = boxWidth * ((float) appConfig.designResolutionY / (float) appConfig.designResolutionX);
            var boxX = _window.MenuWidth + 560;
            var boxY = 50;
            GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "", EditorStyles.helpBox);

            float areaWidth = boxWidth * triggerHScreenAspect;
            float areaHeight = boxHeight * triggerVScreenAspect;
            float areaX = 0;
            float areaY = 0;
            switch (triggerAlignment)
            {
                case TriggerAlignment.TopLeft:
                    areaX = boxX;
                    areaY = boxY;
                    break;
                case TriggerAlignment.TopRight:
                    areaX = boxWidth - areaWidth + boxX;
                    areaY = boxY;
                    break;
                case TriggerAlignment.BottomLeft:
                    areaX = boxX;
                    areaY = boxHeight - areaHeight + boxY;
                    break;
                case TriggerAlignment.BottomRight:
                    areaX = boxWidth - areaWidth + boxX;
                    areaY = boxHeight - areaHeight + boxY;
                    break;
                case TriggerAlignment.CenterLeft:
                    areaX = boxX;
                    areaY = (boxHeight - areaHeight) / 2f + boxY;
                    break;
                case TriggerAlignment.CenterRight:
                    areaX = boxWidth - areaWidth + boxX;
                    areaY = (boxHeight - areaHeight) / 2f + boxY;
                    break;
                case TriggerAlignment.TopCenter:
                    areaX = (boxWidth - areaWidth) / 2f + boxX;
                    areaY = boxY;
                    break;
                case TriggerAlignment.BottomCenter:
                    areaX = (boxWidth - areaWidth) / 2f + boxX;
                    areaY = boxHeight - areaHeight + boxY;
                    break;
            }

            GUI.Box(new Rect(areaX, areaY, areaWidth, areaHeight), "", EditorStyles.helpBox);
        }

        #endregion
    }
}