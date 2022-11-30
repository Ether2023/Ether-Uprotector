using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace OZ_IL2CPP_GUI
{
	class ZipUtils
    {
		public void ZipFile(string strFile, string strZip)
		{
			if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
			{
				strFile += Path.DirectorySeparatorChar;
			}
			ZipOutputStream outstream = new ZipOutputStream(File.Create(strZip));
			outstream.SetLevel(6);
			ZipCompress(strFile, outstream, strFile);
			outstream.Finish();
			outstream.Close();
		}

		void ZipCompress(string strFile, ZipOutputStream outstream, string staticFile)
		{
			if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
			{
				strFile += Path.DirectorySeparatorChar;
			}
			Crc32 crc = new Crc32();
			//获取指定目录下所有文件和子目录文件名称
			string[] filenames = Directory.GetFileSystemEntries(strFile);
			//遍历文件
			foreach (string file in filenames)
			{
				if (Directory.Exists(file))
				{
					ZipCompress(file, outstream, staticFile);
				}
				//否则，直接压缩文件
				else
				{
					//打开文件
					FileStream fs = File.OpenRead(file);
					//定义缓存区对象
					byte[] buffer = new byte[fs.Length];
					//通过字符流，读取文件
					fs.Read(buffer, 0, buffer.Length);
					//得到目录下的文件（比如:D:\Debug1\test）,test
					string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
					ZipEntry entry = new ZipEntry(tempfile);
					entry.DateTime = DateTime.Now;
					entry.Size = fs.Length;
					fs.Close();
					crc.Reset();
					crc.Update(buffer);
					entry.Crc = crc.Value;
					outstream.PutNextEntry(entry);
					//写文件
					outstream.Write(buffer, 0, buffer.Length);
				}
			}
		}

		public string UnZipFile(string TargetFile, string fileDir)
		{
			string rootFile = "";
			try
			{
				if (!Directory.Exists(fileDir))
				{
					Directory.CreateDirectory(fileDir);
				}
				//读取压缩文件（zip文件），准备解压缩
				ZipInputStream inputstream = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
				ZipEntry entry;
				string path = fileDir;
				//解压出来的文件保存路径
				string rootDir = "";
				//根目录下的第一个子文件夹的名称
				while ((entry = inputstream.GetNextEntry()) != null)
				{
					rootDir = Path.GetDirectoryName(entry.Name);
					//得到根目录下的第一级子文件夹的名称
					if (rootDir.IndexOf("\\") >= 0)
					{
						rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
					}
					string dir = Path.GetDirectoryName(entry.Name);
					//得到根目录下的第一级子文件夹下的子文件夹名称
					string fileName = Path.GetFileName(entry.Name);
					//根目录下的文件名称
					if (dir != "")
					{
						//创建根目录下的子文件夹，不限制级别
						if (!Directory.Exists(fileDir + "\\" + dir))
						{
							path = fileDir + "\\" + dir;
							//在指定的路径创建文件夹
							Directory.CreateDirectory(path);
						}
					}
					else if (dir == "" && fileName != "")
					{
						//根目录下的文件
						path = fileDir;
						rootFile = fileName;
					}
					else if (dir != "" && fileName != "")
					{
						//根目录下的第一级子文件夹下的文件
						if (dir.IndexOf("\\") > 0)
						{
							//指定文件保存路径
							path = fileDir + "\\" + dir;
						}
					}
					if (dir == rootDir)
					{
						//判断是不是需要保存在根目录下的文件
						path = fileDir + "\\" + rootDir;
					}

					//以下为解压zip文件的基本步骤
					//基本思路：遍历压缩文件里的所有文件，创建一个相同的文件
					if (fileName != String.Empty)
					{
						FileStream fs = File.Create(path + "\\" + fileName);
						int size = 2048;
						byte[] data = new byte[2048];
						while (true)
						{
							size = inputstream.Read(data, 0, data.Length);
							if (size > 0)
							{
								fs.Write(data, 0, size);
							}
							else
							{
								break;
							}
						}
						fs.Close();
					}
				}
				inputstream.Close();
				return rootFile;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}

	[Obsolete]
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
				Utilitys.ShowError(e.ToString());
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
				Utilitys.ShowError(e.ToString());
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
