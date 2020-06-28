/*
 * @Author: fasthro
 * @Date: 2020-06-28 00:12:24
 * @Description: 资源池
 */
using System.Collections.Generic;

namespace UFramework.ResLoader
{
    public class ResPool
    {
        static Dictionary<string, Res> resDictionary = new Dictionary<string, Res>();

        /// <summary>
        /// 分配资源
        /// </summary>
        /// <param name="resName"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        public static T Allocate<T>(string resName, ResType resType) where T : Res
        {
            Res res = null;
            if (!resDictionary.TryGetValue(resName, out res))
            {
                if (resType == ResType.AssetBundle)
                {
                    res = BundleRes.Allocate(resName);
                }
                else if (resType == ResType.AssetBundleAsset)
                {
                    res = BundleAssetRes.Allocate(resName);
                }
                else if (resType == ResType.Resource)
                {
                    res = ResourceAssetRes.Allocate(resName);
                }
                resDictionary.Add(resName, res);
            }
            res.Retain();
            return res as T;
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        /// <param name="resName"></param>
        public static void Recycle(string resName)
        {
            if (resDictionary.ContainsKey(resName))
            {
                resDictionary.Remove(resName);
            }
        }
    }
}