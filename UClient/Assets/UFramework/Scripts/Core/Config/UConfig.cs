/*
 * @Author: fasthro
 * @Date: 2020-05-26 23:07:50
 * @Description: UConfig
 */
using System.Collections.Generic;
using LitJson;
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
        /// Resource目录
        /// </summary>
        Resource,
    }

    public class UConfig
    {
        // 配置缓存字字典
        static Dictionary<string, IConfigObject> configDictionart = new Dictionary<string, IConfigObject>();
        // 基础配置
        static BaseConfig baseConfig;

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Read<T>() where T : IConfigObject, new()
        {
            ReadBaseConfig();

            string configName = typeof(T).Name;

            IConfigObject config;
            if (configDictionart.TryGetValue(configName, out config))
            {
                return (T)config;
            }

            string content;
            string fileName = configName + ".json";
#if UNITY_EDITOR
            content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigDirectory, fileName));
#else
            if (baseConfig.addressDictionary.ContainsKey(configName))
            {
                if (baseConfig.addressDictionary[configName] == FileAddress.Resource)
                {
                    content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigResourceDirectory, fileName));
                }
                else
                {
                    content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigDirectory, fileName));
                }
            }
            else
            {
                content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigDirectory, fileName));
            }
#endif
            if (!string.IsNullOrEmpty(content))
            {
                config = JsonMapper.ToObject<T>(content);
            }
            else
            {
                config = new T();
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
            string configName = typeof(T).Name;

            if (configDictionart.ContainsKey(configName))
            {
                configDictionart[configName] = (T)config;
            }

            string fileName = configName + ".json";
            string content = JsonMapper.ToJson(config);
            string path = IOPath.PathCombine(App.ConfigDirectory, fileName);
            IOPath.FileCreateText(path, content);
#endif
        }

        /// <summary>
        /// 读取基础配置
        /// </summary>
        private static void ReadBaseConfig()
        {
            if (baseConfig == null)
            {
                string content;
#if UNITY_EDITOR
                content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigDirectory, "BaseConfig.json"));
#else
                content = IOPath.FileReadText(IOPath.PathCombine(App.ConfigResourceDirectory, "BaseConfig.json"));
#endif
                if (!string.IsNullOrEmpty(content))
                {
                    baseConfig = JsonMapper.ToObject<BaseConfig>(content);
                }
                else
                {
                    baseConfig = new BaseConfig();
                }
            }
        }
    }
}