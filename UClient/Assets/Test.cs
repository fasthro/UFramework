using System.Collections;
using System.Collections.Generic;
using UFramework.Config;
using UnityEngine;
using UFramework.ResLoader;

public class Test : MonoBehaviour
{
    AssetBundleLoader loader;

    void Start()
    {
        loader = AssetBundleLoader.AllocateRes("Assets/Art/Cube/Prefab/Cube1.prefab");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var ready = loader.LoadSync();
            if (ready)
            {
                var prefab = loader.bundleAssetRes.GetAsset<GameObject>();
                var go = GameObject.Instantiate(prefab) as GameObject;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            loader.Unload(true);
        }
    }
}
