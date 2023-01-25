using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace Ether.Il2cpp
{
    class Zip
    {
		public static int avg = 10485760;

		public static bool CreateZip(string FileToZip, string ZipedFile)
		{
			return CreateZip(FileToZip, ZipedFile, string.Empty);
		}

		public static bool ZipNo(string FolderToZip, string ZipedFile)
		{
			if (!Directory.Exists(FolderToZip))
			{
				return false;
			}
			if (ZipedFile == string.Empty)
			{
				ZipedFile = FolderToZip + ".zip";
			}
			ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(ZipedFile));
			zipOutputStream.SetLevel(6);
			string[] files = Directory.GetFiles(FolderToZip);
			ZipEntry zipEntry = null;
			FileStream fileStream = null;
			//Crc32 crc = new Crc32();
			for (int i = 0; i < files.Length; i++)
			{
				fileStream = File.OpenRead(files[i]);
				byte[] array = new byte[avg];
				zipEntry = new ZipEntry(Path.GetFileName(files[i]));
				zipEntry.DateTime = DateTime.MinValue;
				zipEntry.Size = fileStream.Length;
				zipOutputStream.PutNextEntry(zipEntry);
				for (int j = 0; j < fileStream.Length; j += avg)
				{
					if (j + avg > fileStream.Length)
					{
						array = new byte[fileStream.Length - j];
					}
					fileStream.Read(array, 0, array.Length);
					zipOutputStream.Write(array, 0, array.Length);
				}
			}
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream = null;
			}
			if (zipEntry != null)
			{
				zipEntry = null;
			}
			GC.Collect();
			GC.Collect(1);
			string[] directories = Directory.GetDirectories(FolderToZip);
			for (int k = 0; k < directories.Length; k++)
			{
				if (!ZipFileDictory(directories[k], zipOutputStream, string.Empty))
				{
				}
			}
			zipOutputStream.Finish();
			zipOutputStream.Close();
			return true;
		}

		public static bool CreateZip(string FileToZip, string ZipedFile, string Password)
		{
			if (Directory.Exists(FileToZip))
			{
				return ZipFileDictory(FileToZip, ZipedFile, Password);
			}
			if (File.Exists(FileToZip))
			{
				return ZipFile(FileToZip, ZipedFile, Password);
			}
			return false;
		}

		public static bool UnZip(string zipFilePath, string unZipDir)
		{
			//Discarded unreachable code: IL_017e
			if (zipFilePath == string.Empty)
			{
				throw new FileNotFoundException("压缩文件不不能为空！");
			}
			if (!File.Exists(zipFilePath))
			{
				throw new FileNotFoundException("压缩文件: " + zipFilePath + " 不存在!");
			}
			try
			{
				if (unZipDir == string.Empty)
				{
					unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), string.Empty);
				}
				if (!Directory.Exists(unZipDir))
				{
					Directory.CreateDirectory(unZipDir);
				}
				using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
				{
					ZipEntry nextEntry;
					while ((nextEntry = zipInputStream.GetNextEntry()) != null)
					{
						string directoryName = Path.GetDirectoryName(nextEntry.Name);
						string fileName = Path.GetFileName(nextEntry.Name);
						string path = (unZipDir + nextEntry.Name).Replace("\\", "/");
						if (directoryName.Length > 0)
						{
							Directory.CreateDirectory(Path.GetDirectoryName(path));
						}
						if (!(fileName != string.Empty))
						{
							continue;
						}
						using (FileStream fileStream = File.Create(path))
						{
							int num = 2048;
							byte[] array = new byte[2048];
							while (true)
							{
								num = zipInputStream.Read(array, 0, array.Length);
								if (num > 0)
								{
									fileStream.Write(array, 0, num);
									continue;
								}
								break;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("ZIp.Unzip ---->\n" + ex.Message);
			}
			return true;
		}

		private static bool ZipFileDictory(string FolderToZip, string ZipedFile, string Password)
		{
			if (!Directory.Exists(FolderToZip))
			{
				return false;
			}
			if (ZipedFile == string.Empty)
			{
				ZipedFile = FolderToZip + ".zip";
			}
			ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(ZipedFile));
			zipOutputStream.SetLevel(6);
			if (!string.IsNullOrEmpty(Password.Trim()))
			{
				zipOutputStream.Password = Password.Trim();
			}
			bool result = ZipFileDictory(FolderToZip, zipOutputStream, string.Empty, 1);
			zipOutputStream.Flush();
			zipOutputStream.Finish();
			zipOutputStream.Close();
			return result;
		}

		private static bool ZipFile(string FileToZip, string ZipedFile, string Password)
		{
			if (!File.Exists(FileToZip))
			{
				throw new FileNotFoundException("指定要压缩的文件: " + FileToZip + " 不存在!");
			}
			if (ZipedFile == string.Empty)
			{
				ZipedFile = FileToZip + ".zip";
			}
			FileStream fileStream = null;
			ZipOutputStream zipOutputStream = null;
			ZipEntry zipEntry = null;
			bool result = true;
			fileStream = File.Create(ZipedFile);
			zipOutputStream = new ZipOutputStream(fileStream);
			zipEntry = new ZipEntry(Path.GetFileName(FileToZip));
			zipOutputStream.PutNextEntry(zipEntry);
			zipOutputStream.SetLevel(6);
			if (!string.IsNullOrEmpty(Password.Trim()))
			{
				zipOutputStream.Password = Password.Trim();
			}
			try
			{
				fileStream = File.OpenRead(FileToZip);
				byte[] array = new byte[avg];
				for (int i = 0; i < fileStream.Length; i += avg)
				{
					if (i + avg > fileStream.Length)
					{
						array = new byte[fileStream.Length - i];
					}
					fileStream.Read(array, 0, array.Length);
					zipOutputStream.Write(array, 0, array.Length);
				}
				return result;
			}
			catch (Exception e)
			{
				return false;
			}
			finally
			{
				if (zipEntry != null)
				{
					zipEntry = null;
				}
				if (zipOutputStream != null)
				{
					zipOutputStream.Flush();
					zipOutputStream.Finish();
					zipOutputStream.Close();
				}
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
				GC.Collect();
				GC.Collect(1);
			}
		}

		private static bool ZipFileDictory(string FolderToZip, ZipOutputStream s, string ParentFolderName, int x = 0)
		{
			bool result = true;
			ZipEntry zipEntry = null;
			FileStream fileStream = null;
            //Crc32 crc = new Crc32();
            if (x == 1)
            {
				string[] files = Directory.GetFiles(FolderToZip);
				for (int i = 0; i < files.Length; i++)
				{
					fileStream = File.OpenRead(files[i]);
					byte[] array = new byte[avg];
					zipEntry = new ZipEntry(Path.GetFileName(files[i]));
					zipEntry.DateTime = DateTime.MinValue;
					zipEntry.Size = fileStream.Length;
					s.PutNextEntry(zipEntry);
					for (int j = 0; j < fileStream.Length; j += avg)
					{
						if (j + avg > fileStream.Length)
						{
							array = new byte[fileStream.Length - j];
						}
						fileStream.Read(array, 0, array.Length);
						s.Write(array, 0, array.Length);
					}
				}
			}
			try
			{
				zipEntry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));
				s.PutNextEntry(zipEntry);
				s.Flush();
				string[] files = Directory.GetFiles(FolderToZip);
				for (int i = 0; i < files.Length; i++)
				{
					fileStream = File.OpenRead(files[i]);
					byte[] array = new byte[avg];
					zipEntry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(files[i])));
					zipEntry.DateTime = DateTime.MinValue;
					zipEntry.Size = fileStream.Length;
					s.PutNextEntry(zipEntry);
					for (int j = 0; j < fileStream.Length; j += avg)
					{
						if (j + avg > fileStream.Length)
						{
							array = new byte[fileStream.Length - j];
						}
						fileStream.Read(array, 0, array.Length);
						s.Write(array, 0, array.Length);
					}
				}
			}
			catch (Exception e)
			{
				//Utilitys.ShowError(e.ToString());
				result = false;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
				if (zipEntry != null)
				{
					zipEntry = null;
				}
				GC.Collect();
				GC.Collect(1);
			}
			string[] directories = Directory.GetDirectories(FolderToZip);
			for (int k = 0; k < directories.Length; k++)
			{
				if (!ZipFileDictory(directories[k], s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
				{
					return false;
				}
			}
			return result;
		}
	}
}
