/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:36:31
 * @Description: panel base
 */
using System;
using System.Collections.Generic;
using LuaInterface;
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
        private LuaTable _peerTable;
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

            UMessenger.AddListener<int>(Global.Event_Panel, OnReceiveEvent);
            UMessenger.AddListener(Global.Event_Panel_Network, OnReceivePack);

            LuaCall("onShow");
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
            UMessenger.RemoveListener<int>(Global.Event_Panel, OnReceiveEvent);
            UMessenger.RemoveListener(Global.Event_Panel_Network, OnReceivePack);

            LuaCall("onHide");
        }

        public void LuaBind(LuaTable peerTable)
        {
            _peerTable = peerTable;
        }

        [NoToLua]
        public bool LuaCall(string funcName, params object[] args)
        {
            if (_peerTable != null)
            {
                LuaFunction ctor = _peerTable.GetLuaFunction(funcName);
                if (ctor != null)
                {
                    try
                    {
                        if (args.Length == 0)
                            ctor.Call(_peerTable);
                        else if (args.Length == 1)
                            ctor.Call(_peerTable, args[0]);
                        else if (args.Length == 2)
                            ctor.Call(_peerTable, args[0], args[1]);
                        else if (args.Length == 3)
                            ctor.Call(_peerTable, args[0], args[1], args[2]);
                    }
                    catch (Exception err)
                    {
                        Debug.LogError(err);
                    }
                    ctor.Dispose();
                    return true;
                }
            }
            return false;
        }

        public void BroadcastEvent(int id)
        {
            UMessenger.Broadcast<int>(Global.Event_Panel, id);
        }

        protected virtual void OnReceiveEvent(int id)
        {
            LuaCall("onReceiveEvent", id);
        }

        protected virtual void OnReceivePack()
        {
            LuaCall("onReceivePack");
        }

        /// <summary>
        /// 更新Panel排序
        /// </summary>
        /// <param name="order"></param>
        public abstract void UpdateSortOrder(int order);
    }
}