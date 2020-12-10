/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: web asset
 */
using System.Collections;
using UFramework.Pool;
using UnityEngine;
using UnityEngine.Networking;

namespace UFramework.Assets
{
    public class WebAssetRequest : AssetRequest
    {
        public override bool isAsset { get { return true; } }

        private UnityWebRequest _request;

        public static WebAssetRequest Allocate()
        {
            return ObjectPool<WebAssetRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<WebAssetRequest>.Instance.Recycle(this);
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            if (assetType == typeof(AudioClip))
            {
                _request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            }
            else if (assetType == typeof(Texture2D))
            {
                _request = UnityWebRequestTexture.GetTexture(url);
            }
            else
            {
                _request = new UnityWebRequest(url);
                _request.downloadHandler = new DownloadHandlerBuffer();
            }
            yield return _request.SendWebRequest();
            if (loadState == LoadState.LoadBundle && _request.isDone)
            {
                if (assetType != typeof(Texture2D))
                {
                    if (assetType != typeof(TextAsset))
                    {
                        if (assetType != typeof(AudioClip))
                            bytes = _request.downloadHandler.data;
                        else
                            asset = DownloadHandlerAudioClip.GetContent(_request);
                    }
                    else
                    {
                        text = _request.downloadHandler.text;
                    }
                }
                else
                {
                    asset = DownloadHandlerTexture.GetContent(_request);
                }
                loadState = LoadState.Loaded;
                OnAsyncCallback();
            }
        }


        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
            StartCoroutine();
        }

        protected override void OnReferenceEmpty()
        {
            if (asset != null)
            {
                Object.Destroy(asset);
                asset = null;
            }
            if (_request != null)
            {
                _request.Dispose();
                _request = null;
            }
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}