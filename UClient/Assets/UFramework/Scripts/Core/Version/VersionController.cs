/*
 * @Author: fasthro
 * @Date: 2020-10-12 16:26:35
 * @Description: 版本控制
 */
using System;

namespace UFramework.Version
{
    public class VersionController : MonoSingleton<VersionController>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="onCompleted"></param>
        public void Initialize(Action onCompleted)
        {
            onCompleted.InvokeGracefully();
        }
    }
}