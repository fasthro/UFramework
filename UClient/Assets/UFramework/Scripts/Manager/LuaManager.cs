/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: LuaManager
 */
using System;
using System.IO;
using LuaInterface;
using UFramework.Core;
using UnityEngine;

namespace UFramework
{
    public class LuaManager : BaseManager
    {
        static LuaState Lua;

        public LuaTable luaEngine { get; private set; }

        private LuaFunction _luaEngineUpdateFunc;
        private LuaFunction _luaEngineLateUpdateFunc;
        private LuaFunction _luaEngineFixedUpdateFunc;

        private LuaLooper _loop;

        protected override void OnInitialize()
        {
            Lua = new LuaState();
            OpenLibs();
            Lua.LuaSetTop(0);

            LuaBinder.Bind(Lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(Lua, Launcher.Main);

            AddSearchPath();
            Lua.Start();

            _loop = Launcher.MainGameObject.AddComponent<LuaLooper>();
            _loop.luaState = Lua;
        }

        public void LaunchEngine()
        {
            DoFile("LuaEngine");
            luaEngine = Lua.GetTable("LuaEngine");
            _luaEngineUpdateFunc = luaEngine.GetLuaFunction("update");
            _luaEngineLateUpdateFunc = luaEngine.GetLuaFunction("lateUpdate");
            _luaEngineFixedUpdateFunc = luaEngine.GetLuaFunction("fixedUpdate");
            luaEngine.Call("initialize", Logger.GetLevel());
            luaEngine.Call("launch");
        }

        #region lib

        private void OpenLibs()
        {
            //保持库名字与5.1.5库中一致
            Lua.BeginPreLoad();
            Lua.AddPreLoadLib("pb2", new LuaCSFunction(LuaDLL.luaopen_pb));
            Lua.AddPreLoadLib("struct", new LuaCSFunction(LuaDLL.luaopen_struct));
            Lua.AddPreLoadLib("lpeg", new LuaCSFunction(LuaDLL.luaopen_lpeg));
            Lua.AddPreLoadLib("cjson", new LuaCSFunction(LuaDLL.luaopen_cjson));
            Lua.AddPreLoadLib("cjson.safe", new LuaCSFunction(LuaDLL.luaopen_cjson_safe));
            Lua.AddPreLoadLib("crypt", new LuaCSFunction(LuaDLL.luaopen_crypt));
            Lua.AddPreLoadLib("protobuf.c", new LuaCSFunction(LuaDLL.luaopen_protobuf_c));
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !LUAC_5_3
            lua.AddPreLoadLib("bit", new LuaCSFunction(LuaDLL.luaopen_bit));
#endif

            if (LuaConst.openLuaSocket || LuaConst.openLuaDebugger)
            {
                OpenLuaSocket();
            }
            Lua.EndPreLoad();
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            Lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            Lua.OpenLibs(LuaDLL.luaopen_cjson);
            Lua.LuaSetField(-2, "cjson");

            Lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            Lua.LuaSetField(-2, "cjson.safe");
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;
            Lua.AddPreLoadLib("socket.core", new LuaCSFunction(LuaDLL.luaopen_socket_core));
            Lua.AddPreLoadLib("mime.core", new LuaCSFunction(LuaDLL.luaopen_mime_core));
        }

        #endregion

        private void AddSearchPath()
        {
            if (Core.Serializer<AppConfig>.Instance.isDevelopmentVersion)
            {
                var serdata = Core.Serializer<LuaConfig>.Instance;
                if (serdata.searchPaths != null)
                {
                    for (int i = 0; i < serdata.searchPaths.Length; i++)
                    {
                        Lua.AddSearchPath(IOPath.PathCombine(Environment.CurrentDirectory, serdata.searchPaths[i]));
                    }
                }
            }
            else
            {
                var dirs = Directory.GetDirectories(IOPath.PathCombine(Application.persistentDataPath, "Lua"));
                for (int i = 0; i < dirs.Length; i++)
                {
                    Lua.AddSearchPath(dirs[i]);
                }
            }
        }

        public void DoFile(string filename)
        {
            Lua.DoFile(filename);
        }

        public void LuaGC()
        {
            Lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        #region call function

        public static void Call(string funcName)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call();
        }

        public static void Call<T1>(string funcName, T1 arg1)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1);
        }

        public static void Call<T1, T2>(string funcName, T1 arg1, T2 arg2)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2);
        }

        public static void Call<T1, T2, T3>(string funcName, T1 arg1, T2 arg2, T3 arg3)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3);
        }

        public static void Call<T1, T2, T3, T4>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4);
        }

        public static void Call<T1, T2, T3, T4, T5>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5);
        }

        public static void Call<T1, T2, T3, T4, T5, T6>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7, T8>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static R1 Invoke<R1>(string funcName)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<R1>();
            return default(R1);
        }

        public static R1 Invoke<T1, R1>(string funcName, T1 arg1)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, R1>(arg1);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, R1>(string funcName, T1 arg1, T2 arg2)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, R1>(arg1, arg2);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, R1>(arg1, arg2, arg3);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, R1>(arg1, arg2, arg3, arg4);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, R1>(arg1, arg2, arg3, arg4, arg5);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, R1>(arg1, arg2, arg3, arg4, arg5, arg6);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            LuaFunction func = Lua.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return default(R1);
        }

        #endregion

        protected override void OnUpdate(float deltaTime)
        {
            _luaEngineUpdateFunc.Call();
        }

        protected override void OnLateUpdate()
        {
            _luaEngineLateUpdateFunc.Call();
        }

        protected override void OnFixedUpdate()
        {
            _luaEngineFixedUpdateFunc.Call();
        }

        protected override void OnDispose()
        {
            if (_luaEngineUpdateFunc != null)
            {
                _luaEngineUpdateFunc.Dispose();
                _luaEngineUpdateFunc = null;
            }

            if (_luaEngineLateUpdateFunc != null)
            {
                _luaEngineLateUpdateFunc.Dispose();
                _luaEngineLateUpdateFunc = null;
            }

            if (_luaEngineFixedUpdateFunc != null)
            {
                _luaEngineFixedUpdateFunc.Dispose();
                _luaEngineFixedUpdateFunc = null;
            }

            if (luaEngine != null)
            {
                luaEngine.Call("destory");

                luaEngine.Dispose();
                luaEngine = null;
            }

            if (_loop != null)
            {
                _loop.Destroy();
                _loop = null;
            }

            if (Lua != null)
            {
                Lua.Dispose();
                Lua = null;
            }
        }
    }
}