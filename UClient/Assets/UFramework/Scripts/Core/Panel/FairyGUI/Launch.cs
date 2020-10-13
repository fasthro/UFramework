/*
 * @Author: fasthro
 * @Date: 2020-10-12 13:06:32
 * @Description: Launch panel
 */
using FairyGUI;
using UFramework.UI;

namespace UFramework.Panel.FairyGUI
{
    public class Launch
    {

        public bool isShowing { get; private set; }

        private Window _window;

        public static Launch Create()
        {
            var launch = new Launch();
            launch.Init();
            return launch;
        }

        private void Init()
        {
            PackageAgents.LoadFairyResource("RP_Launch", () =>
            {
                _window = new Window();
                _window.MakeFullScreen();
                _window.contentPane = UIPackage.CreateObject("RP_Launch", "Launch").asCom;
                _window.contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
                _window.sortingOrder = (int)UFramework.UI.Layer.PANEL;
                Show();
            });
        }

        public void Show()
        {
            isShowing = true;
            if (_window != null)
                _window.Show();
        }

        public void Hide()
        {
            isShowing = false;
            if (_window != null)
                _window.Hide();
        }

        public void Dispose()
        {
            _window.Dispose();
            _window = null;
            PackageAgents.Unload("RP_Launch");
        }
    }
}