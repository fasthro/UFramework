/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:38:01
 * @Description:
 */

using LuaInterface;
using System;

namespace UFramework
{
    public abstract class BaseBehaviour : IBehaviour
    {
        public static ManagerContainer managerContainer;

        public BaseBehaviour()
        {
            Initialize();
        }

        public static void SetContainer(ManagerContainer mc)
        {
            managerContainer = mc;
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public void LateUpdate()
        {
            OnLateUpdate();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

        public void Dispose()
        {
            OnDispose();
        }

        public void ApplicationQuit()
        {
            OnApplicationQuit();
        }

        public virtual void SetReference()
        {
            _luaManager = managerContainer.GetManager<LuaManager>();
            _networkManager = managerContainer.GetManager<NetworkManager>();
            _resManager = managerContainer.GetManager<ResManager>();
        }

        protected LuaManager _luaManager;

        protected NetworkManager _networkManager;

        protected ResManager _resManager;

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        protected virtual void OnLateUpdate()
        {
        }

        protected virtual void OnFixedUpdate()
        {
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnApplicationQuit()
        {
        }

        #region lua

        public LuaTable luaTable { get; set; }

        public bool CallLuaFunction(string funcName, params object[] args)
        {
            if (luaTable != null)
            {
                LuaFunction ctor = luaTable.GetLuaFunction(funcName);
                if (ctor != null)
                {
                    try
                    {
                        if (args.Length == 0)
                            ctor.Call(luaTable);
                        else if (args.Length == 1)
                            ctor.Call(luaTable, args[0]);
                        else if (args.Length == 2)
                            ctor.Call(luaTable, args[0], args[1]);
                        else if (args.Length == 3)
                            ctor.Call(luaTable, args[0], args[1], args[2]);
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

        #endregion lua
    }
}