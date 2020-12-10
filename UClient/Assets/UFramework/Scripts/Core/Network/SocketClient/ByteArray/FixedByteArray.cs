/*
 * @Author: fasthro
 * @Date: 2020-11-09 13:07:59
 * @Description: fixed capacity
 */
using System;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Network
{
    public class FixedByteArray : AutoByteArray
    {
        public byte[] buffer
        {
            get { return _buffer; }
        }

        public FixedByteArray(int capacity) : base(capacity, capacity) { }

        public void Write(ushort value)
        {
            Write(EndianReverse(BitConverter.GetBytes(value)));
        }

        public void Write(int value)
        {
            Write(EndianReverse(BitConverter.GetBytes(value)));
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(EndianReverse(Read(2)), 0);
        }

        static byte[] EndianReverse(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return data;
        }
    }
}