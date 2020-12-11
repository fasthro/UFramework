/*
 * @Author: fasthro
 * @Date: 2020-09-09 10:18:30
 * @Description: 资源 Manager（AssetsBundle/Resources）
 */

using System;
using UFramework.Core;

namespace UFramework
{
    public class ResManager : BaseManager
    {
        /// <summary>
        /// bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AssetRequest LoadAssetAsync(string path, Type type, UCallback<AssetRequest> callback)
        {
            return Assets.LoadAssetAsync(path, type, callback);
        }

        /// <summary>
        /// bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetRequest LoadAsset(string path, Type type)
        {
            return Assets.LoadAsset(path, type);
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AssetRequest LoadResourceAssetAsync(string path, Type type, UCallback<AssetRequest> callback)
        {
            return Assets.LoadResourceAssetAsync(path, type, callback);
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetRequest LoadResourceAsset(string path, Type type)
        {
            return Assets.LoadResourceAsset(path, type);
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetRequest LoadWebAsset(string path, Type type, UCallback<AssetRequest> callback)
        {
            return Assets.LoadWebAsset(path, type, callback);
        }

        /// <summary>
        /// unload
        /// </summary>
        /// <param name="asset"></param>
        public void UnloadAsset(AssetRequest asset)
        {
            Assets.UnloadAsset(asset);
        }
    }
}