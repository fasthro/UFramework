/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using UFramework.Assets;
using UFramework.Native;
using UFramework.Tools;
using UnityEngine;

namespace UFramework
{
    [MonoSingletonPath("UFramework")]
    public class AppLaunch : MonoSingleton<AppLaunch>
    {
        public static AppLaunch main { get { return AppLaunch.Instance; } }
        public static GameObject mainGameObject { get { return AppLaunch.Instance.gameObject; } }

        protected override void OnSingletonAwake()
        {
            ThreadQueue.Instance.Default();
            UNative.Instance.Default();

            Asset.Instance.Initialize(OnAssetCompleted);
        }

        private void OnAssetCompleted(bool result)
        {
            if (!result)
            {
                return;
            }
            var asset = Asset.LoadAssetAsync("Assets/Art/Cube/Prefab/Cube1.prefab", typeof(GameObject), (request) =>
            {
                var go = GameObject.Instantiate(request.asset, Vector3.zero, Quaternion.identity) as GameObject;
            });
        }

        protected override void OnSingletonStart()
        {
            App.Initialize();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            App.Update(deltaTime);
        }

        protected override void OnSingletonLateUpdate()
        {
            App.LateUpdate();
        }

        protected override void OnSingletonFixedUpdate()
        {
            App.FixedUpdate();
        }

        protected override void OnSingletonDestory()
        {
            App.Destory();
        }
    }
}