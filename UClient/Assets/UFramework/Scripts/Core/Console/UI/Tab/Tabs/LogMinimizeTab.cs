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
    public class LogMinimizeTab : BaseConsoleTab
    {
        private static LogService logService;

        private GButton _debugButton;
        private GButton _warnButton;
        private GButton _errorButton;

        private GButton _trashButton;
        private GButton _maximizeButton;
        private GButton _dropdownButton;
        private Controller _dropdownController;

        private GButton _dragButton;

        private GList _logList;

        private GTextInput _inputText;

        private Controller _visibleController;

        private CircularBuffer<LogEntry> _entries;

        private bool _isShowError = true;
        private bool _isShowWarn = true;
        private bool _isShowDebug = true;

        private bool _isUpdateDirty;

        private bool _isVisible = true;

        private Vector2 _dragStartPos;
        private float _listHeight;

        public LogMinimizeTab(ConsolePanel consolePanel) : base(consolePanel)
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
            _view = _consolePanel.view.GetChild("_consoleMinTab").asCom;

            _visibleController = _view.GetController("_visible");

            _debugButton = _view.GetChild("_debug").asButton;
            _debugButton.onClick.Set(OnDebugClick);

            _warnButton = _view.GetChild("_warn").asButton;
            _warnButton.onClick.Set(OnWarnClick);

            _errorButton = _view.GetChild("_error").asButton;
            _errorButton.onClick.Set(OnErrorClick);

            _trashButton = _view.GetChild("_trash").asButton;
            _trashButton.onClick.Set(OnTrashClick);

            _maximizeButton = _view.GetChild("_maximize").asButton;
            _maximizeButton.onClick.Set(OnMaximizeClick);

            _dropdownButton = _view.GetChild("_dropdown").asButton;
            _dropdownButton.onClick.Set(OnDropdownClick);
            _dropdownController = _dropdownButton.GetController("_state");

            _dragButton = _view.GetChild("_drag").asButton;
            _dragButton.onTouchBegin.Set(OnDragBegin);
            _dragButton.onTouchMove.Set(OnDragMove);

            _inputText = _view.GetChild("_filterInput").asCom.GetChild("_input").asTextInput;
            _inputText.onChanged.Set(OnInputChanged);

            _logList = _view.GetChild("_logList").asList;
            _logList.RemoveChildrenToPool();
            _logList.SetVirtual();
            _logList.itemRenderer = OnItemRenderer;
        }

        protected override void OnShow()
        {
            _isUpdateDirty = true;

            _isVisible = true;

            _isShowDebug = true;
            _isShowWarn = true;
            _isShowError = true;

            logService.logListener += OnLog;

            _debugButton.selected = _isShowDebug;
            _warnButton.selected = _isShowWarn;
            _errorButton.selected = _isShowError;

            _dropdownController.SetSelectedIndex(1);
            _visibleController.SetSelectedIndex(0);
        }

        protected override void OnRefresh()
        {
            _isUpdateDirty = false;

            _debugButton.title = GetNumberString(logService.debugCount, 999, "999+");
            _warnButton.title = GetNumberString(logService.warningCount, 999, "999+");
            _errorButton.title = GetNumberString(logService.errorCount, 999, "999+");

            _entries.Clear();
            var allEntries = logService.entries;
            var filterText = _inputText.text;
            for (var i = 0; i < allEntries.Size; i++)
            {
                var e = allEntries[i];

                if ((e.logType == LogType.Error || e.logType == LogType.Exception || e.logType == LogType.Assert) && !_isShowError)
                {
                    continue;
                }

                if (e.logType == LogType.Warning && !_isShowWarn)
                {
                    continue;
                }

                if (e.logType == LogType.Log && !_isShowDebug)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(filterText))
                {
                    if (e.message.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        continue;
                    }
                }

                _entries.PushBack(e);
            }

            var isBottomMost = _logList.scrollPane.isBottomMost;
            _logList.numItems = _entries.Size;
            if (isBottomMost)
                _logList.scrollPane.ScrollBottom();
        }

        protected override void OnUpdate()
        {
            if (!_isUpdateDirty)
                return;
            
            Refresh();
        }

        protected override void OnHide()
        {
            logService.logListener -= OnLog;
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

            item.GetController("_state").SetSelectedIndex(index % 2 == 0 ? 0 : 1);

            obj.data = data;
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
            _isUpdateDirty = true;
            logService.Clear();
            ReCheckLog();
        }

        private void OnMaximizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.LogMaximize);
        }

        private void OnDropdownClick()
        {
            _isVisible = !_isVisible;
            _dropdownController.SetSelectedIndex(_isVisible ? 1 : 0);
            _visibleController.SetSelectedIndex(_isVisible ? 0 : 1);
        }

        private void OnInputChanged()
        {
            _isUpdateDirty = true;
            ReCheckLog();
        }

        private void OnDragBegin()
        {
            _listHeight = _logList.height;
            _dragStartPos = Stage.inst.touchPosition;
        }

        private void OnDragMove()
        {
            var value = Stage.inst.touchPosition.y - _dragStartPos.y;
            var newHeight = _listHeight + value;
            if (newHeight < 80)
                newHeight = 80;
            var max = GRoot.inst.height - _logList.y - _dragButton.height;
            if (newHeight > max)
                newHeight = max;

            _logList.height = newHeight;
        }

        #endregion

        static string GetNumberString(int value, int max, string exceedsMaxString)
        {
            return value >= max ? exceedsMaxString : value.ToString();
        }
    }
}