/*
 * @Author: fasthro
 * @Date: 2020-09-09 10:18:30
 * @Description: 资源 Manager（AssetsBundle/Resources）
 */

using System;
using UFramework.Assets;
using UFramework.Messenger;

namespace UFramework
{
    public class ResManager : BaseManager
    {
        protected override void OnInitialize() { }
        protected override void OnUpdate(float deltaTime) { }
        protected override void OnLateUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnDispose() { }

        /// <summary>
        /// bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AssetRequest LoadAssetAsync(string path, Type type, UCallback<AssetRequest> callback)
        {
            return Asset.LoadAssetAsync(path, type, callback);
        }

        /// <summary>
        /// bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetRequest LoadAsset(string path, Type type)
        {
            return Asset.LoadAsset(path, type);
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
            return Asset.LoadResourceAssetAsync(path, type, callback);
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetRequest LoadResourceAsset(string path, Type type)
        {
            return Asset.LoadResourceAsset(path, type);
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetRequest LoadWebAsset(string path, Type type, UCallback<AssetRequest> callback)
        {
            return Asset.LoadWebAsset(path, type, callback);
        }

        /// <summary>
        /// unload
        /// </summary>
        /// <param name="asset"></param>
        public void UnloadAsset(AssetRequest asset)
        {
            Asset.UnloadAsset(asset);
        }
    }
}