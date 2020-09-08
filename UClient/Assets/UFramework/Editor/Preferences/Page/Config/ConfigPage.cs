/*
 * @Author: fasthro
 * @Date: 2020-06-29 08:35:09
 * @Description: Config Page
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class ConfigPage : IPage, IPageBar
    {
        public string menuName { get { return "Config"; } }

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true, KeyLabel = "Name", ValueLabel = "Address")]
        [ReadOnly]
        [LabelText("Editor")]
        public Dictionary<string, FileAddress> editorAddressDictionary = new Dictionary<string, FileAddress>();

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true, KeyLabel = "Name", ValueLabel = "Address")]
        [ReadOnly]
        [LabelText("Runtime")]
        public Dictionary<string, FileAddress> runtimeAddressDictionary = new Dictionary<string, FileAddress>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            editorAddressDictionary.Clear();
            runtimeAddressDictionary.Clear();

            Type[] types = Assembly.Load("Assembly-CSharp-Editor").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.GetInterface("IConfigObject") != null)
                {
                    var configName = type.Name;
                    if (!editorAddressDictionary.ContainsKey(configName))
                    {
                        var ctors = type.GetConstructors();
                        var address = (FileAddress)type.GetProperty("address").GetValue(ctors[0].Invoke(null));
                        if (address == FileAddress.Editor)
                        {
                            editorAddressDictionary.Add(configName, address);
                        }
                        else
                        {
                            runtimeAddressDictionary.Add(configName, address);
                        }
                    }
                }
            }

            types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (types[i].GetInterface("IConfigObject") != null)
                {
                    var configName = types[i].Name;
                    if (!runtimeAddressDictionary.ContainsKey(configName))
                    {
                        var ctors = type.GetConstructors();
                        var address = (FileAddress)type.GetProperty("address").GetValue(ctors[0].Invoke(null));
                        if (address != FileAddress.Editor)
                        {
                            runtimeAddressDictionary.Add(configName, address);
                        }
                        else
                        {
                            editorAddressDictionary.Add(configName, address);
                        }
                    }
                }
            }
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {

        }
    }
}