/*
 * @Author: fasthro
 * @Date: 2020-05-26 23:07:50
 * @Description: UConfig
 */
using System;
using System.Collections.Generic;
using LitJson;
using UFramework.Assets;
using UnityEngine;

namespace UFramework.Config
{
    /// <summary>
    /// 配置文件移动设备上所在路径
    /// </summary>
    public enum FileAddress
    {
        /// <summary>
        /// 编辑器目录
        /// </summary>
        Editor,
        /// <summary>
        /// 持久化数据目录
        /// </summary>
        Data,
        /// <summary>
        /// Resources 目录
        /// </summary>
        Resources,
    }

    public class UConfig
    {
        // 配置缓存字字典
        static Dictionary<string, IConfigObject> configDictionart = new Dictionary<string, IConfigObject>();

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Read<T>() where T : IConfigObject, new()
        {
            string configName = typeof(T).Name;

            IConfigObject config;
            if (configDictionart.TryGetValue(configName, out config))
            {
                return (T)config;
            }

            config = new T();
            var type = config.GetType();
            FileAddress address = (FileAddress)type.GetProperty("address").GetValue(config, null);

            string content = null;
            string fileName = configName + ".json";
#if UNITY_EDITOR
            content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigDirectory, address.ToString(), fileName));
#else
            if (address == FileAddress.Resources)
            {
                var asset = Asset.LoadResourceAsset(IOPath.PathCombine(App.ConfigResourceDirectory, address.ToString(), fileName), typeof(TextAsset));
                content = asset.GetAsset<TextAsset>().text;
                asset.Unload();
            }
            else
            {
                var asset = Asset.LoadAsset(IOPath.PathCombine(App.ConfigDataDirectory, address.ToString(), fileName), typeof(TextAsset));
                content = asset.GetAsset<TextAsset>().text;
                asset.Unload();
            }
#endif
            if (!string.IsNullOrEmpty(content))
            {
                config = JsonMapper.ToObject<T>(content);
            }
            configDictionart.Add(configName, config);
            return (T)config;
        }

        /// <summary>
        /// 写入配置
        /// 在编辑器配置下自动写入UAssets/Config
        /// 在移动设备上，自动写入到持久化数据目录
        /// </summary>
        /// <param name="config"></param>
        public static void Write<T>(object config) where T : IConfigObject, new()
        {
#if UNITY_EDITOR
            Type type = typeof(T);
            FileAddress address = (FileAddress)type.GetProperty("address").GetValue(config, null);
            string configName = typeof(T).Name;

            if (configDictionart.ContainsKey(configName))
            {
                configDictionart[configName] = (T)config;
            }

            string fileName = configName + ".json";
            string content = JsonMapper.ToJson(config);
            string path = IOPath.PathCombine(App.ConfigDirectory, address.ToString(), fileName);
            IOPath.FileCreateText(path, content);
#endif
        }
    }
}