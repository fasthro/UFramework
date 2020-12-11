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
    public abstract class BaseManager : ILifeCycle
    {
        public BaseManager()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnApplicationQuit()
        {

        }

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
    }
}