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
        
        public static void SetContainer(ManagerContainer mc)
        {
            managerContainer = mc;
        }
        
        protected LuaManager _luaManager;
        protected NetworkManager _networkManager;
        protected ResManager _resManager;
        protected AdapterManager _adapterManager;

        public BaseBehaviour()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            OnInitialize();
        }
        
        public virtual void SetReference()
        {
            _luaManager = managerContainer.GetManager<LuaManager>();
            _networkManager = managerContainer.GetManager<NetworkManager>();
            _resManager = managerContainer.GetManager<ResManager>();
            _adapterManager= managerContainer.GetManager<AdapterManager>();
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
                var ctor = luaTable.GetLuaFunction(funcName);
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