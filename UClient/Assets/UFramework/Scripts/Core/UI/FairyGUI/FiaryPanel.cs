/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:20:22
 * @Description: fiary panel
 */
using System.Collections.Generic;
using FairyGUI;

namespace UFramework.UI
{
    public class FiaryPanel : UIPanel
    {
        public Window window { get; private set; }

        private string _packageName;
        private string _panelName;

        public FiaryPanel(string panelName, string packageName, Layer layer) : base(layer)
        {
            window = new Window();
            _packageName = packageName;
            _panelName = panelName;
        }

        protected override void OnInit()
        {
            base.OnInit();
            window.MakeFullScreen();
            window.contentPane = UIPackage.CreateObject(_packageName, _panelName).asCom;
            window.contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
            window.sortingOrder = LayerAgents.Register(this);
        }

        protected override void OnShowAnimation()
        {
            base.OnShowAnimation();
        }

        protected override void OnShow()
        {
            base.OnShow();
            window.Show();
        }

        protected override void OnHide()
        {
            base.OnHide();
            window.Hide();
        }

        protected override void OnNotification(int id)
        {
            base.OnNotification(id);
        }

        protected override void OnNetMessage()
        {
            base.OnNetMessage();
        }

        public override void UpdateSortOrder(int order)
        {
            window.sortingOrder = order;
        }
    }
}