/*
 * @Author: fasthro
 * @Date: 2020-07-01 15:14:43
 * @Description: 资源映射配置
 */

using System.Collections.Generic;
using UFramework.Config;

namespace UFramework.ResLoader
{
    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResAssetInfo
    {
        /// <summary>
        /// bundle name
        /// </summary>
        public string assetBundleName;

        /// <summary>
        /// size
        /// </summary>
        public long size;

        /// <summary>
        /// md5
        /// </summary>
        public string md5;
    }

    /// <summary>
    /// 资源信息配置
    /// </summary>
    public class ResLoaderConfig : IConfigObject
    {
        public string name { get { return "ResConfig"; } }
        public FileAddress address { get { return FileAddress.Data; } }

        /// <summary>
        /// 资源映射关系
        /// </summary>
        /// <typeparam name="string">资源</typeparam>
        /// <typeparam name="string">所属bundle</typeparam>
        /// <returns></returns>
        public Dictionary<string, ResAssetInfo> assetInfoDictionary = new Dictionary<string, ResAssetInfo>();

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResAssetInfo GetAssetInfo(string key)
        {
            ResAssetInfo info = null;
            if (assetInfoDictionary.TryGetValue(key, out info))
            {
                return info;
            }
            return null;
        }

        public void Save()
        {
            UConfig.Write<ResLoaderConfig>(this);
        }
    }
}