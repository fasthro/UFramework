// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 15:43
// * @Description:
// --------------------------------------------------------------------------------

using System;
using FairyGUI;
using UFramework.UI;

namespace UFramework.Consoles
{
    public enum ConsolePanelTab
    {
        LogMaximize,
        LogMinimize,
    }

    public class ConsolePanel : FiaryPanel
    {
        #region component

        private Controller _stateController;
        private Controller _tabController;

        private GComboBox _functionComboBox;
        private GButton _closeButton;

        #endregion

        #region tabs

        private LogMaximizeTab _logMaximizeTab;
        private LogMinimizeTab _logMinimizeTab;

        #endregion

        private ConsolePanelTab _curShowTab;

        public static ConsolePanel Create()
        {
            var panel = new ConsolePanel {isResourceLoad = true};
            panel.AddPackage("RP_Console");
            return panel;
        }

        public ConsolePanel() : base("Console", "RP_Console", Layer.Console)
        {
            _logMaximizeTab = new LogMaximizeTab(this);
            _logMinimizeTab = new LogMinimizeTab(this);
        }

        public void ShowTab(ConsolePanelTab tab)
        {
            _curShowTab = tab;

            switch (tab)
            {
                case ConsolePanelTab.LogMaximize:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(0);
                    _logMaximizeTab.DoShow();
                    break;
                case ConsolePanelTab.LogMinimize:
                    _stateController.SetSelectedIndex(1);
                    _tabController.SetSelectedIndex(1);
                    _logMinimizeTab.DoShow();
                    break;
            }
        }

        private void HideTab()
        {
            switch (_curShowTab)
            {
                case ConsolePanelTab.LogMaximize:
                    _logMaximizeTab.DoHide();
                    break;
                case ConsolePanelTab.LogMinimize:
                    _logMinimizeTab.DoHide();
                    break;
            }
        }

        protected override void OnShow()
        {
            base.OnShow();

            _stateController = view.GetController("_state");
            _tabController = view.GetController("_tab");

            _functionComboBox = view.GetChild("_function").asComboBox;
            _functionComboBox.onChanged.Set(OnFunctionComboBoxChanged);

            _closeButton = view.GetChild("_close").asButton;
            _closeButton.onClick.Set(Hide);
            
            ShowTab(ConsolePanelTab.LogMaximize);
        }

        protected override void OnHide()
        {
            base.OnHide();
            HideTab();
        }

        private void OnFunctionComboBoxChanged()
        {
            if (_functionComboBox.selectedIndex == 0)
            {
                if (_curShowTab != ConsolePanelTab.LogMaximize)
                {
                    HideTab();
                    ShowTab(ConsolePanelTab.LogMaximize);
                }
            }
        }

        public void DoUpdate()
        {
            if (!isShowed)
                return;

            switch (_curShowTab)
            {
                case ConsolePanelTab.LogMaximize:
                    _logMaximizeTab?.DoUpdate();
                    break;
                case ConsolePanelTab.LogMinimize:
                    _logMinimizeTab?.DoUpdate();
                    break;
            }
        }
    }
}