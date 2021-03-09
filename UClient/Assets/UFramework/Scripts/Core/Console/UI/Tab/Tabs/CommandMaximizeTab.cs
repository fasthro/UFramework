// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:04
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;

namespace UFramework.Consoles
{
    public class CommandMaximizeTab : BaseConsoleTab
    {
        private static CommandService commandService;

        #region component

        private GList _list;
        private GButton _minimizeButton;
        
        private GTextInput _input;
        private GButton _clearButton;
        
        #endregion

        private Command[] _commands;

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

            var inputCom = _view.GetChild("_input").asCom;
            _input = inputCom.GetChild("_input").asTextInput;
            
            _clearButton = inputCom.GetChild("_clear").asButton;
            _clearButton.onClick.Set(OnClearClick);

            _minimizeButton = _view.GetChild("_minimize").asButton;
            _minimizeButton.onClick.Set(OnMinimizeClick);
        }

        protected override void OnShow()
        {
            if (_commands == null)
                _commands = commandService.CollectCommands();

            Refresh();
        }

        protected override void OnHide()
        {
        }

        protected override void OnRefresh()
        {
            _list.numItems = _commands?.Length ?? 0;
            _list.scrollPane.ScrollTop();
        }

        protected override void OnUpdate()
        {
        }

        private void OnItemRenderer(int index, GObject obj)
        {
            var item = obj.asCom;
            var cmd = _commands[index];

            item.GetChild("_name").asTextField.text = cmd.name;
            item.GetChild("_des").asTextField.text = cmd.description;

            item.GetController("_style").SetSelectedIndex(index % 2 == 0 ? 0 : 1);

            item.data = cmd;
        }

        private void OnItemClick(EventContext context)
        {
            var item = context.data as GComponent;
            var cmd = item.data as Command;

            _input.text = $"{cmd.name} {cmd.paramStatement}";
        }

        private void OnMinimizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.CommandMinimize);
        }

        private void OnClearClick()
        {
            _input.text = "";
        }
    }
}