﻿// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 13:04
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Consoles;
using UFramework.UI;

namespace UFramework.Core
{
    [MonoSingletonPath("UFramework/Console")]
    public class Console : MonoSingleton<Console>
    {
        #region service

        public LogService logService { get; private set; }
        public LogFileService logFileService { get; private set; }
        public TriggerService triggerService { get; private set; }

        #endregion

        public ConsolePanel consolePanel { get; private set; }

        protected override void OnSingletonAwake()
        {
            logService = CreateService<LogService>();
        }

        protected override void OnSingletonStart()
        {
            triggerService = CreateService<TriggerService>();
            logFileService = CreateService<LogFileService>();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            triggerService.Update();
            logFileService.Update();

            consolePanel?.DoUpdate();
        }

        protected override void OnSingletonDestory()
        {
            logService.Dispose();
            triggerService.Dispose();
            logFileService.Dispose();
        }

        public void TriggerConsolePanel()
        {
            if (consolePanel == null || !consolePanel.isShowed)
                ShowConsolePanel();
        }

        public void ShowConsolePanel()
        {
            if (consolePanel == null)
            {
                consolePanel = ConsolePanel.Create();
                consolePanel.Show();
            }
            else
            {
                if (!consolePanel.isShowed)
                    consolePanel.Show();
            }
        }

        static T CreateService<T>() where T : BaseConsoleService, new()
        {
            var service = new T();
            service.Initialize();
            return service;
        }
    }
}