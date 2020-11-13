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
        public int position { get; private set; }
        public int capacity { get; private set; }

        public FixedByteArray(int capacity)
        {
            this.capacity = capacity;
            this.position = -1;
            _buffer = new byte[capacity];
        }

        public FixedByteArray Write(short value)
        {
            if (CheckCapacity(2))
            {
                _buffer[++position] = (byte)value;
                _buffer[++position] = (byte)(value >> 8);
            }
            return this;
        }

        public FixedByteArray Write(byte[] value)
        {
            if (CheckCapacity(value.Length))
            {
                for (int i = 0; i < value.Length; i++)
                    _buffer[++position] = value[i];
            }
            return this;
        }

        public FixedByteArray Write(int value)
        {
            if (CheckCapacity(4))
            {
                _buffer[++position] = (byte)value;
                _buffer[++position] = (byte)(value >> 8);
                _buffer[++position] = (byte)(value >> 16);
                _buffer[++position] = (byte)(value >> 24);
            }
            return this;
        }

        public FixedByteArray Write(long value)
        {
            if (CheckCapacity(8))
            {
                _buffer[++position] = (byte)value;
                _buffer[++position] = (byte)(value >> 8);
                _buffer[++position] = (byte)(value >> 16);
                _buffer[++position] = (byte)(value >> 24);
                _buffer[++position] = (byte)(value >> 32);
                _buffer[++position] = (byte)(value >> 40);
                _buffer[++position] = (byte)(value >> 48);
                _buffer[++position] = (byte)(value >> 56);
            }
            return this;
        }

        public virtual short ReadInt16()
        {
            return (short)(_buffer[++position] | _buffer[++position] << 8);
        }

        public int ReadInt32()
        {
            return (int)(_buffer[++position] | _buffer[++position] << 8 | _buffer[++position] << 16 | _buffer[++position] << 24);
        }

        public virtual long ReadInt64()
        {
            uint lo = (uint)(_buffer[++position] | _buffer[++position] << 8 |
                             _buffer[++position] << 16 | _buffer[++position] << 24);
            uint hi = (uint)(_buffer[++position] | _buffer[++position] << 8 |
                             _buffer[++position] << 16 | _buffer[++position] << 24);
            return (long)((ulong)hi) << 32 | lo;
        }

        public byte[] GetBuffer()
        {
            return _buffer;
        }

        public bool isEmpty()
        {
            return position == -1;
        }

        public void Reset()
        {
            position = -1;
        }

        private bool CheckCapacity(int len)
        {
            if (position + 2 <= capacity) return true;
            else Debug.LogError("FixedByteArray 数组容量已满, 无法继续写入数据!");
            return false;
        }
    }
}