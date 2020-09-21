/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: web asset
 */
using System.Collections;
<<<<<<< HEAD
=======
using System.Collections.Generic;
using UFramework.Coroutine;
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
using UFramework.Pool;
using UnityEngine;
using UnityEngine.Networking;

<<<<<<< HEAD
namespace UFramework.Assets
=======
namespace UFramework.Asset
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
{
    public class WebAssetRequest : AssetRequest
    {
        private UnityWebRequest request;

<<<<<<< HEAD
        public override bool isAsset { get { return true; } }

        public static WebAssetRequest Allocate()
        {
            return ObjectPool<WebAssetRequest>.Instance.Allocate();
=======
        public static WebAssetRequest Allocate(string _url)
        {
            return ObjectPool<WebAssetRequest>.Instance.Allocate().Initialize(_url);
        }

        private WebAssetRequest Initialize(string _url)
        {
            url = _url;
            return this;
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
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
<<<<<<< HEAD
                OnAsyncCallback();
            }
=======
            }
            UCoroutineTask.TaskComplete();
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
        }


        public override void Load()
        {
<<<<<<< HEAD
            base.Load();
            if (loadState != LoadState.Init) return;
            StartCoroutine();
=======
            if (loadState != LoadState.Init) return;
            UCoroutineTask.AddTaskRunner(this);
        }

        public override void Unload()
        {
            Release();
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
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