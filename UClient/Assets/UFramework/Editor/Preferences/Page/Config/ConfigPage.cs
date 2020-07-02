/*
 * @Author: fasthro
 * @Date: 2020-06-29 08:35:09
 * @Description: Config Page
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class ConfigPage : IPage
    {
        public string menuName { get { return "Config"; } }
        static BaseConfig describeObject;

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true, KeyLabel = "Name", ValueLabel = "Address")]
        [InfoBox("配置文件地址设置")]
        public Dictionary<string, FileAddress> addressDictionary = new Dictionary<string, FileAddress>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<BaseConfig>();

            bool hasNew = false;
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].GetInterface("IConfigObject") != null)
                {
                    var configName = types[i].Name;
                    if (!describeObject.addressDictionary.ContainsKey(configName))
                    {
                        hasNew = true;
                        describeObject.addressDictionary.Add(configName, FileAddress.Editor);
                    }
                }
            }
            if (hasNew)
            {
                describeObject.Save();
            }
            addressDictionary = describeObject.addressDictionary;
        }

        public void OnDrawFunctoinButton()
        {

        }

        public void OnApply()
        {
            describeObject.addressDictionary = addressDictionary;
            describeObject.Save();
        }
    }
}