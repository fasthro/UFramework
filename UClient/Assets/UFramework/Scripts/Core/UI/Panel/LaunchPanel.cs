/*
 * @Author: fasthro
 * @Date: 2020-10-12 13:06:32
 * @Description: Launch panel
 */

using System;
using FairyGUI;
using UFramework.Core;

namespace UFramework.UI
{
    public class LaunchPanel : FiaryPanel, IUpdater
    {
        private Controller _barCtr;
        private GProgressBar _bar;
        private GTextField _content;
        private GTextField _version;
        private Controller _touchCtr;
        private GComponent _touch;

        private Action _onTouch;

        private UpdaterStep _step;

        public static LaunchPanel Create()
        {
            var launch = new LaunchPanel {isResourceLoad = true};
            launch.AddPackage("RP_Launch");
            return launch;
        }

        public LaunchPanel() : base("Launch", "RP_Launch", Layer.PANEL)
        {
        }

        protected override void OnShow()
        {
            base.OnShow();
            
            _barCtr = view.GetController("_bar");
            _bar = view.GetChild("_bar").asProgress;
            _content = view.GetChild("_content").asTextField;
            _version = view.GetChild("_version").asTextField;
            _touchCtr = view.GetController("_touch");
            _touch = view.GetChild("_touch").asCom;
            _touch.onClick.Set(OnTouchBegin);

            _bar.max = 1;

            _barCtr.SetSelectedIndex(0);
            _touchCtr.SetSelectedIndex(0);
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
            else
                _barCtr.SetSelectedIndex(0);
        }

        public void OnProgress(float progress)
        {
            _bar.value = progress;
            var value = (int) (progress * 100f);
            if (_step == UpdaterStep.CheckFileCopy)
                _content.text = $"检查资源:({value}%)";
            else if (_step == UpdaterStep.Copy)
                _content.text = $"解压资源:({value}%)";
            else if (_step == UpdaterStep.Copy)
                _content.text = $"下载更新:({value}%)";
        }

        public void OnEndUpdate()
        {
        }
    }
}