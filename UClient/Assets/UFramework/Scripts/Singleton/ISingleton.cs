/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: ISingleton
 */

using System;

namespace UFramework
{
    public interface ISingleton : IDisposable
    {
        void SingletonAwake();
    }
}
