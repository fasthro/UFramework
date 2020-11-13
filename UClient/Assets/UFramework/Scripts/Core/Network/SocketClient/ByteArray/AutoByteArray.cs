/*
 * @Author: fasthro
 * @Date: 2020-11-09 14:57:42
 * @Description: aotu capacity
 */
using System;
using UnityEngine;

namespace UFramework.Network
{
    public class AutoByteArray
    {
        /// <summary>
        /// 默认容量
        /// </summary>
        readonly static int DEFAULT_CAPACITY = 16;

        private byte[] _buffer;
        public int dataSize { get; private set; }
        public int capacity { get; private set; }

        public AutoByteArray()
        {
            _buffer = new byte[DEFAULT_CAPACITY];
            dataSize = 0;
        }

        public AutoByteArray Write(byte[] value)
        {
            return Write(value, value.Length);
        }

        public AutoByteArray Write(byte[] value, int len)
        {
            Resize(len);
            Array.Copy(value, 0, _buffer, dataSize, len);
            dataSize += len;
            return this;
        }

        public byte[] Read(int len)
        {
            if (len <= dataSize)
            {
                byte[] newBytes = new byte[len];
                Array.Copy(_buffer, 0, newBytes, 0, len);
                dataSize -= len;
                return newBytes;
            }
            Debug.LogError("AutoByteArray 读取长度超出数据长度!");
            return null;
        }

        public byte[] ReadAll()
        {
            return Read(dataSize);
        }

        private void Resize(int len)
        {
            var free = capacity - dataSize;
            var expand = len - free;
            if (free < len)
            {
                byte[] newBytes = new byte[(capacity + expand) * 2];
                Array.Copy(_buffer, 0, newBytes, 0, _buffer.Length);
                _buffer = newBytes;
            }
        }

        public bool isEmpty()
        {
            return dataSize == 0;
        }

        public void Reset()
        {
            dataSize = 0;
        }
    }
}