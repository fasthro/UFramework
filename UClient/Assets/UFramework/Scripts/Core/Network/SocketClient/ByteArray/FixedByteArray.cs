/*
 * @Author: fasthro
 * @Date: 2020-11-09 13:07:59
 * @Description: fixed capacity
 */
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Network
{
    public class FixedByteArray
    {
        private byte[] _buffer;
        public int size { get; private set; }
        public int _cursor { get; private set; }
        public int freeSize { get { return _buffer.Length - size; } }
        public byte[] data { get { return _buffer; } }
        public bool isEmpty
        {
            get { return size == 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity">容量</param>
        public FixedByteArray(int capacity)
        {
            _buffer = new byte[capacity];
            size = 0;
            _cursor = -1;
        }

        public bool Write(short value)
        {
            if (CheckCapacity(2))
            {
                _buffer[++_cursor] = (byte)value;
                _buffer[++_cursor] = (byte)(value >> 8);
                size += 2;
                return true;
            }
            return false;
        }

        public bool Write(byte[] value)
        {
            if (CheckCapacity(value.Length))
            {
                for (int i = 0; i < value.Length; i++)
                    _buffer[++_cursor] = value[i];
                size += value.Length;
                return true;
            }
            return false;
        }

        public bool Write(int value)
        {
            if (CheckCapacity(4))
            {
                _buffer[++_cursor] = (byte)value;
                _buffer[++_cursor] = (byte)(value >> 8);
                _buffer[++_cursor] = (byte)(value >> 16);
                _buffer[++_cursor] = (byte)(value >> 24);
                size += 4;
                return true;
            }
            return false;
        }

        public bool Write(long value)
        {
            if (CheckCapacity(8))
            {
                _buffer[++_cursor] = (byte)value;
                _buffer[++_cursor] = (byte)(value >> 8);
                _buffer[++_cursor] = (byte)(value >> 16);
                _buffer[++_cursor] = (byte)(value >> 24);
                _buffer[++_cursor] = (byte)(value >> 32);
                _buffer[++_cursor] = (byte)(value >> 40);
                _buffer[++_cursor] = (byte)(value >> 48);
                _buffer[++_cursor] = (byte)(value >> 56);
                size += 8;
                return true;
            }
            return false;
        }

        public virtual short ReadInt16()
        {
            return (short)(_buffer[++_cursor] | _buffer[++_cursor] << 8);
        }

        public int ReadInt32()
        {
            return (int)(_buffer[++_cursor] | _buffer[++_cursor] << 8 | _buffer[++_cursor] << 16 | _buffer[++_cursor] << 24);
        }

        public virtual long ReadInt64()
        {
            uint lo = (uint)(_buffer[++_cursor] | _buffer[++_cursor] << 8 |
                             _buffer[++_cursor] << 16 | _buffer[++_cursor] << 24);
            uint hi = (uint)(_buffer[++_cursor] | _buffer[++_cursor] << 8 |
                             _buffer[++_cursor] << 16 | _buffer[++_cursor] << 24);
            return (long)((ulong)hi) << 32 | lo;
        }

        public void Clear()
        {
            size = 0;
            _cursor = -1;
        }
        
        private bool CheckCapacity(int len)
        {
            return size + len <= _buffer.Length;
        }
    }
}