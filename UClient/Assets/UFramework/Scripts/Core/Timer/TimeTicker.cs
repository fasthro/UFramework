/*
 * @Author: fasthro
 * @Date: 2020-06-21 10:26:23
 * @Description: timer ticker
 */

using System;
using UFramework.Messenger;

namespace UFramework.Timer
{
    public class TimeTicker
    {
        public uint typeId;
        public uint frameCount;
        public uint refCount;
        public object param;
        public UCallback<uint, object> callback;
    }
}
