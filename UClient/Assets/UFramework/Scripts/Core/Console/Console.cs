// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 13:04
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UFramework.Consoles;

namespace UFramework.Core
{
    [MonoSingletonPath("UFramework/Console")]
    public class Console : MonoSingleton<Console>
    {
        private Dictionary<Type, BaseConsoleService> _serviceDict;
        public ConsolePanel consolePanel { get; private set; }

        protected override void OnSingletonAwake()
        {
            _serviceDict = new Dictionary<Type, BaseConsoleService>();
         
            CreateService<LogService>();
            CreateService<TriggerService>();
            CreateService<LogFileService>();
            CreateService<FPSService>();
            CreateService<SystemService>();
            CreateService<MemoryService>();
            CreateService<CommandService>();

            foreach (var service in _serviceDict)
                service.Value.Initialize();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            foreach (var service in _serviceDict)
                service.Value.Update();

            consolePanel?.DoUpdate();
        }

        protected override void OnSingletonDestory()
        {
            foreach (var service in _serviceDict)
                service.Value.Dispose();
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

        private void CreateService<T>() where T : BaseConsoleService, new()
        {
            var service = new T();
            _serviceDict.Add(service.GetType(), service);
        }

        public T GetService<T>() where T : BaseConsoleService, new()
        {
            var type = typeof(T);
            _serviceDict.TryGetValue(type, out var service);
            return service as T;
        }
    }
}