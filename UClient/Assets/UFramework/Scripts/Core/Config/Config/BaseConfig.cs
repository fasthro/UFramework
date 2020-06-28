/*
 * @Author: fasthro
 * @Date: 2020-05-31 22:14:16
 * @Description: Config 配置数据
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UFramework.Config
{
    public class BaseConfig : IConfigObject
    {
        public string name { get; }

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true)]
        [InfoBox("Setting Config File Address")]
        public Dictionary<string, FileAddress> addressDictionary = new Dictionary<string, FileAddress>();

        public object OdinReaded()
        {
            addressDictionary = new Dictionary<string, FileAddress>();
            return this;
        }

        [Button(ButtonSizes.Large)]
        public void Save()
        {
            UConfig.Write<BaseConfig>(this);
        }
    }
}