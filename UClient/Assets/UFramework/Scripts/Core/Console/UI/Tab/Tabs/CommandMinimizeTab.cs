// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:04
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;

namespace UFramework.Consoles
{
    public class CommandMinimizeTab : BaseConsoleTab
    {
        #region component

        private GTextInput _input;
        private GButton _clearButton;
        
        private GButton _maximizeButton;

        #endregion

        public CommandMinimizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_commandMinTab").asCom;
            
            var inputCom = _view.GetChild("_input").asCom;
            _input = inputCom.GetChild("_input").asTextInput;
            
            _clearButton = inputCom.GetChild("_clear").asButton;
            _clearButton.onClick.Set(OnClearClick);

            _maximizeButton = _view.GetChild("_maximize").asButton;
            _maximizeButton.onClick.Set(OnMaximizeClick);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnRefresh()
        {
        }

        protected override void OnUpdate()
        {
        }

        private void OnMaximizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.CommandMaximize);
        }
        
        private void OnClearClick()
        {
            _input.text = "";
        }
    }
}