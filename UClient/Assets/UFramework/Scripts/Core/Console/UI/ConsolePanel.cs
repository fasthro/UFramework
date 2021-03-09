// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 15:43
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        CommandMaximize,
        CommandMinimize
    }

    public class ConsolePanel : FiaryPanel
    {
        private Dictionary<ConsolePanelTab, BaseConsoleTab> _tabDict = new Dictionary<ConsolePanelTab, BaseConsoleTab>();

        #region component

        private Controller _stateController;
        private Controller _tabController;

        private GComboBox _functionComboBox;
        private GButton _closeButton;

        #endregion

        private ConsolePanelTab _tabType = ConsolePanelTab.None;
        private BaseConsoleTab _tab;

        public static ConsolePanel Create()
        {
            var panel = new ConsolePanel {isResourceLoad = true};
            panel.AddPackage("RP_Console");
            return panel;
        }

        public ConsolePanel() : base("Console", "RP_Console", Layer.Console)
        {
            AddTab(ConsolePanelTab.LogMaximize, new LogMaximizeTab(this));
            AddTab(ConsolePanelTab.LogMinimize, new LogMinimizeTab(this));
            AddTab(ConsolePanelTab.System, new SystemTab(this));
            AddTab(ConsolePanelTab.ProfilerMaximize, new ProfilerMaximizeTab(this));
            AddTab(ConsolePanelTab.ProfilerMinimize, new ProfilerMinimizeTab(this));
            AddTab(ConsolePanelTab.CommandMaximize, new CommandMaximizeTab(this));
            AddTab(ConsolePanelTab.CommandMinimize, new CommandMinimizeTab(this));
        }

        public void ShowTab(ConsolePanelTab tabType)
        {
            HideTab();

            switch (tabType)
            {
                case ConsolePanelTab.LogMaximize:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(0);
                    break;
                case ConsolePanelTab.LogMinimize:
                    _stateController.SetSelectedIndex(1);
                    _tabController.SetSelectedIndex(0);
                    break;
                case ConsolePanelTab.System:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(1);
                    break;
                case ConsolePanelTab.ProfilerMaximize:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(2);
                    break;
                case ConsolePanelTab.ProfilerMinimize:
                    _stateController.SetSelectedIndex(1);
                    _tabController.SetSelectedIndex(2);
                    break;
                case ConsolePanelTab.CommandMaximize:
                    _stateController.SetSelectedIndex(0);
                    _tabController.SetSelectedIndex(3);
                    break;
                case ConsolePanelTab.CommandMinimize:
                    _stateController.SetSelectedIndex(1);
                    _tabController.SetSelectedIndex(3);
                    break;
            }

            _tabType = tabType;
            _tab = GetTab(_tabType);
            _tab.Show();
        }

        private void HideTab()
        {
            GetTab(_tabType)?.Hide();
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
                if (_tabType != ConsolePanelTab.LogMaximize)
                {
                    ShowTab(ConsolePanelTab.LogMaximize);
                }
            }
            else if (_functionComboBox.selectedIndex == 1)
            {
                if (_tabType != ConsolePanelTab.System)
                {
                    ShowTab(ConsolePanelTab.System);
                }
            }
            else if (_functionComboBox.selectedIndex == 2)
            {
                if (_tabType != ConsolePanelTab.ProfilerMaximize)
                {
                    ShowTab(ConsolePanelTab.ProfilerMaximize);
                }
            }
            else if (_functionComboBox.selectedIndex == 3)
            {
                if (_tabType != ConsolePanelTab.CommandMaximize)
                {
                    ShowTab(ConsolePanelTab.CommandMaximize);
                }
            }
        }

        public void DoUpdate()
        {
            if (!isShowed)
                return;

            _tab?.Update();
        }

        private void AddTab(ConsolePanelTab tabType, BaseConsoleTab tab)
        {
            _tabDict.Add(tabType, tab);
        }

        public T GetTab<T>(ConsolePanelTab tabType) where T : BaseConsoleTab
        {
            return GetTab(tabType) as T;
        }

        public BaseConsoleTab GetTab(ConsolePanelTab tabType)
        {
            _tabDict.TryGetValue(tabType, out var tab);
            return tab;
        }
    }
}