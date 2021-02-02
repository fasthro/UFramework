// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using System;

namespace UFramework
{
    public class FixedByteArray : AutoByteArray
    {
        public byte[] buffer => _buffer;

        public FixedByteArray(int capacity) : base(capacity, capacity)
        {
        }
        
        public void Write(short value)
        {
            Write(EndianReverse(BitConverter.GetBytes(value)));
        }

        public void Write(ushort value)
        {
            Write(EndianReverse(BitConverter.GetBytes(value)));
        }

        public void Write(int value)
        {
            Write(EndianReverse(BitConverter.GetBytes(value)));
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(EndianReverse(Read(2)), 0);
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