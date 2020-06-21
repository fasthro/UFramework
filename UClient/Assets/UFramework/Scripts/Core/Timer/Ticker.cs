/*
 * @Author: fasthro
 * @Date: 2020-06-21 10:26:23
 * @Description: ticker
 */

using System;
using UFramework.Messenger;

namespace UFramework.Timer
{
    public class Ticker
    {
        public uint remain;
        public uint frameCount;
        public object param;
        public UCallback<object> callback;
    }
}
