// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-04-07 23:28
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Helper
{
    public static class EditorCompileComplete
    {
        private static readonly string PrefKey = "EditorCompileComplete-Methods";

        public static void Rsegister<T>(string methodName, string arg = null)
        {
            var type = typeof(T);
            AppendPref($"{type.FullName}.{methodName}({arg})");
        }

        static void AppendPref(string value)
        {
            SetPref(GetMethodsValue() + ";" + value);
        }

        static void SetPref(string value)
        {
            EditorPrefs.SetString(PrefKey, value);
        }

        static string GetMethodsValue()
        {
            return EditorPrefs.HasKey(PrefKey) ? EditorPrefs.GetString(PrefKey) : "";
        }

        static string[] GetMethods()
        {
            return GetMethodsValue().Split(';').Where(value => value.Trim() != "").ToArray();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            var methods = GetMethods();
            foreach (var method in methods)
            {
                var pointIndex = method.LastIndexOf(".", StringComparison.Ordinal);
                var left = method.LastIndexOf("(", StringComparison.Ordinal);
                var right = method.LastIndexOf(")", StringComparison.Ordinal);
                string className = method.Substring(0, pointIndex);
                string methodName = method.Substring(pointIndex + 1, left - pointIndex - 1);
                string argName = method.Substring(left + 1, right - left - 1);
                Type type = Type.GetType(className);
                if (type == null)
                    continue;

                var method1 = type.GetMethod(methodName,
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { }, null);
                var method2 = type.GetMethod(methodName,
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null,
                    new Type[] {typeof(string)}, null);
                if (method1 == null && method2 == null)
                    continue;

                if (method1 != null && argName.Trim() == "")
                    method1.Invoke(null, null);
                if (method2 != null && argName.Trim() != "")
                    method2.Invoke(null, new object[] {argName});
            }

            SetPref("");
        }
    }
}