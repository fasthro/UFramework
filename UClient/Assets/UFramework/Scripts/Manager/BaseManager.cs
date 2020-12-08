/*
 * @Author: fasthro
 * @Date: 2020-05-25 23:52:22
 * @Description: BaseManager
 */

using System;
using LuaInterface;
using UnityEngine;

namespace UFramework
{
    public abstract class BaseManager
    {
        public void Initialize()
        {
            OnInitialize();
        }

        protected abstract void OnInitialize();

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);

        public void LateUpdate()
        {
            OnLateUpdate();
        }

        protected abstract void OnLateUpdate();

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

        protected abstract void OnFixedUpdate();

        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnDispose();

        #region lua

        protected LuaTable _lua;

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

        #endregion

        protected virtual void Log(object message)
        {
            Logger.Debug(message);
        }

        protected virtual void LogError(object message)
        {
            Logger.Error(message);
        }
    }
}