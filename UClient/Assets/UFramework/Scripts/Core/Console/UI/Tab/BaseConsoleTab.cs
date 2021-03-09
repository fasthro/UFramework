// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:46
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;

namespace UFramework.Consoles
{
    public class BaseConsoleTab : IConsolePanelTab
    {
        protected readonly ConsolePanel _consolePanel;
        protected GComponent _view;

        public bool initialized { get; protected set; }
        public bool isShowing { get; protected set; }

        protected BaseConsoleTab(ConsolePanel consolePanel)
        {
            _consolePanel = consolePanel;
        }

        protected virtual void OnInitialize()
        {
        }

        public void Show()
        {
            if (!initialized)
            {
                OnInitialize();
                initialized = true;
            }

            isShowing = true;
            OnShow();
        }

        protected virtual void OnShow()
        {
        }

        public void Hide()
        {
            isShowing = false;
            OnHide();
        }

        protected virtual void OnHide()
        {
        }

        public void Refresh()
        {
            if (!initialized || !isShowing)
                return;

            OnRefresh();
        }

        protected virtual void OnRefresh()
        {
        }

        public void Update()
        {
            if (!initialized || !isShowing)
                return;

            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}