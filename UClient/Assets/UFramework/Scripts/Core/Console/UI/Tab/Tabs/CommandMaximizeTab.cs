// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:04
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Consoles
{
    public class CommandMaximizeTab : BaseConsoleTab
    {
        private static CommandService commandService;

        #region component

        private GList _list;
        private GList _historyList;
        private GButton _minimizeButton;

        private GComponent _inputCom;
        private GTextInput _input;
        private GButton _clearButton;
        private GButton _historyButton;
        private GButton _sendButton;

        #endregion

        private Command[] _commands;
        private HistoryCommand[] _historyCommands;
        private bool _historySelected;


        public CommandMaximizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            commandService = Console.Instance.GetService<CommandService>();
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_commandMaxTab").asCom;

            _list = _view.GetChild("_list").asList;
            _list.RemoveChildrenToPool();
            _list.SetVirtual();
            _list.itemRenderer = OnItemRenderer;
            _list.onClickItem.Set(OnItemClick);

            _historyList = _view.GetChild("_history").asList;
            _historyList.RemoveChildrenToPool();
            _historyList.SetVirtual();
            _historyList.itemRenderer = OnHistoryItemRenderer;
            _historyList.onClickItem.Set(OnHistoryItemClick);
            
            _inputCom = _view.GetChild("_input").asCom;
            _input = _inputCom.GetChild("_input").asTextInput;
            _input.onSubmit.Set(onSubmit);
            _input.onFocusIn.Set(onFocusIn);

            _clearButton = _inputCom.GetChild("_clear").asButton;
            _clearButton.onClick.Set(OnClearClick);

            _historyButton = _inputCom.GetChild("_history").asButton;
            _historyButton.onClick.Set(OnHistoryClick);

            _sendButton = _inputCom.GetChild("_send").asButton;
            _sendButton.onClick.Set(onSubmit);

            _minimizeButton = _view.GetChild("_minimize").asButton;
            _minimizeButton.onClick.Set(OnMinimizeClick);
        }

        protected override void OnShow()
        {
            _historySelected = false;
            _historyButton.selected = _historySelected;
            _historyList.height = 0;
            _historyList.y = _inputCom.y - 10;

            _input.text = "";

            if (_commands == null)
                _commands = commandService.CollectCommands();

            if (_historyCommands == null)
                _historyCommands = commandService.GetHistoryCommands(true, 0);

            Refresh();
        }

        protected override void OnRefresh()
        {
            _list.numItems = _commands?.Length ?? 0;
            _list.scrollPane.ScrollTop();
        }

        #region list

        private void OnItemRenderer(int index, GObject obj)
        {
            var item = obj.asCom;
            var cmd = _commands[index];

            item.GetChild("_name").asTextField.text = cmd.name;
            item.GetChild("_des").asTextField.text = $"({cmd.description})";

            item.GetController("_style").SetSelectedIndex(index % 2 == 0 ? 0 : 1);

            item.data = cmd;
        }

        private void OnItemClick(EventContext context)
        {
            var item = context.data as GComponent;
            var cmd = item.data as Command;

            _input.text = $"{cmd.name} {cmd.paramStatement}";
            _input.RequestFocus();
        }

        #endregion

        #region history

        private void OnHistoryItemRenderer(int index, GObject obj)
        {
            var item = obj.asCom;
            var cmd = _historyCommands[index];

            item.GetChild("_name").asTextField.text = cmd.command.name;
            item.GetChild("_des").asTextField.text = $"({cmd.command.description})";
            item.GetChild("_content").asTextField.text = $">> [{cmd.content}]";

            item.GetController("_style").SetSelectedIndex(index % 2 == 0 ? 0 : 1);

            item.data = cmd;
        }

        private void OnHistoryItemClick(EventContext context)
        {
            var item = context.data as GComponent;
            var cmd = item.data as HistoryCommand;

            _input.text = cmd.content;
            _input.RequestFocus();
        }

        private void OnHistoryClick()
        {
            VisibleHistory(!_historySelected);
        }

        private void VisibleHistory(bool visible)
        {
            if (visible)
            {
                _historyCommands = commandService.GetHistoryCommands(true, 0);
                _historyList.numItems = _historyCommands.Length;

                var dpCount = Mathf.Min(_historyCommands.Length, 3);
                _historyList.height = dpCount * 50f;
                _historyList.y = _inputCom.y - _historyList.height - 10;

                _historySelected = true;
                _historyButton.selected = true;
            }
            else
            {
                _historySelected = false;
                _historyButton.selected = false;
                _historyList.height = 0;
                _historyList.y = _inputCom.y - _historyList.height - 10;
            }
        }

        #endregion

        #region input

        private void OnClearClick()
        {
            _input.text = "";
        }

        private void onSubmit()
        {
            var text = _input.text;
            if (string.IsNullOrEmpty(text))
                return;

            commandService.ExecuteCommand(_input.text);
            _input.text = "";

            VisibleHistory(false);
        }

        private void onFocusIn()
        {
            VisibleHistory(true);
        }

        #endregion

        private void OnMinimizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.CommandMinimize);
        }
    }
}