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
    [System.Serializable]
    public class ConfigItem
    {
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Config Name")]
        public string name;

        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Address")]
        public FileAddress address;
    }

    public class ConfigPage : IPage, IPageBar
    {
        public string menuName { get { return "Config"; } }

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true)]
        [LabelText("Editor")]
        public List<ConfigItem> editors = new List<ConfigItem>();

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true)]
        [LabelText("Runtime")]
        public List<ConfigItem> runtimes = new List<ConfigItem>();

        private Dictionary<string, FileAddress> editorAddressDictionary = new Dictionary<string, FileAddress>();
        private Dictionary<string, FileAddress> runtimeAddressDictionary = new Dictionary<string, FileAddress>();

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

            editors.Clear();
            foreach (KeyValuePair<string, FileAddress> item in editorAddressDictionary)
            {
                var cItem = new ConfigItem();
                cItem.name = item.Key;
                cItem.address = item.Value;

                editors.Add(cItem);
            }

            runtimes.Clear();
            foreach (KeyValuePair<string, FileAddress> item in runtimeAddressDictionary)
            {
                var cItem = new ConfigItem();
                cItem.name = item.Key;
                cItem.address = item.Value;

                runtimes.Add(cItem);
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