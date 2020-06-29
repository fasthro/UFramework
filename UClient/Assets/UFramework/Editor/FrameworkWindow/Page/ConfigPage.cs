/*
 * @Author: fasthro
 * @Date: 2020-06-29 08:35:09
 * @Description: Config Window
 */
using System;
using System.Reflection;
using UFramework.Config;
using UnityEngine;

namespace UFramework.FrameworkWindow
{
    public class ConfigPage : IPage
    {
        public string menuName { get { return "Config"; } }

        static BaseConfig instance;

        public object GetInstance()
        {
            instance = UConfig.Read<BaseConfig>();
            return instance;
        }

        public void OnRenderBefore()
        {
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsInstanceOfType(typeof(IConfigObject)))
                {
                    Debug.Log("ddddddd");
                }
            }
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }
    }
}