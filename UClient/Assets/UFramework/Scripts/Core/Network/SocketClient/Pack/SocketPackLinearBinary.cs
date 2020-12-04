/*
 * @Author: fasthro
 * @Date: 2020-11-09 11:01:48
 * @Description: linear stream
 */
using System;
using System.IO;
using System.Text;

namespace UFramework.Network
{
    public class SocketPackLinearBinary : SocketPack
    {
        public override ProtocalType protocal { get { return ProtocalType.LinearBinary; } }

        private MemoryStream _stream;
        private BinaryWriter _writer;
        private BinaryReader _reader;

        protected override void InitializeWriter(int cmd)
        {
            base.InitializeWriter(cmd);
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        protected override void InitializeReader(int cmd, byte[] data)
        {
            base.InitializeReader(cmd, data);
            _stream = new MemoryStream(data);
            _reader = new BinaryReader(_stream);
        }

        public override void Pack()
        {
            _stream.Flush();
            rawData = _stream.ToArray();
        }

        public void WriteByte(byte data)
        {
            _writer.Write(data);
        }

        public void WriteBytes(byte[] data, bool alone = true)
        {
            if (alone)
                _writer.Write(data.Length);
            _writer.Write(data);
        }

        public void WriteInt(int data)
        {
            _writer.Write(data);
        }

        public void WriteUint(uint data)
        {
            _writer.Write(data);
        }

        public void WriteShort(short data)
        {
            _writer.Write(data);
        }

        public void WriteUshort(ushort data)
        {
            _writer.Write(data);
        }

        public void WriteLong(long data)
        {
            _writer.Write(data);
        }

        public void WriteUlong(ulong data)
        {
            _writer.Write(data);
        }

        public void WriteBoolean(bool data)
        {
            _writer.Write(data);
        }

        public void WriteFloat(float data)
        {
            byte[] temp = EndianReverse(BitConverter.GetBytes(data));
            _writer.Write(temp.Length);
            _writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double data)
        {
            byte[] temp = EndianReverse(BitConverter.GetBytes(data));
            _writer.Write(temp.Length);
            _writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string data)
        {
            byte[] temp = Encoding.UTF8.GetBytes(data);
            _writer.Write(temp.Length);
            _writer.Write(temp);
        }

        public void WriteUnicodeString(string data)
        {
            byte[] temp = Encoding.Unicode.GetBytes(data);
            _writer.Write(temp.Length);
            _writer.Write(temp);
        }

        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        public byte[] ReadBytes()
        {
            return _reader.ReadBytes(ReadInt());
        }

        public int ReadInt()
        {
            return _reader.ReadInt32();
        }

        public uint ReadUint()
        {
            return _reader.ReadUInt32();
        }

        public short ReadShort()
        {
            return _reader.ReadInt16();
        }

        public ushort ReadUshort()
        {
            return _reader.ReadUInt16();
        }

        public long ReadLong()
        {
            return _reader.ReadInt64();
        }

        public ulong ReadUlong()
        {
            return _reader.ReadUInt64();
        }

        public bool ReadBoolean()
        {
            return _reader.ReadBoolean();
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(EndianReverse(ReadBytes()), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(EndianReverse(ReadBytes()), 0);
        }

        public string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes());
        }

        public string ReadUnicodeString()
        {
            return Encoding.Unicode.GetString(ReadBytes());
        }

        static byte[] EndianReverse(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return data;
        }
    }
}