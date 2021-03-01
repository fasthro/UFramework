// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-17 15:33:31
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UFramework;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Core
{
    [MonoSingletonPath("UFramework/Assets")]
    public class Assets : MonoSingleton<Assets>
    {
        /// <summary>
        /// AssetBundle 后缀
        /// </summary>
        public static readonly string Extension = ".unity3d";

        /// <summary>
        /// 开发模式
        /// </summary>
        /// <value></value>
        public static bool isDevelop { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public static bool isBuildNameHash { get; private set; }

        /// <summary>
        /// AssetBundlePath
        /// </summary>
        private static string AssetBundlePath;

        /// <summary>
        /// asset to bundle mapping
        /// </summary>
        /// <typeparam name="string">asset</typeparam>
        /// <typeparam name="string">bundle</typeparam>
        /// <returns></returns>
        private static Dictionary<string, string> Asset2BundleDict = new Dictionary<string, string>();

        /// <summary>
        /// bundle to dependencies mapping
        /// </summary>
        /// <typeparam name="string">bundle</typeparam>
        /// <typeparam name="string[]">dependencies bundle</typeparam>
        /// <returns></returns>
        private static Dictionary<string, string[]> Bundle2DependencieDict = new Dictionary<string, string[]>();

        /// <summary>
        /// assets
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="AssetObject"></typeparam>
        /// <returns></returns>
        private Dictionary<string, AssetRequest> _assetDict = new Dictionary<string, AssetRequest>();

        /// <summary>
        /// bundles
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="BundleRequest"></typeparam>
        /// <returns></returns>
        private Dictionary<string, AssetRequest> _bundleDict = new Dictionary<string, AssetRequest>();

        private Action<bool> _onInitializeCompleted;
        private string[] _dependencies = new string[0];

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="develop"></param>
        /// <param name="onCompleted"></param>
        public void Initialize(Action<bool> onCompleted)
        {
            isDevelop = Serializer<AppConfig>.Instance.isDevelopmentVersion;
            isBuildNameHash = Serializer<AppConfig>.Instance.isBuildAssetBunldeNameHash;
            AssetBundlePath = IOPath.PathCombine(isDevelop ? UApplication.TempDirectory : Application.persistentDataPath, Platform.RuntimePlatformCurrentName);
            _onInitializeCompleted = onCompleted;
            ManifestRequest.Allocate().AddCallback(OnInitialize).Load();
        }

        private void OnInitialize(AssetRequest request)
        {
            if (!request.isDone)
            {
                _onInitializeCompleted(false);
                return;
            }

            var manifest = request.GetRequest<ManifestRequest>().manifest;
            var directorys = manifest.dirs;
            var assets = manifest.assets;
            var bundles = manifest.bundles;

            for (var i = 0; i < bundles.Length; i++)
            {
                var bundle = bundles[i];
                Bundle2DependencieDict[bundle.name] = Array.ConvertAll(bundle.dependencies, id => bundles[id].name);
            }

            for (var i = 0; i < assets.Length; i++)
            {
                var asset = assets[i];
                var path = $"{directorys[asset.dirIndex]}/{asset.name}";
                if (asset.bundle >= 0 && asset.bundle < bundles.Length)
                {
                    Asset2BundleDict[path] = bundles[asset.bundle].name;
                }
                else
                {
                    Logger.Error($"{path} bundle {asset.bundle} not exist.");
                }
            }

            request.Unload();

            _onInitializeCompleted(true);
        }

        /// <summary>
        /// Bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="async">异步</param>
        /// <returns></returns>
        public T GetBundle<T>(string bundleName, bool async) where T : AssetRequest
        {
            var bundleUrl = bundleName;
            if (_bundleDict.TryGetValue(bundleName, out var bundle))
            {
                return bundle as T;
            }

            if (bundleName.StartsWith("http://", StringComparison.Ordinal) ||
                bundleName.StartsWith("https://", StringComparison.Ordinal) ||
                bundleName.StartsWith("file://", StringComparison.Ordinal) ||
                bundleName.StartsWith("ftp://", StringComparison.Ordinal))
            {
                bundle = new WebBundleRequest {url = bundleName};
            }
            else
            {
                if (async) bundle = BundleAsyncRequest.Allocate();
                else bundle = BundleRequest.Allocate();
                bundle.url = IOPath.PathCombine(AssetBundlePath, bundleName);
            }

            bundle.name = bundleName;
            _bundleDict.Add(bundleName, bundle);

            return bundle as T;
        }

        /// <summary>
        /// Asset
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="async"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>(string url, Type type, bool async) where T : AssetRequest
        {
            if (_assetDict.TryGetValue(url, out var asset))
            {
                return asset as T;
            }

            // bundle asset
            if (Asset2BundleDict.ContainsKey(url))
            {
                if (async) asset = BundleAssetAsyncRequest.Allocate();
                else asset = BundleAssetRequest.Allocate();
            }
            // web asset
            else if (url.StartsWith("http://", StringComparison.Ordinal) ||
                     url.StartsWith("https://", StringComparison.Ordinal) ||
                     url.StartsWith("file://", StringComparison.Ordinal) ||
                     url.StartsWith("ftp://", StringComparison.Ordinal) ||
                     url.StartsWith("jar:file://", StringComparison.Ordinal))
            {
                asset = WebAssetRequest.Allocate();
            }
            // resources asset
            else
            {
                if (async) asset = ResourceAssetAsyncRequest.Allocate();
                else asset = ResourceAssetRequest.Allocate();
            }

            asset.name = name;
            asset.url = url;
            asset.assetType = type;
            _assetDict.Add(url, asset);

            return asset as T;
        }

        /// <summary>
        /// 回收 AssetRequest 对象
        /// </summary>
        /// <param name="request"></param>
        public void RecycleAsset(AssetRequest request)
        {
            Logger.Debug("recycle asset: " + request.name);
            if (request.isAsset)
            {
                if (_assetDict.ContainsKey(request.name))
                    _assetDict.Remove(request.name);
            }
            else
            {
                if (_bundleDict.ContainsKey(request.name))
                    _bundleDict.Remove(request.name);
            }
        }

        /// <summary>
        /// 通过资源名称获取bundle名称
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public string GetBundleNameWithAssetName(string asset)
        {
            if (Asset2BundleDict.TryGetValue(asset, out var bundle))
                return bundle;
            Logger.Error($"{asset} not exist.");
            return null;
        }

        /// <summary>
        /// 获取bundle依赖
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public string[] GetDependencies(string bundle)
        {
            if (Bundle2DependencieDict.TryGetValue(bundle, out var dependencies))
            {
                return dependencies;
            }

            return _dependencies;
        }

        #region API

        /// <summary>
        /// bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static AssetRequest LoadBundleAsync(string path, UCallback<AssetRequest> callback)
        {
            var asset = Instance.GetBundle<BundleAsyncRequest>(path, true);
            asset?.AddCallback(callback);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static AssetRequest LoadBundle(string path)
        {
            var asset = Instance.GetBundle<BundleRequest>(path, false);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// bundle asset
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static AssetRequest LoadAssetAsync(string path, Type type, UCallback<AssetRequest> callback)
        {
            var asset = Instance.GetAsset<BundleAssetAsyncRequest>(path, type, true);
            asset?.AddCallback(callback);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// bundle asset
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AssetRequest LoadAsset(string path, Type type)
        {
            var asset = Instance.GetAsset<BundleAssetRequest>(path, type, false);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static AssetRequest LoadResourceAssetAsync(string path, Type type, UCallback<AssetRequest> callback)
        {
            var asset = Instance.GetAsset<ResourceAssetAsyncRequest>(path, type, true);
            asset?.AddCallback(callback);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AssetRequest LoadResourceAsset(string path, Type type)
        {
            var asset = Instance.GetAsset<ResourceAssetRequest>(path, type, false);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// resource
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AssetRequest LoadWebAsset(string path, Type type, UCallback<AssetRequest> callback)
        {
            var asset = Instance.GetAsset<WebAssetRequest>(path, type, true);
            asset?.AddCallback(callback);
            asset?.Load();
            return asset;
        }

        /// <summary>
        /// unload
        /// </summary>
        /// <param name="asset"></param>
        public static void UnloadAsset(AssetRequest asset)
        {
            asset?.Unload();
        }

        /// <summary>
        /// bundle path 2 bundle name(hash)
        /// </summary>
        /// <param name="bundlePath"></param>
        /// <returns></returns>
        public static string BundlePath2BundleName(string bundlePath)
        {
            return (isBuildNameHash ? HashUtils.GetMD5Hash(bundlePath) : bundlePath) + Extension;
        }

        #endregion
    }
}