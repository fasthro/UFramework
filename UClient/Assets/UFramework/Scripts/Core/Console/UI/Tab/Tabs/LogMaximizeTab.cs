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
    public class LogMaximizeTab : BaseConsoleTab, IConsolePanelTab
    {
        private static LogService logService;

        private GButton _debugButton;
        private GButton _warnButton;
        private GButton _errorButton;

        private GButton _trashButton;
        private GButton _minimizeButton;
        private GButton _downButton;

        private GList _logList;
        private GComponent _stackCom;
        private GRichTextField _stackText;

        private GTextInput _inputText;

        private CircularBuffer<LogEntry> _entries;

        private bool _isShowError = true;
        private bool _isShowWarn = true;
        private bool _isShowDebug = true;

        private LogEntry _selectedEntry;

        private bool _isUpdateDirty;

        public LogMaximizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            _entries = new CircularBuffer<LogEntry>(2048);
            logService = Console.Instance.GetService<LogService>();
        }

        private void OnLog(LogEntry entry)
        {
            if (entry == null)
                return;

            _isUpdateDirty = true;

            if (!Filter(entry, _inputText.text))
                _entries.PushBack(entry);
        }

        private void ReCheckLog()
        {
            _entries.Clear();
            var allEntries = logService.entries;
            var filterText = _inputText.text;
            for (var i = 0; i < allEntries.Size; i++)
            {
                var e = allEntries[i];
                if (!Filter(e, filterText))
                    _entries.PushBack(e);
            }
        }

        private bool Filter(LogEntry entry, string filterText)
        {
            if ((entry.logType == LogType.Error || entry.logType == LogType.Exception || entry.logType == LogType.Assert) && !_isShowError)
                return true;

            if (entry.logType == LogType.Warning && !_isShowWarn)
                return true;

            if (entry.logType == LogType.Log && !_isShowDebug)
                return true;

            if (!string.IsNullOrEmpty(filterText))
            {
                if (entry.message.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) < 0)
                    return true;
            }

            return false;
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_consoleMaxTab").asCom;

            _debugButton = _view.GetChild("_debug").asButton;
            _debugButton.onClick.Set(OnDebugClick);

            _warnButton = _view.GetChild("_warn").asButton;
            _warnButton.onClick.Set(OnWarnClick);

            _errorButton = _view.GetChild("_error").asButton;
            _errorButton.onClick.Set(OnErrorClick);

            _trashButton = _view.GetChild("_trash").asButton;
            _trashButton.onClick.Set(OnTrashClick);

            _minimizeButton = _view.GetChild("_minimize").asButton;
            _minimizeButton.onClick.Set(OnMinimizeClick);

            _inputText = _view.GetChild("_filterInput").asCom.GetChild("_input").asTextInput;
            _inputText.onChanged.Set(OnInputChanged);

            _downButton = _view.GetChild("_down").asButton;
            _downButton.onClick.Set(OnDownClick);

            _logList = _view.GetChild("_logList").asList;
            _logList.RemoveChildrenToPool();
            _logList.SetVirtual();
            _logList.itemRenderer = OnItemRenderer;
            _logList.onClickItem.Set(OnItemClick);

            _stackCom = _view.GetChild("_stack").asCom;
            _stackText = _stackCom.GetChild("_text").asRichTextField;
        }

        public void DoShow()
        {
            Initialize();

            _isUpdateDirty = true;

            _isShowDebug = true;
            _isShowWarn = true;
            _isShowError = true;

            logService.logListener += OnLog;

            _debugButton.selected = _isShowDebug;
            _warnButton.selected = _isShowWarn;
            _errorButton.selected = _isShowError;

            _selectedEntry = null;

            ReCheckLog();
            SetStackTrace(null);
        }

        public void DoRefresh()
        {
            _isUpdateDirty = false;

            _debugButton.title = GetNumberString(logService.debugCount, 999, "999+");
            _warnButton.title = GetNumberString(logService.warningCount, 999, "999+");
            _errorButton.title = GetNumberString(logService.errorCount, 999, "999+");

            var isBottomMost = _logList.scrollPane.isBottomMost;
            _logList.numItems = _entries.Size;
            if (isBottomMost)
                _logList.scrollPane.ScrollBottom();
        }

        public void DoUpdate()
        {
            if (_isUpdateDirty)
                DoRefresh();

            if (initialized)
                _downButton.visible = !_logList.scrollPane.isBottomMost;
        }

        public void DoHide()
        {
            logService.logListener -= OnLog;

            if (_selectedEntry != null)
                _selectedEntry.isSelected = false;
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
            item.GetController("_count").SetSelectedIndex(data.count > 1 ? 1 : 0);
            item.GetChild("_count").asTextField.text = data.count.ToString();
            if (data.isSelected)
            {
                item.GetController("_state").SetSelectedIndex(2);
            }
            else
            {
                item.GetController("_state").SetSelectedIndex(index % 2 == 0 ? 0 : 1);
            }

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

            SetStackTrace(_selectedEntry);
        }

        #endregion

        #region stack trace

        private void SetStackTrace(LogEntry entry)
        {
            if (entry == null)
            {
                _stackText.text = "";
            }
            else
            {
                var text = entry.message + Environment.NewLine +
                           (!string.IsNullOrEmpty(entry.stackTrace)
                               ? entry.stackTrace
                               : "No Stack Trace Available");

                if (text.Length > 2000)
                {
                    text = text.Substring(0, 2000);
                    text += "\nMessage Truncated";
                }

                _stackText.text = text;
            }

            _stackCom.container.SetXY(0, 0);
        }

        #endregion


        #region callback

        private void OnDebugClick()
        {
            _isShowDebug = _debugButton.selected;
            _isUpdateDirty = true;
            ReCheckLog();
        }

        private void OnWarnClick()
        {
            _isShowWarn = _warnButton.selected;
            _isUpdateDirty = true;
            ReCheckLog();
        }

        private void OnErrorClick()
        {
            _isShowError = _errorButton.selected;
            _isUpdateDirty = true;
            ReCheckLog();
        }

        private void OnTrashClick()
        {
            logService.Clear();
            _isUpdateDirty = true;
            ReCheckLog();
        }

        private void OnMinimizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.LogMinimize);
        }

        private void OnDownClick()
        {
            _logList.scrollPane.ScrollBottom();
        }

        private void OnInputChanged()
        {
            _isUpdateDirty = true;
            ReCheckLog();
        }

        #endregion

        static string GetNumberString(int value, int max, string exceedsMaxString)
        {
            return value >= max ? exceedsMaxString : value.ToString();
        }
    }
}