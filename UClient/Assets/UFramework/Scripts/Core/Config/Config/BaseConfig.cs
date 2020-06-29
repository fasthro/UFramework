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
        public string name { get { return "BaseConfig"; } }

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.OneLine)]
        [InfoBox("配置文件地址设置")]
        public Dictionary<string, FileAddress> addressDictionary = new Dictionary<string, FileAddress>();
        
        /// <summary>
        /// Save
        /// </summary>
        [Button(ButtonSizes.Large, Name = "Apply")]
        public void Save()
        {
            UConfig.Write<BaseConfig>(this);
        }
    }
}