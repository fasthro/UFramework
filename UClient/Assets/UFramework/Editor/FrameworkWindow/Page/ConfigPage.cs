/*
 * @Author: fasthro
 * @Date: 2020-06-29 08:35:09
 * @Description: Config Page
 */
using System;
using System.Collections.Generic;
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
            bool hasNew = false;
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].GetInterface("IConfigObject") != null)
                {
                    var configName = types[i].Name;
                    if (!instance.addressDictionary.ContainsKey(configName))
                    {
                        hasNew = true;
                        instance.addressDictionary.Add(configName, FileAddress.Editor);
                    }
                }
            }
            if (hasNew)
            {
                instance.Save();
            }
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }
    }
}