// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 16:28
// * @Description:
// --------------------------------------------------------------------------------

using System;
using FairyGUI;
using UFramework.Core;
using UnityEngine;
using Console = UFramework.Core.Console;

namespace UFramework.Consoles
{
    public class ConsolePanel_LogTab : IConsolePanelTab
    {
        private static LogService logService => Console.Instance.logService;

        private ConsolePanel _consolePanel;
        private GComponent _view;

        private GButton _debugButton;
        private GButton _warnButton;
        private GButton _errorButton;

        private GButton _trashButton;
        private GButton _filterButton;

        private GList _logList;
        private GComponent _stackCom;
        private GRichTextField _stackText;

        private CircularBuffer<LogEntry> _entries;

        private bool _isShowError = true;
        private bool _isShowWarn = true;
        private bool _isShowDebug = true;

        private LogEntry _selectedEntry;

        private bool _isUpdateDirty;

        public ConsolePanel_LogTab(ConsolePanel consolePanel)
        {
            _consolePanel = consolePanel;
            _entries = new CircularBuffer<LogEntry>(2048);
        }

        private void OnLogRefresh()
        {
            _isUpdateDirty = true;
        }

        public void DoShow()
        {
            logService.refreshListener += OnLogRefresh;

            _view = _consolePanel.view.GetChild("_consoleTab").asCom;

            _debugButton = _view.GetChild("_debug").asButton;
            _debugButton.onClick.Set(OnDebugClick);
            _debugButton.selected = _isShowDebug;

            _warnButton = _view.GetChild("_warn").asButton;
            _warnButton.onClick.Set(OnWarnClick);
            _warnButton.selected = _isShowWarn;

            _errorButton = _view.GetChild("_error").asButton;
            _errorButton.onClick.Set(OnErrorClick);
            _errorButton.selected = _isShowError;

            _trashButton = _view.GetChild("_trash").asButton;
            _trashButton.onClick.Set(OnTrashClick);

            _filterButton = _view.GetChild("_filter").asButton;
            _filterButton.onClick.Set(OnFilterClick);

            _logList = _view.GetChild("_loglist").asList;
            _logList.RemoveChildrenToPool();
            _logList.SetVirtual();
            _logList.itemRenderer = OnItemRenderer;
            _logList.onClickItem.Set(OnItemClick);

            _stackCom = _view.GetChild("_stack").asCom;
            _stackText = _stackCom.GetChild("_text").asRichTextField;

            _isUpdateDirty = true;
        }
        
        public void DoRefresh()
        {
            _isUpdateDirty = false;

            _debugButton.title = GetNumberString(logService.debugCount, 999, "999+");
            _warnButton.title = GetNumberString(logService.warningCount, 999, "999+");
            _errorButton.title = GetNumberString(logService.errorCount, 999, "999+");

            _entries.Clear();
            var allEntries = logService.entries;
            for (var i = 0; i < allEntries.Size; i++)
            {
                var e = allEntries[i];

                if ((e.logType == LogType.Error || e.logType == LogType.Exception || e.logType == LogType.Assert) && !_isShowError)
                {
                }

                if (e.logType == LogType.Warning && !_isShowWarn)
                {
                    continue;
                }

                if (e.logType == LogType.Log && !_isShowDebug)
                {
                    continue;
                }

                // if (!string.IsNullOrEmpty(Filter))
                // {
                //     if (e.message.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) < 0)
                //     {
                //         continue;
                //     }
                // }

                _entries.PushBack(e);
            }

            _logList.numItems = _entries.Size;
        }

        public void DoUpdate()
        {
            if (!_isUpdateDirty)
                return;
            DoRefresh();
        }

        public void DoHide()
        {
            logService.refreshListener -= OnLogRefresh;
        }

        #region log list

        private void OnItemRenderer(int index, GObject obj)
        {
            var item = obj.asCom;
            var data = _entries[index];

            var logType = item.GetController("_logType");
            switch (data.logType)
            {
                case LogType.Log:
                    logType.SetSelectedIndex(2);
                    break;
                case LogType.Warning:
                    logType.SetSelectedIndex(1);
                    break;
                case LogType.Assert:
                case LogType.Exception:
                case LogType.Error:
                    logType.SetSelectedIndex(0);
                    break;
            }

            item.GetChild("_text").asRichTextField.text = data.messagePreview;
            item.GetChild("_stackText").asRichTextField.text = data.stackTracePreview;
            item.GetController("_selected").SetSelectedIndex(data.isSelected ? 1 : 0);

            obj.data = data;
        }

        private void OnItemClick(EventContext context)
        {
            if (_selectedEntry != null)
                _selectedEntry.isSelected = false;

            var item = context.data as GComponent;
            _selectedEntry = item.data as LogEntry;
            _selectedEntry.isSelected = true;

            _logList.RefreshVirtualList();
        }

        #endregion


        #region callback

        private void OnDebugClick()
        {
            _isShowDebug = _debugButton.selected;
            _isUpdateDirty = true;
        }

        private void OnWarnClick()
        {
            _isShowWarn = _warnButton.selected;
            _isUpdateDirty = true;
        }

        private void OnErrorClick()
        {
            _isShowError = _errorButton.selected;
            _isUpdateDirty = true;
        }

        private void OnTrashClick()
        {
            logService.Clear();
        }

        private void OnFilterClick()
        {
        }

        #endregion

        static string GetNumberString(int value, int max, string exceedsMaxString)
        {
            return value >= max ? exceedsMaxString : value.ToString();
        }
    }
}