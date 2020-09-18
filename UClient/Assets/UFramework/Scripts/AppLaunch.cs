/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using UFramework.Asset;
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

            UAsset.Instance.Initialize(false, (result) =>
            {
                Debug.Log("UAsset Initlized Result: " + result);
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