// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 14:51
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using FairyGUI;
using UFramework.Core;

namespace UFramework.Consoles
{
    public class SystemTab : BaseConsoleTab
    {
        private static SystemService systemService;

        #region component

        private GList _list;
        private GButton _refreshBtn;

        #endregion

        private Dictionary<string, Dictionary<string, object>> _reportDict;
        private List<string> _titles;
        private List<Dictionary<string, object>> _values;

        private StringBuilder _stringBuilder;

        public SystemTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            systemService = Console.Instance.GetService<SystemService>();
            _stringBuilder = new StringBuilder();
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_systemTab").asCom;

            _list = _view.GetChild("_list").asList;
            _list.RemoveChildrenToPool();
            _list.SetVirtual();
            _list.itemRenderer = OnItemRenderer;

            _refreshBtn = _view.GetChild("_refresh").asButton;
            _refreshBtn.onClick.Set(OnRefreshClick);
        }

        protected override void OnShow()
        {
            Refresh();
        }

        protected override void OnRefresh()
        {
            _reportDict = systemService.CreateReport();

            _titles = new List<string>();
            _values = new List<Dictionary<string, object>>();
            foreach (var item in _reportDict)
            {
                _titles.Add(item.Key);
                _values.Add(item.Value);
            }

            _list.numItems = _titles.Count;
            _list.scrollPane.ScrollTop();
        }

        private void OnItemRenderer(int index, GObject obj)
        {
            var item = obj.asCom;
            var title = _titles[index];
            var values = _values[index];

            item.GetChild("_header").text = title;

            _stringBuilder.Clear();
            foreach (var value in values)
                _stringBuilder.AppendLine($"<font color=#BCBCBC>{value.Key}:</font>  {value.Value.ToString()}");

            item.GetChild("_text").text = _stringBuilder.ToString();
        }

        #region callback

        private void OnRefreshClick()
        {
            systemService.CreateSystemInfo();
            Refresh();
        }

        #endregion
    }
}