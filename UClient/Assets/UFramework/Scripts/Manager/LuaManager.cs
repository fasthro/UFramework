/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: LuaManager
 */
using System;
using LuaInterface;
using UFramework.Config;
using UFramework.Lua;
using UnityEngine;

namespace UFramework
{
    public class LuaManager : BaseManager
    {
        static LuaState lua;
        private LuaLooper m_loop = null;

        private LuaTable luaApp;
        private LuaFunction luaAppUpdateFunc;
        private LuaFunction luaAppLateUpdateFunc;
        private LuaFunction luaAppFixedUpdateFunc;

        protected override void OnInitialize()
        {
            lua = new LuaState();
            OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, AppLaunch.main);

            AddSearchPath();
            lua.Start();

            m_loop = AppLaunch.mainGameObject.AddComponent<LuaLooper>();
            m_loop.luaState = lua;

            DoFile("luaApp");
            luaApp = lua.GetTable("luaApp");
            luaAppUpdateFunc = luaApp.GetLuaFunction("update");
            luaAppLateUpdateFunc = luaApp.GetLuaFunction("lateUpdate");
            luaAppFixedUpdateFunc = luaApp.GetLuaFunction("fixedUpdate");
            luaApp.Call("start");
        }

        #region lib

        private void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
            lua.OpenLibs(LuaDLL.luaopen_struct);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);

            OpenCJson();
            OpenLuaSocket();
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;

            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.RegFunction("mime.core", LuaOpen_Mime_Core);
            lua.EndPreLoad();
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }

        #endregion

        private void AddSearchPath()
        {
            var appConfig = UConfig.Read<AppConfig>();
            if (appConfig.isDevelopmentVersion)
            {
                var luaConfig = UConfig.Read<LuaConfig>();
                if (luaConfig.searchPaths != null)
                {
                    for (int i = 0; i < luaConfig.searchPaths.Length; i++)
                    {
                        lua.AddSearchPath(IOPath.PathCombine(IOPath.PathParent(Application.dataPath), luaConfig.searchPaths[i]));
                    }
                }
            }
            else
            {
                lua.AddSearchPath(App.LuaDataDirectory);
            }
        }

        public void DoFile(string filename)
        {
            lua.DoFile(filename);
        }

        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        #region call function

        public static void Call(string funcName)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call();
        }

        public static void Call<T1>(string funcName, T1 arg1)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1);
        }

        public static void Call<T1, T2>(string funcName, T1 arg1, T2 arg2)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2);
        }

        public static void Call<T1, T2, T3>(string funcName, T1 arg1, T2 arg2, T3 arg3)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3);
        }

        public static void Call<T1, T2, T3, T4>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4);
        }

        public static void Call<T1, T2, T3, T4, T5>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5);
        }

        public static void Call<T1, T2, T3, T4, T5, T6>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7, T8>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static R1 Invoke<R1>(string funcName)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<R1>();
            return default(R1);
        }

        public static R1 Invoke<T1, R1>(string funcName, T1 arg1)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, R1>(arg1);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, R1>(string funcName, T1 arg1, T2 arg2)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, R1>(arg1, arg2);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, R1>(arg1, arg2, arg3);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, R1>(arg1, arg2, arg3, arg4);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, R1>(arg1, arg2, arg3, arg4, arg5);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, R1>(arg1, arg2, arg3, arg4, arg5, arg6);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return default(R1);
        }

        #endregion

        protected override void OnUpdate(float deltaTime)
        {
            luaAppUpdateFunc.Call();
        }

        protected override void OnLateUpdate()
        {
            luaAppLateUpdateFunc.Call();
        }

        protected override void OnFixedUpdate()
        {
            luaAppFixedUpdateFunc.Call();
        }

        protected override void OnDispose()
        {
            if (luaAppUpdateFunc != null)
            {
                luaAppUpdateFunc.Dispose();
                luaAppUpdateFunc = null;
            }

            if (luaAppLateUpdateFunc != null)
            {
                luaAppLateUpdateFunc.Dispose();
                luaAppLateUpdateFunc = null;
            }

            if (luaAppFixedUpdateFunc != null)
            {
                luaAppFixedUpdateFunc.Dispose();
                luaAppFixedUpdateFunc = null;
            }

            if (luaApp != null)
            {
                luaApp.Call("destory");

                luaApp.Dispose();
                luaApp = null;
            }

            if (m_loop != null)
            {
                m_loop.Destroy();
                m_loop = null;
            }

            if (lua != null)
            {
                lua.Dispose();
                lua = null;
            }
        }
    }
}