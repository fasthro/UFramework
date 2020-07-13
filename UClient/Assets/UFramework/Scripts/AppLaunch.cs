/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using UnityEngine;

namespace UFramework
{
    [MonoSingletonPath("UFramework")]
    public class AppLaunch : MonoSingleton<AppLaunch>
    {
        public static AppLaunch main { get { return AppLaunch.Instance; } }
        public static GameObject mainGameObject { get { return AppLaunch.Instance.gameObject; } }
    }
}