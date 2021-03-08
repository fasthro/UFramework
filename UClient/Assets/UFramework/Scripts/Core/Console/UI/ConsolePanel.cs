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
        None,
        LogMaximize,
        LogMinimize,
        System,
        ProfilerMaximize,
        ProfilerMinimize,
        Command,
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

        private SystemTab _systemTab;

        private ProfilerMaximizeTab _profilerMaximizeTab;
        private ProfilerMinimizeTab _profilerMinimizeTab;

        private CommandTab _commandTab;

        #endregion

        private ConsolePanelTab _curShowTab = ConsolePanelTab.None;

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

            _systemTab = new SystemTab(this);

            _profilerMaximizeTab = new ProfilerMaximizeTab(this);
            _profilerMinimizeTab = new ProfilerMinimizeTab(this);

            _commandTab = new CommandTab(this);
        }

        public void ShowTab(ConsolePanelTab tab)
        {
            HideTab();

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
                case ConsolePanelTab.System:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(2);
                    _systemTab.DoShow();
                    break;
                case ConsolePanelTab.ProfilerMaximize:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(3);
                    _profilerMaximizeTab.DoShow();
                    break;
                case ConsolePanelTab.ProfilerMinimize:
                    _stateController.SetSelectedIndex(1);
                    _tabController.SetSelectedIndex(4);
                    _profilerMinimizeTab.DoShow();
                    break;
                case ConsolePanelTab.Command:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(5);
                    _commandTab.DoShow();
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
                case ConsolePanelTab.System:
                    _systemTab.DoHide();
                    break;
                case ConsolePanelTab.ProfilerMaximize:
                    _profilerMaximizeTab.DoHide();
                    break;
                case ConsolePanelTab.ProfilerMinimize:
                    _profilerMinimizeTab.DoHide();
                    break;
                case ConsolePanelTab.Command:
                    _commandTab.DoHide();
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
                    ShowTab(ConsolePanelTab.LogMaximize);
                }
            }
            else if (_functionComboBox.selectedIndex == 1)
            {
                if (_curShowTab != ConsolePanelTab.System)
                {
                    ShowTab(ConsolePanelTab.System);
                }
            }
            else if (_functionComboBox.selectedIndex == 2)
            {
                if (_curShowTab != ConsolePanelTab.ProfilerMaximize)
                {
                    ShowTab(ConsolePanelTab.ProfilerMaximize);
                }
            }
            else if (_functionComboBox.selectedIndex == 3)
            {
                if (_curShowTab != ConsolePanelTab.Command)
                {
                    ShowTab(ConsolePanelTab.Command);
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
                case ConsolePanelTab.System:
                    _systemTab?.DoUpdate();
                    break;
                case ConsolePanelTab.ProfilerMaximize:
                    _profilerMaximizeTab?.DoUpdate();
                    break;
                case ConsolePanelTab.ProfilerMinimize:
                    _profilerMinimizeTab?.DoUpdate();
                    break;
                case ConsolePanelTab.Command:
                    _commandTab?.DoUpdate();
                    break;
            }
        }
    }
}