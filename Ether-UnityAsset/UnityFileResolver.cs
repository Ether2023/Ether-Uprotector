using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ether_UnityAsset.Endian;
namespace Ether_UnityAsset
{
    public abstract class UnityFile<TVersionFormat> : IVersionAble<TVersionFormat> where TVersionFormat : Enum
    {
        public TVersionFormat Version { get; private set; }
        public string FullFilePath { get; private set; }
        public string FileName { get; private set; }
        public UnityFile(UnityFileReader _Reader)
        {
            FullFilePath = _Reader.FullFilePath;
            FileName = _Reader.FileName;
        }
        public virtual void Write(UnityFileWriter _Writer)
        {
        }
    }
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
    public class UnityFileReader : EndianBinaryReader
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
}
