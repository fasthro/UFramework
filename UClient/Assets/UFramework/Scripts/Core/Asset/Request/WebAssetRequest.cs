/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: web asset
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;
using UnityEngine.Networking;

namespace UFramework.Asset
{
    public class WebAssetRequest : AssetRequest
    {
        private UnityWebRequest request;

        public static WebAssetRequest Allocate(string _url)
        {
            return ObjectPool<WebAssetRequest>.Instance.Allocate().Initialize(_url);
        }

        private WebAssetRequest Initialize(string _url)
        {
            url = _url;
            return this;
        }

        public override void Recycle()
        {
            ObjectPool<WebAssetRequest>.Instance.Recycle(this);
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            if (assetType == typeof(AudioClip))
            {
                request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            }
            else if (assetType == typeof(Texture2D))
            {
                request = UnityWebRequestTexture.GetTexture(url);
            }
            else
            {
                request = new UnityWebRequest(url);
                request.downloadHandler = new DownloadHandlerBuffer();
            }
            yield return request.SendWebRequest();
            if (loadState == LoadState.LoadBundle && request.isDone)
            {
                if (assetType != typeof(Texture2D))
                {
                    if (assetType != typeof(TextAsset))
                    {
                        if (assetType != typeof(AudioClip))
                            bytes = request.downloadHandler.data;
                        else
                            asset = DownloadHandlerAudioClip.GetContent(request);
                    }
                    else
                    {
                        text = request.downloadHandler.text;
                    }
                }
                else
                {
                    asset = DownloadHandlerTexture.GetContent(request);
                }
                loadState = LoadState.Loaded;
            }
            UCoroutineTask.TaskComplete();
        }


        public override void Load()
        {
            if (loadState != LoadState.Init) return;
            UCoroutineTask.AddTaskRunner(this);
        }

        public override void Unload()
        {
            Release();
        }

        protected override void OnReferenceEmpty()
        {
            if (asset != null)
            {
                Object.Destroy(asset);
                asset = null;
            }
            if (request != null)
            {
                request.Dispose();
                request = null;
            }
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}