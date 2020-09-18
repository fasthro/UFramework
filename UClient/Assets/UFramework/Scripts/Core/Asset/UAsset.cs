/*
 * @Author: fasthro
 * @Date: 2020-09-17 15:33:31
 * @Description: uasset
 */

using System;
using System.Collections.Generic;
using UFramework;
using UnityEngine;

namespace UFramework.Asset
{
    [MonoSingletonPath("UFramework/UAsset")]
    public class UAsset : MonoSingleton<UAsset>
    {
        /// <summary>
        /// asset to bundle mapping
        /// </summary>
        /// <typeparam name="string">asset</typeparam>
        /// <typeparam name="string">bundle</typeparam>
        /// <returns></returns>
        private static Dictionary<string, string> asset2BundleDictionary = new Dictionary<string, string>();

        /// <summary>
        /// bundle to dependencies mapping
        /// </summary>
        /// <typeparam name="string">bundle</typeparam>
        /// <typeparam name="string[]">dependencies bundle</typeparam>
        /// <returns></returns>
        private static Dictionary<string, string[]> bundle2DependencieDictionary = new Dictionary<string, string[]>();

        /// <summary>
        /// assets
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="AssetObject"></typeparam>
        /// <returns></returns>
        private Dictionary<string, AssetObject> assetDictionary = new Dictionary<string, AssetObject>();

        /// <summary>
        /// bundles
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="BundleRequest"></typeparam>
        /// <returns></returns>
        private Dictionary<string, BundleRequest> bundleDictionary = new Dictionary<string, BundleRequest>();

        /// <summary>
        /// async bundles
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="BundleAsyncRequest"></typeparam>
        /// <returns></returns>
        private Dictionary<string, BundleAsyncRequest> bundleAsyncDictionary = new Dictionary<string, BundleAsyncRequest>();

        /// <summary>
        /// web bundle
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="WebBundleRequest"></typeparam>
        /// <returns></returns>
        private Dictionary<string, WebBundleRequest> webBundleDictionary = new Dictionary<string, WebBundleRequest>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="async"></param>
        /// <param name="onCompleted"></param>
        public void Initialize(bool async, Action<bool> onCompleted)
        {
            var loader = BundleLoader.AllocateAsset(Manifest.PATH, Manifest.BUNDLE_NAME + App.AssetBundleExtension, (result, obj) =>
            {
                if (async)
                {
                    if (result)
                    {
                        InitializeManifest(obj.GetAsset<Manifest>());
                        onCompleted.InvokeGracefully(true);
                    }
                    else onCompleted.InvokeGracefully(false);
                }
            });
            if (!async)
            {
                if (loader.LoadSync())
                {
                    InitializeManifest(loader.asset.GetAsset<Manifest>());
                    onCompleted.InvokeGracefully(true);
                }
                else onCompleted.InvokeGracefully(false);
            }
            else loader.LoadAsync();
        }

        /// <summary>
        /// 初始化 Manifest
        /// </summary>
        /// <param name="manifest"></param>
        private void InitializeManifest(Manifest manifest)
        {
            var directorys = manifest.directorys;
            var assets = manifest.assets;
            var bundles = manifest.bundles;

            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = bundles[i];
                bundle2DependencieDictionary[bundle.name] = Array.ConvertAll(bundle.dependencies, id => bundles[id].name);
            }

            for (int i = 0; i < assets.Length; i++)
            {
                var asset = assets[i];
                var path = string.Format("{0}/{1}", directorys[asset.directory], asset.name);
                if (asset.bundle >= 0 && asset.bundle < bundles.Length)
                {
                    asset2BundleDictionary[path] = bundles[asset.bundle].name;
                }
                else
                {
                    Debug.LogError(string.Format("{0} bundle {1} not exist.", path, asset.bundle));
                }
            }
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>(string bundleName, string assetName, AssetType type) where T : AssetObject, new()
        {
            string asset = string.IsNullOrEmpty(assetName) ? bundleName : assetName;
            AssetObject obj;
            if (!assetDictionary.TryGetValue(asset, out obj))
            {
                switch (type)
                {
                    case AssetType.Bundle:
                        obj = Bundle.Allocate(bundleName);
                        break;
                    case AssetType.BundleAsset:
                        obj = BundleAsset.Allocate(bundleName, assetName);
                        break;
                    case AssetType.Resource:
                        obj = ResourceAsset.Allocate(assetName);
                        break;
                }
                assetDictionary.Add(asset, obj);
            }
            return obj as T;
        }

        public BundleRequest LoadBundle(string url)
        {
            BundleRequest bundle;
            if (bundleDictionary.TryGetValue(url, out bundle))
                return bundle;
            bundle = BundleRequest.Allocate(url);
            bundle.Load();
            return bundle;
        }

        public BundleAsyncRequest LoadAsyncBundle(string url)
        {
            BundleAsyncRequest bundle;
            if (bundleAsyncDictionary.TryGetValue(url, out bundle))
                return bundle;
            bundle = BundleAsyncRequest.Allocate(url);
            bundle.Load();
            return bundle;
        }

        public WebBundleRequest LoadWebBundle(string url)
        {
            WebBundleRequest bundle;
            if (webBundleDictionary.TryGetValue(url, out bundle))
                return bundle;
            bundle = WebBundleRequest.Allocate(url);
            bundle.Load();
            return bundle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public string GetBundleNameWithAssetName(string asset)
        {
            string bundle;
            if (asset2BundleDictionary.TryGetValue(asset, out bundle))
                return bundle;
            Debug.LogError(string.Format("{0} not exist.", asset));
            return null;
        }

        /// <summary>
        /// 获取bundle依赖
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public string[] GetDependencies(string bundle)
        {
            string[] dependencies;
            if (bundle2DependencieDictionary.TryGetValue(bundle, out dependencies))
            {
                return dependencies;
            }
            return null;
        }

        /// <summary>
        /// 回收AssetObject
        /// </summary>
        /// <param name="asset"></param>
        public void RecycleAssetObject(string asset)
        {
            assetDictionary.Remove(asset);
        }
    }
}