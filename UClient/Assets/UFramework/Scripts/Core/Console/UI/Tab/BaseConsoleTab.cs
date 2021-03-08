// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:46
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;

namespace UFramework.Consoles
{
    public class BaseConsoleTab
    {
        protected ConsolePanel _consolePanel;
        protected GComponent _view;
        
        public bool initialized { get; protected set; }

        public BaseConsoleTab(ConsolePanel consolePanel)
        {
            _consolePanel = consolePanel;
        }

        protected void Initialize()
        {
            if (!initialized)
            {
                OnInitialize();
                initialized = true;
            }
        }

        protected virtual void OnInitialize()
        {
            
        }
    }
}