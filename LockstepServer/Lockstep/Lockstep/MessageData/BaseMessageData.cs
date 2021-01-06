/*
 * @Author: fasthro
 * @Date: 2021-01-06 12:16:07
 * @Description: 
 */
using System;
using Google.Protobuf;

namespace Lockstep.MessageData
{
    public abstract class BaseMessageData : ICloneable
    {
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public abstract object DeepClone();
        public abstract object ToMessage();
        public abstract void FromMessage(IMessage message);
    }
}

