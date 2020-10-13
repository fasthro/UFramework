/*
 * @Author: fasthro
 * @Date: 2020-10-12 10:25:05
 * @Description: messagebox
 */
using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UFramework.Pool;
using UFramework.UI;

namespace UFramework.Panel.FairyGUI
{
    public class MessageBox : IEnumerator, IPoolObject
    {
        #region IEnumerator
        public bool isOK { get; private set; }

        public object Current { get { return null; } }

        public bool MoveNext()
        {
            return isShowing;
        }

        public void Reset()
        {

        }

        #endregion

        #region pool

        public bool isRecycled { get; set; }

        public static MessageBox Allocate()
        {
            return ObjectPool<MessageBox>.Instance.Allocate();
        }

        public void Recycle()
        {
            ObjectPool<MessageBox>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            _title = null;
            _content = null;
            _okBtn = null;
            _cancelBtn = null;
            onCompleted = null;
        }

        #endregion

        #region message box
        private Window _window;

        private GTextField _title;
        private GRichTextField _content;
        private GButton _okBtn;
        private GButton _cancelBtn;
        private GButton _closeBtn;
        private Controller _btn;

        /// <summary>
        /// 是否显示
        /// </summary>
        /// <value></value>
        public bool isShowing
        {
            get
            {
                return _window != null && _window.isShowing;
            }
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <value>true/false</value>
        public Action<bool> onCompleted { get; set; }

        /// <summary>
        /// 显示得列表
        /// </summary>
        /// <typeparam name="MessageBox"></typeparam>
        /// <returns></returns>
        readonly static List<MessageBox> showeds = new List<MessageBox>();

        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="ok">OK按钮标题</param>
        /// <param name="cancel">取消按钮标题</param>
        /// <returns></returns>
        public MessageBox Show(string title, string content, string ok = "OK", string cancel = "Cancel")
        {
            Init(title, content, ok, cancel);
            return this;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Hide()
        {
            if (_window == null)
                return;

            _window.Hide();
            _window.Dispose();
            _window = null;
            showeds.Remove(this);
            PackageAgents.Unload("RP_MessageBox");
            Recycle();
        }

        /// <summary>
        /// 关闭所有显示MessageBox
        /// </summary>
        public static void HideAll()
        {
            for (int i = 0; i < showeds.Count; i++)
            {
                showeds[0].Hide();
            }
        }

        /// <summary>
        /// 只显示OK按钮
        /// </summary>
        /// <returns></returns>
        public MessageBox OnlyOK()
        {
            if (isShowing)
            {
                _btn.SetSelectedIndex(1);
            }
            return this;
        }

        /// <summary>
        /// 只显示取消按钮
        /// </summary>
        /// <returns></returns>
        public MessageBox OnlyCancel()
        {
            if (isShowing)
            {
                _btn.SetSelectedIndex(2);
            }
            return this;
        }

        /// <summary>
        /// 隐藏关闭按钮
        /// </summary>
        /// <returns></returns>
        public MessageBox HideClose()
        {
            if (isShowing)
            {
                _closeBtn.visible = false;
            }
            return this;
        }

        private void Init(string title, string content, string ok, string cancel)
        {
            PackageAgents.LoadFairyResource("RP_MessageBox", () =>
            {
                _window = new Window();
                _window.MakeFullScreen();
                _window.contentPane = UIPackage.CreateObject("RP_MessageBox", "MessageBox").asCom;
                _window.contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
                _window.sortingOrder = (int)UFramework.UI.Layer.MESSAGE_BOX;
                _window.Show();

                var center = _window.contentPane.GetChild("_center").asCom;
                _title = center.GetChild("_title").asTextField;
                _content = center.GetChild("_content").asRichTextField;
                _okBtn = center.GetChild("_ok").asButton;
                _cancelBtn = center.GetChild("_cancel").asButton;
                _closeBtn = center.GetChild("_close").asButton;
                _closeBtn.visible = true;

                _btn = center.GetController("_btn");
                _btn.SetSelectedIndex(0);

                _title.text = title;
                _content.text = content;
                _okBtn.text = ok;
                _cancelBtn.text = cancel;

                _okBtn.onClick.Set(OnOK);
                _cancelBtn.onClick.Set(OnCancel);
                _closeBtn.onClick.Set(OnClose);

                showeds.Add(this);
            });
        }

        private void OnOK()
        {
            HandleEvent(true);
        }

        private void OnCancel()
        {
            HandleEvent(false);
        }

        private void OnClose()
        {
            if (_btn.selectedIndex == 0 || _btn.selectedIndex == 2)
            {
                HandleEvent(false);
            }
            else
            {
                HandleEvent(true);
            }
        }

        private void HandleEvent(bool et)
        {
            onCompleted.InvokeGracefully(et);
            isOK = et;
            Hide();
        }
    }
}