/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:36:31
 * @Description: panel base
 */
using System.Collections.Generic;
using UFramework.Messenger;
using UnityEngine;

namespace UFramework.UI
{
    public abstract class UIPanel
    {
        #region public

        /// <summary>
        /// 层级
        /// </summary>
        public Layer layer { get; protected set; }

        /// <summary>
        /// 已经显示
        /// </summary>
        /// <value></value>
        public bool isShowed { get; protected set; }

        /// <summary>
        /// 加载完成
        /// </summary>
        /// <value></value>
        public bool isLoaded { get; protected set; }

        /// <summary>
        /// 加载中
        /// </summary>
        /// <value></value>
        public bool isLoading { get; protected set; }

        /// <summary>
        /// 已经隐藏
        /// </summary>
        /// <value></value>
        public bool isHidden { get; protected set; }

        #endregion

        #region protected

        // 包列表
        protected HashSet<string> packages = new HashSet<string>();

        #endregion

        #region private

        private int _packageCount;
        private bool _inited;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        public UIPanel(Layer layer)
        {
            this.layer = layer;
        }

        /// <summary>
        /// 添加包
        /// </summary>
        /// <param name="packageName"></param>
        public void AddPackage(string packageName)
        {
            if (!packages.Contains(packageName))
                packages.Add(packageName);
        }

        public void Show()
        {
            if (isShowed) return;
            if (!isLoaded && !isLoading)
            {
                isLoading = true;
                _packageCount = packages.Count;
                foreach (var package in packages)
                {
                    PackageAgents.Load(package, OnPackageLoaded);
                }
            }
            else if (isLoaded)
            {
                Init();
                DoShowAnimation();
            }
        }

        private void OnPackageLoaded()
        {
            _packageCount--;
            if (_packageCount <= 0)
            {
                isLoaded = true;
                isLoading = false;
                Init();
                DoShowAnimation();
            }
        }

        protected virtual void OnShow()
        {
            isShowed = true;

            UMessenger.AddListener<int>(Global.EVENT_UI_NOTIFICATION, OnNotification);
            UMessenger.AddListener(Global.EVENT_UI_NET_MESSAGE, OnNetMessage);
        }

        private void Init()
        {
            if (!_inited) OnInit();
        }

        protected virtual void OnInit()
        {
            _inited = true;
        }

        private void DoShowAnimation()
        {
            OnShowAnimation();
            OnShow();
        }

        protected virtual void OnShowAnimation()
        {

        }

        public void Hide()
        {
            OnHide();
        }

        protected virtual void OnHide()
        {
            UMessenger.RemoveListener<int>(Global.EVENT_UI_NOTIFICATION, OnNotification);
            UMessenger.RemoveListener(Global.EVENT_UI_NET_MESSAGE, OnNetMessage);
        }

        public void SendNotification(int id)
        {
            UMessenger.Broadcast<int>(Global.EVENT_UI_NOTIFICATION, id);
        }

        protected virtual void OnNotification(int id)
        {

        }

        protected virtual void OnNetMessage()
        {

        }

        /// <summary>
        /// 更新Panel排序
        /// </summary>
        /// <param name="order"></param>
        public abstract void UpdateSortOrder(int order);
    }
}