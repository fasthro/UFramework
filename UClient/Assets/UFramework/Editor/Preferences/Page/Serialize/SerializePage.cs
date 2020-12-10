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
using UFramework.Serialize;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class SerializePage : IPage, IPageBar
    {
        public string menuName { get { return "Serdata(序列化)"; } }

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true)]
        [LabelText("Editor")]
        public List<SerializeItem> editors = new List<SerializeItem>();

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true)]
        [LabelText("Runtime")]
        public List<SerializeItem> runtimes = new List<SerializeItem>();

        private Dictionary<string, SerializableType> _editorAddressDict = new Dictionary<string, SerializableType>();
        private Dictionary<string, SerializableType> _runtimeAddressDict = new Dictionary<string, SerializableType>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            _editorAddressDict.Clear();
            _runtimeAddressDict.Clear();

            Type[] types = Assembly.Load("Assembly-CSharp-Editor").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.GetInterface("ISerializable") != null)
                {
                    var name = type.Name;
                    if (name.EndsWith("Serdata") && !_editorAddressDict.ContainsKey(name))
                    {
                        var ctors = type.GetConstructors();
                        var st = (SerializableType)type.GetProperty("serializableType").GetValue(ctors[0].Invoke(null));
                        if (st == SerializableType.Editor)
                        {
                            _editorAddressDict.Add(name, st);
                        }
                        else if (st != SerializableType.User)
                        {
                            _runtimeAddressDict.Add(name, st);
                        }
                    }
                }
            }

            types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (types[i].GetInterface("ISerializable") != null)
                {
                    var name = types[i].Name;
                    if (name.EndsWith("Serdata") && !_runtimeAddressDict.ContainsKey(name))
                    {
                        Debug.Log(name);
                        var ctors = type.GetConstructors();
                        var st = (SerializableType)type.GetProperty("serializableType").GetValue(ctors[0].Invoke(null));
                        if (st != SerializableType.Editor && st != SerializableType.User)
                        {
                            _runtimeAddressDict.Add(name, st);
                        }
                        else
                        {
                            _editorAddressDict.Add(name, st);
                        }
                    }
                }
            }

            editors.Clear();
            foreach (KeyValuePair<string, SerializableType> item in _editorAddressDict)
            {
                var cItem = new SerializeItem();
                cItem.name = item.Key;
                cItem.serializableType = item.Value;

                editors.Add(cItem);
            }

            runtimes.Clear();
            foreach (KeyValuePair<string, SerializableType> item in _runtimeAddressDict)
            {
                var cItem = new SerializeItem();
                cItem.name = item.Key;
                cItem.serializableType = item.Value;

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