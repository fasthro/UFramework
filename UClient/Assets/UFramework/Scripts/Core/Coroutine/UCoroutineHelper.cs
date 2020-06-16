/*
 * @Author: fasthro
 * @Date: 2020-05-26 23:37:07
 * @Description: UCoroutineHelper 负责协同程序调度
 */

using UnityEngine;

namespace UFramework.Coroutine
{
    public class UCoroutineHelper : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}