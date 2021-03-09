// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/09 15:01
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UFramework.Consoles
{
    public class CommandService : BaseConsoleService
    {
        private Dictionary<string, Command> _cmdDict = new Dictionary<string, Command>();

        /// <summary>
        /// 收集所有命令
        /// </summary>
        public Command[] CollectCommands()
        {
            _cmdDict.Clear();

            var assembly = Assembly.Load("Assembly-CSharp");
            var types = assembly.GetTypes();
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

            foreach (var type in types)
            {
                var methods = type.GetMethods(flags);
                foreach (var method in methods)
                {
                    var atr = method.GetCustomAttribute(typeof(MethodCommandAttribute), false);
                    if (atr != null)
                    {
                        if (method.IsStatic)
                            AddStaticMethodCommand(method, atr as MethodCommandAttribute);
                    }
                }
            }

            return _cmdDict.Values.ToArray();
        }

        /// <summary>
        /// 添加静态方法命令
        /// </summary>
        /// <param name="info"></param>
        /// <param name="attr"></param>
        private void AddStaticMethodCommand(MethodInfo info, MethodCommandAttribute attr)
        {
            if (_cmdDict.ContainsKey(attr.name))
            {
                Debug.LogError("命令已经存在. [" + attr.name + "]");
            }
            else
            {
                _cmdDict.Add(attr.name, new StaticMethodCommand(attr.name, attr.description, info));
            }
        }

        /// <summary>
        /// 添加静态方法命令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="methodName"></param>
        /// <param name="type"></param>
        public void AddStaticMethodCommand(string name, string description, string methodName, Type type)
        {
            if (_cmdDict.ContainsKey(name))
            {
                Debug.LogError("命令已经存在. [" + name + "]");
            }
            else
            {
                var info = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (info != null)
                {
                    _cmdDict.Add(name, new StaticMethodCommand(name, description, info));
                }
            }
        }

        /// <summary>
        /// 添加方法命令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="methodName"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        public void AddMethodCommand(string name, string description, string methodName, Type type, object target)
        {
            if (target == null)
                return;

            if (_cmdDict.ContainsKey(name))
            {
                Debug.LogError("命令已经存在. [" + name + "]");
            }
            else
            {
                var info = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (info != null)
                {
                    _cmdDict.Add(name, new MethodCommand(name, description, info, target));
                }
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="line"></param>
        public void ExecuteCommand(string line)
        {
            var words = line.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0)
            {
                _cmdDict.TryGetValue(words[0], out var command);
                command?.Execute(line);
            }
        }

        [MethodCommandAttribute("TestStaticCommand", "Print Static Command.", "Hello")]
        public static void TestStaticCommand(string arg)
        {
            Debug.Log("Hello Static Command > " + arg);
        }
    }
}