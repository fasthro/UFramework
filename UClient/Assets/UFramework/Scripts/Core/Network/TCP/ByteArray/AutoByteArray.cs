/*
 * @Author: fasthro
 * @Date: 2020-11-09 14:57:42
 * @Description: aotu capacity
 */
using System;

namespace UFramework.Network
{
    public class AutoByteArray
    {
        /// <summary>
        /// 数据长度
        /// </summary>
        /// <value></value>
        public int size { get; private set; }

        /// <summary>
        /// 空闲长度
        /// </summary>
        /// <value></value>
        public int freeSize { get { return _buffer.Length - size; } }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <value></value>
        public bool isEmpty
        {
            get { return size == 0; }
        }

        protected byte[] _buffer;
        private int _minCapacity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity">初始容量</param>
        /// <param name="minCapacity">最小容量</param>
        public AutoByteArray(int capacity = 16, int minCapacity = 16)
        {
            _buffer = new byte[capacity];
            _minCapacity = minCapacity;
            size = 0;
        }

        public void Write(byte[] value)
        {
            Write(value, value.Length);
        }

        public void Write(byte[] value, int len)
        {
            if (freeSize < len)
                Resize((size + len) * 2);
            Array.Copy(value, 0, _buffer, size, len);
            size += len;
        }

        public byte[] ReadAll()
        {
            return Read(size);
        }

        public byte[] Read(int len)
        {
            if (len <= size)
            {
                byte[] newBytes = new byte[len];
                Array.Copy(_buffer, 0, newBytes, 0, len);

                size -= len;
                for (int i = 0; i < size; i++)
                    _buffer[i] = _buffer[i + len];

                if (size <= _buffer.Length / 4)
                {
                    var ns = _buffer.Length / 2;
                    if (ns > _minCapacity)
                        Resize(ns);
                }
                return newBytes;
            }
            return new byte[] { };
        }

        private void Resize(int tlen)
        {
            byte[] newBytes = new byte[tlen];
            Array.Copy(_buffer, 0, newBytes, 0, size);
            _buffer = newBytes;
        }

        public void Clear()
        {
            size = 0;
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < _buffer.Length; i++)
            {
                if (i < _buffer.Length - 1)
                {
                    res += _buffer[i] + ",";
                }
                else
                {
                    res += _buffer[i];
                }
            }
            return string.Format("capacity: {0} size: {1} [{2}]", _buffer.Length, size, res);
        }
    }
}