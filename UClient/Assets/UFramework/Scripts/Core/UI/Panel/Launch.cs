/*
 * @Author: fasthro
 * @Date: 2020-10-12 13:06:32
 * @Description: Launch panel
 */
using System;
using FairyGUI;
using UFramework.UI;
using UFramework.Core;
using UnityEngine;

namespace UFramework.UI
{
    public class Launch : IUpdater
    {
        public bool isShowing { get; private set; }

        private Window _window;

        private Controller _barCtr;
        private GProgressBar _bar;
        private GTextField _content;
        private GTextField _version;
        private Controller _touchCtr;
        private GComponent _touch;

        private Action _onTouch;

        private UpdaterStep _step;

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

            _barCtr = _window.contentPane.GetController("_bar");
            _bar = _window.contentPane.GetChild("_bar").asProgress;
            _content = _window.contentPane.GetChild("_content").asTextField;
            _version = _window.contentPane.GetChild("_version").asTextField;
            _touchCtr = _window.contentPane.GetController("_touch");
            _touch = _window.contentPane.GetChild("_touch").asCom;
            _touch.onClick.Set(OnTouchBegin);

            _bar.max = 1;

            _barCtr.SetSelectedIndex(0);
            _touchCtr.SetSelectedIndex(0);
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

        #region touch begin

        public void ShowTouchBeginOperation(Action onTouch)
        {
            _onTouch = onTouch;
            _touchCtr.SetSelectedIndex(1);
        }

        private void OnTouchBegin()
        {
            _onTouch.InvokeGracefully();
        }

        #endregion

        public void OnStartUpdate()
        {
            _version.text = "";
        }

        public void OnStep(UpdaterStep step)
        {
            _step = step;
            if (step == UpdaterStep.CheckFileCopy)
            {
                _barCtr.SetSelectedIndex(1);
                _version.text = UApplication.Version;
            }
            else if (step == UpdaterStep.Copy)
                _barCtr.SetSelectedIndex(1);
            else if (step == UpdaterStep.Copy)
                _barCtr.SetSelectedIndex(1);
            else
                _barCtr.SetSelectedIndex(0);
        }

        public void OnProgress(float progress)
        {
            _bar.value = progress;
            var value = (int)(progress * 100f);
            if (_step == UpdaterStep.CheckFileCopy)
                _content.text = string.Format("检查资源:({0}%)", value);
            else if (_step == UpdaterStep.Copy)
                _content.text = string.Format("解压资源:({0}%)", value);
            else if (_step == UpdaterStep.Copy)
                _content.text = string.Format("下载更新:({0}%)", value);
        }

        public void OnEndUpdate()
        {
        }
    }
}