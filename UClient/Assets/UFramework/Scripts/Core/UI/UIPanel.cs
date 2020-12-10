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
        protected HashSet<string> _packages = new HashSet<string>();

        #endregion

        #region private

        private int _packageCount;
        private bool _inited;
        private LuaTable _lua;
        
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
            if (!_packages.Contains(packageName))
                _packages.Add(packageName);
        }

        public void Show()
        {
            if (isShowed) return;
            if (!isLoaded && !isLoading)
            {
                isLoading = true;
                _packageCount = _packages.Count;
                foreach (var package in _packages)
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
            LuaCall("onHide");
        }

        public void LuaBind(LuaTable lua)
        {
            _lua = lua;
        }

        [NoToLua]
        protected bool LuaCall(string funcName, params object[] args)
        {
            if (_lua != null)
            {
                LuaFunction ctor = _lua.GetLuaFunction(funcName);
                if (ctor != null)
                {
                    try
                    {
                        if (args.Length == 0)
                            ctor.Call(_lua);
                        else if (args.Length == 1)
                            ctor.Call(_lua, args[0]);
                        else if (args.Length == 2)
                            ctor.Call(_lua, args[0], args[1]);
                        else if (args.Length == 3)
                            ctor.Call(_lua, args[0], args[1], args[2]);
                    }
                    catch (Exception err)
                    {
                        Logger.Error(err);
                    }
                    ctor.Dispose();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新Panel排序
        /// </summary>
        /// <param name="order"></param>
        public abstract void UpdateSortOrder(int order);
    }
}