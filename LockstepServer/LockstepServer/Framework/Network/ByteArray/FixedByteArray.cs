/*
 * @Author: fasthro
 * @Date: 2020/12/22 12:50:38
 * @Description:
 */

using System;

namespace LockstepServer
{
    public class FixedByteArray : AutoByteArray
    {
        public FixedByteArray(int capacity) : base(capacity, capacity)
        {
        }

        public byte[] buffer
        {
            get { return _buffer; }
        }

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

        public int ReadInt32()
        {
            return BitConverter.ToInt32(EndianReverse(Read(4)), 0);
        }

        private static byte[] EndianReverse(byte[] data)
        {
            if (GameConst.LITTLE_ENDIAN)
            {
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(data);
            }
            else
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(data);
            }
            return data;
        }
    }
}