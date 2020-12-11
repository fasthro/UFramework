/*
 * @Author: fasthro
 * @Date: 2020-09-22 17:45:37
 * @Description: Asset Sample
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Sample
{
    public class AssetSample : SampleScene
    {
        protected override void OnRenderGUI()
        {
            if (GUILayout.Button("Load Asset", GUILayout.Width(300), GUILayout.Height(100)))
            {
                var asset = Assets.LoadAssetAsync("Assets/Art/Cube/Prefab/Cube1.prefab", typeof(GameObject), (request) =>
                {
                    GameObject.Instantiate(request.asset, Vector3.zero, Quaternion.identity);
                });
            }
        }
    }
}