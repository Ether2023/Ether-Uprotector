using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_Obfuscator.Unity
{
    public enum EndianType
    {
        //
        // 摘要:
        //     Little endian.
        LittleEndian,
        //
        // 摘要:
        //     Big endian.
        BigEndian
    }
    public interface IVersionAble<TVersionFormat> where TVersionFormat : Enum
    {
        //
        // 摘要:
        //     Current version.
        TVersionFormat Version { get; }
    }
    public abstract class UnityFile<TVersionFormat> : IVersionAble<TVersionFormat> where TVersionFormat : Enum
    {
        //
        // 摘要:
        //     The version format.
        public TVersionFormat Version { get; private set; }

        //
        // 摘要:
        //     The full file path.
        public string FullFilePath { get; private set; }

        //
        // 摘要:
        //     The file name.
        public string FileName { get; private set; }

        //
        // 摘要:
        //     Read a unity file.
        //
        // 参数:
        //   _Reader:
        public UnityFile(UnityFileReader _Reader)
        {
            FullFilePath = _Reader.FullFilePath;
            FileName = _Reader.FileName;
        }

        //
        // 摘要:
        //     Write a unity file.
        //
        // 参数:
        //   _Writer:
        public virtual void Write(UnityFileWriter _Writer)
        {
        }
    }

    public class UnityFileReader : EndianBinaryReader
{
    public enum UnityFileType
    {
        AssetsFile,
        BundleFile,
        WebFile,
        ResourceFile,
        ZipFile,
        GZipFile,
        BrotliFile
    }
    //
    // 摘要:
    //     Get the full file path.
    public string FullFilePath { get; private set; }

    //
    // 摘要:
    //     Get the file name.
    public string FileName { get; private set; }

    //
    // 摘要:
    //     Get the file type.
    public UnityFileType FileType { get; private set; }

    //
    // 摘要:
    //     Create a reader by the _FilePath.
    //
    // 参数:
    //   _FilePath:
    public UnityFileReader(string _FilePath)
        : this(_FilePath, File.Open(_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
    }

    //
    // 摘要:
    //     Create a reader for _FilePath by _Stream.
    //
    // 参数:
    //   _FilePath:
    //
    //   _Stream:
    public UnityFileReader(string _FilePath, Stream _Stream)
        : base(_Stream)
    {
        FullFilePath = Path.GetFullPath(_FilePath);
        FileName = Path.GetFileName(_FilePath);
        FileType = CheckFileType();
    }

    //
    // 摘要:
    //     Find the file type and reset stream position.
    private UnityFileType CheckFileType()
    {
        string text = ReadStringToNull(20);
        base.Position = 0L;
        switch (text)
        {
            case "UnityWeb":
            case "UnityRaw":
            case "UnityArchive":
            case "UnityFS":
                return UnityFileType.BundleFile;
            case "UnityWebData1.0":
                return UnityFileType.WebFile;
            default:
                if (IsZipFile())
                {
                    return UnityFileType.ZipFile;
                }

                if (IsGZipFile())
                {
                    return UnityFileType.GZipFile;
                }

                if (IsBrotliFile())
                {
                    return UnityFileType.BrotliFile;
                }

                if (IsAssetsFile())
                {
                    return UnityFileType.AssetsFile;
                }

                return UnityFileType.ResourceFile;
        }
    }

    private bool IsZipFile()
    {
        long position = base.Position;
        base.Position = 0L;
        byte[] second = ReadBytes(4);
        base.Position = position;
        byte[] first = new byte[4] { 80, 75, 3, 4 };
        byte[] first2 = new byte[4] { 80, 75, 7, 8 };
        if (!first.SequenceEqual(second))
        {
            return first2.SequenceEqual(second);
        }

        return true;
    }

    private bool IsGZipFile()
    {
        long position = base.Position;
        base.Position = 0L;
        byte[] second = ReadBytes(2);
        base.Position = position;
        return new byte[2] { 31, 139 }.SequenceEqual(second);
    }

    private bool IsBrotliFile()
    {
        long position = base.Position;
        base.Position = 32L;
        byte[] second = ReadBytes(6);
        base.Position = position;
        return new byte[6] { 98, 114, 111, 116, 108, 105 }.SequenceEqual(second);
    }

    //
    // 摘要:
    //     Find if the file is a assets unity file.
    private bool IsAssetsFile()
    {
        long position = base.Position;
        base.Position = 0L;
        long length = BaseStream.Length;
        if (length < 20)
        {
            return false;
        }

        ReadUInt32();
        long num = ReadUInt32();
        uint num2 = ReadUInt32();
        long num3 = ReadUInt32();
        ReadByte();
        ReadBytes(3);
        if ((int)num2 >= 22)
        {
            if (length < 48)
            {
                base.Position = position;
                return false;
            }

            ReadUInt32();
            num = ReadInt64();
            num3 = ReadInt64();
        }

        base.Position = position;
        if (num != length)
        {
            return false;
        }

        if (num3 > length)
        {
            return false;
        }

        return true;
    }
}
    public class UnityFileWriter : EndianBinaryWriter
{
    //
    // 摘要:
    //     Get the full file path.
    public string FullFilePath { get; private set; }

    //
    // 摘要:
    //     Get the file name.
    public string FileName { get; private set; }

    //
    // 摘要:
    //     Create a reader by the _FilePath.
    //
    // 参数:
    //   _FilePath:
    public UnityFileWriter(string _FilePath)
        : this(_FilePath, File.Open(_FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
    {
    }

    //
    // 摘要:
    //     Create a reader for _FilePath by _Stream.
    //
    // 参数:
    //   _FilePath:
    //
    //   _Stream:
    public UnityFileWriter(string _FilePath, Stream _Stream)
        : base(_Stream)
    {
        FullFilePath = Path.GetFullPath(_FilePath);
        FileName = Path.GetFileName(_FilePath);
    }
}
    public class EndianBinaryReader : BinaryReader
{
    //
    // 摘要:
    //     Simple read 2 byte buffer.
    private readonly byte[] buffer2;

    //
    // 摘要:
    //     Simple read 4 byte buffer.
    private readonly byte[] buffer4;

    //
    // 摘要:
    //     Simple read 8 byte buffer.
    private readonly byte[] buffer8;

    //
    // 摘要:
    //     The EndianType to read.
    public EndianType Endian { get; set; }

    //
    // 摘要:
    //     The BaseStream Position.
    public long Position
    {
        get
        {
            return BaseStream.Position;
        }
        set
        {
            BaseStream.Position = value;
        }
    }

    //
    // 摘要:
    //     The EndianType of the BitConverter.
    private EndianType BitConverterEndian
    {
        get
        {
            if (!BitConverter.IsLittleEndian)
            {
                return EndianType.BigEndian;
            }

            return EndianType.LittleEndian;
        }
    }

    //
    // 摘要:
    //     The endian binary reader needs a stream to read from and the endian.
    //
    // 参数:
    //   stream:
    //
    //   endian:
    public EndianBinaryReader(Stream stream, EndianType endian = EndianType.BigEndian)
        : base(stream)
    {
        Endian = endian;
        buffer2 = new byte[2];
        buffer4 = new byte[4];
        buffer8 = new byte[8];
    }

    //
    // 摘要:
    //     Unity uses 4 byte blocks. Align the stream if uneven to it.
    public void AlignStream()
    {
        AlignStream(4);
    }

    //
    // 摘要:
    //     Align the stream to _Alignment if uneven to it.
    public void AlignStream(int _Alignment)
    {
        long num = Position % _Alignment;
        if (num != 0L)
        {
            Position += _Alignment - num;
        }
    }

    //
    // 摘要:
    //     Read a Int16 depend on Endian.
    public override short ReadInt16()
    {
        Read(buffer2, 0, 2);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer2);
        }

        return BitConverter.ToInt16(buffer2, 0);
    }

    //
    // 摘要:
    //     Read a UInt16 depend on Endian.
    public override ushort ReadUInt16()
    {
        Read(buffer2, 0, 2);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer2);
        }

        return BitConverter.ToUInt16(buffer2, 0);
    }

    //
    // 摘要:
    //     Read a Int32 depend on Endian.
    public override int ReadInt32()
    {
        Read(buffer4, 0, 4);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer4);
        }

        return BitConverter.ToInt32(buffer4, 0);
    }

    //
    // 摘要:
    //     Read a UInt32 depend on Endian.
    public override uint ReadUInt32()
    {
        Read(buffer4, 0, 4);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer4);
        }

        return BitConverter.ToUInt32(buffer4, 0);
    }

    //
    // 摘要:
    //     Read a Int64 depend on Endian.
    public override long ReadInt64()
    {
        Read(buffer8, 0, 8);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer8);
        }

        return BitConverter.ToInt64(buffer8, 0);
    }

    //
    // 摘要:
    //     Read a UInt64 depend on Endian.
    public override ulong ReadUInt64()
    {
        Read(buffer8, 0, 8);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer8);
        }

        return BitConverter.ToUInt64(buffer8, 0);
    }

    //
    // 摘要:
    //     Read a Float depend on Endian.
    public override float ReadSingle()
    {
        Read(buffer4, 0, 4);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer4);
        }

        return BitConverter.ToSingle(buffer4, 0);
    }

    //
    // 摘要:
    //     Read a Double depend on Endian.
    public override double ReadDouble()
    {
        Read(buffer8, 0, 8);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(buffer8);
        }

        return BitConverter.ToDouble(buffer8, 0);
    }

    //
    // 摘要:
    //     Read an aligned string.
    public string ReadAlignedString()
    {
        int num = ReadInt32();
        if (num > 0 && num <= BaseStream.Length - BaseStream.Position)
        {
            byte[] bytes = ReadBytes(num);
            string @string = Encoding.UTF8.GetString(bytes);
            AlignStream();
            return @string;
        }

        return "";
    }

    //
    // 摘要:
    //     Read a string until read null byte.
    //
    // 参数:
    //   _MaxLength:
    public string ReadStringToNull(int _MaxLength = 32767)
    {
        List<byte> list = new List<byte>();
        int num = 0;
        while (BaseStream.Position != BaseStream.Length && num < _MaxLength)
        {
            byte b = ReadByte();
            if (b == 0)
            {
                break;
            }

            list.Add(b);
            num++;
        }

        return Encoding.UTF8.GetString(list.ToArray());
    }
}
    public class EndianBinaryWriter : BinaryWriter
{
    //
    // 摘要:
    //     The EndianType to read.
    public EndianType Endian { get; set; }

    //
    // 摘要:
    //     The BaseStream Position.
    public long Position
    {
        get
        {
            return BaseStream.Position;
        }
        set
        {
            BaseStream.Position = value;
        }
    }

    //
    // 摘要:
    //     The EndianType of the BitConverter.
    private EndianType BitConverterEndian
    {
        get
        {
            if (!BitConverter.IsLittleEndian)
            {
                return EndianType.BigEndian;
            }

            return EndianType.LittleEndian;
        }
    }

    //
    // 摘要:
    //     The endian binary writer needs a stream to write to and the endian.
    //
    // 参数:
    //   stream:
    //
    //   endian:
    public EndianBinaryWriter(Stream stream, EndianType endian = EndianType.BigEndian)
        : base(stream)
    {
        Endian = endian;
    }

    //
    // 摘要:
    //     Unity uses 4 byte blocks. Align the stream if uneven to it.
    public void AlignStream()
    {
        AlignStream(4);
    }

    //
    // 摘要:
    //     Align the stream to _Alignment if uneven to it.
    public void AlignStream(int _Alignment)
    {
        long num = BaseStream.Position % _Alignment;
        if (num != 0L)
        {
            Write(new byte[_Alignment - num]);
        }
    }

    //
    // 摘要:
    //     Write a Int16 depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(short _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a UInt16 depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(ushort _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a Int32 depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(int _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a UInt32 depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(uint _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a Int64 depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(long _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a UInt64 depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(ulong _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a float depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(float _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write a double depend on Endian.
    //
    // 参数:
    //   _Value:
    public override void Write(double _Value)
    {
        byte[] bytes = BitConverter.GetBytes(_Value);
        if (Endian != BitConverterEndian)
        {
            Array.Reverse(bytes);
        }

        Write(bytes);
    }

    //
    // 摘要:
    //     Write an aligned string and write the length.
    //
    // 参数:
    //   _String:
    public void WriteAlignedString(string _String)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(_String);
        Write(bytes.Length);
        Write(bytes);
        AlignStream();
    }

    //
    // 摘要:
    //     Write a string without writing the length. And add a NULL at the end.
    //
    // 参数:
    //   _String:
    public void WriteStringToNull(string _String)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(_String);
        Write(bytes);
        Write(new byte[1]);
    }
}
}