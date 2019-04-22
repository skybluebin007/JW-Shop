namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public sealed class FileHelper
    {
        public static void CopyDirectory(string fromDirectory, string toDirectroy)
        {
            if (Directory.Exists(fromDirectory))
            {
                if (!Directory.Exists(toDirectroy))
                {
                    Directory.CreateDirectory(toDirectroy);
                }
                string[] files = Directory.GetFiles(fromDirectory);
                if (files.Length > 0)
                {
                    foreach (string str in files)
                    {
                        string destFileName = toDirectroy + str.Substring(str.LastIndexOf(@"\"));
                        File.Copy(str, destFileName);
                    }
                }
                string[] directories = Directory.GetDirectories(fromDirectory);
                foreach (string str3 in directories)
                {
                    string str4 = toDirectroy + str3.Substring(str3.LastIndexOf(@"\"));
                    CopyDirectory(str3, str4);
                }
            }
        }

        public static string CreateFileName(FileNameType types, string OriginalFileName, string Extension)
        {
            return CreateFileName(0x3e8, types, OriginalFileName, Extension);
        }

        public static string CreateFileName(int Seed, FileNameType types, string OriginalFileName, string Extension)
        {
            Random random = new Random();
            switch (types)
            {
                case FileNameType.Date:
                    return (DateTime.Now.ToString("yyyyMMddhhmmss") + random.Next(Seed) + Extension);

                case FileNameType.FileNameAndDate:
                    return string.Concat(new object[] { OriginalFileName.Replace(Extension, "-"), DateTime.Now.ToString("yyyyMMddhhmmss"), random.Next(Seed), Extension });

                case FileNameType.MD516:
                    return (StringHelper.MD516(Guid.NewGuid().ToString()) + Extension);

                case FileNameType.MD532:
                    return (StringHelper.MD532(Guid.NewGuid().ToString()) + Extension);

                case FileNameType.Guid:
                    return (Guid.NewGuid().ToString() + Extension);

                case FileNameType.OriginalFileName:
                    return OriginalFileName;
            }
            return (Guid.NewGuid().ToString() + Extension);
        }

        public static void DeleteDirectory(List<string> directory)
        {
            if (directory != null)
            {
                foreach (string str in directory)
                {
                    try
                    {
                        Directory.Delete(ServerHelper.MapPath(str), true);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void DeleteFile(List<string> strFileName)
        {
            if (strFileName != null)
            {
                foreach (string str in strFileName)
                {
                    try
                    {
                        File.Delete(ServerHelper.MapPath(str));
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public static string GetFileName(string content)
        {
            string fullFileName = GetFullFileName(content);
            if ((fullFileName != string.Empty) && (content.IndexOf(".") > -1))
            {
                fullFileName = fullFileName.Substring(0, fullFileName.LastIndexOf("."));
            }
            return fullFileName;
        }

        public static string GetFolderName(string content)
        {
            string str = string.Empty;
            if (content.LastIndexOf("/") > -1)
            {
                str = content.Substring(0, content.LastIndexOf("/")) + "/";
            }
            return str;
        }

        public static string GetFullFileName(string content)
        {
            string str = content;
            if ((str != string.Empty) && (content.IndexOf(".") > -1))
            {
                string folderName = GetFolderName(content);
                if (folderName != string.Empty)
                {
                    str = str.Replace(folderName, string.Empty);
                }
            }
            return str;
        }

        public static string GetKeys()
        {
            return StringHelper.MD532(DateTime.Now.Ticks.ToString() + Guid.NewGuid().ToString() + new Random().Next(0x2710));
        }

        public static List<DirectoryInfo> ListDirectory(string rootDirectory)
        {
            List<DirectoryInfo> list = new List<DirectoryInfo>();
            if (Directory.Exists(rootDirectory))
            {
                DirectoryInfo[] directories = new DirectoryInfo(rootDirectory).GetDirectories();
                for (int i = 0; i < directories.Length; i++)
                {
                    list.Add(directories[i]);
                }
            }
            return list;
        }

        public static List<FileInfo> ListDirectory(string rootDirectory, string allowFile)
        {
            List<FileInfo> list = new List<FileInfo>();
            if (Directory.Exists(rootDirectory))
            {
                FileSystemInfo[] fileSystemInfos = new DirectoryInfo(rootDirectory).GetFileSystemInfos();
                foreach (FileSystemInfo info2 in fileSystemInfos)
                {
                    if (info2 is FileInfo)
                    {
                        if (allowFile == string.Empty)
                        {
                            list.Add((FileInfo) info2);
                        }
                        else if (allowFile.ToLower().IndexOf("|" + info2.Extension.ToString().ToLower() + "|") > -1)
                        {
                            list.Add((FileInfo) info2);
                        }
                    }
                    if (info2 is DirectoryInfo)
                    {
                        list.AddRange(ListDirectory(rootDirectory + "/" + info2.Name, allowFile));
                    }
                }
            }
            return list;
        }

        public static List<FileInfo> ListFile(string rootDirectory)
        {
            List<FileInfo> list = new List<FileInfo>();
            if (Directory.Exists(rootDirectory))
            {
                FileInfo[] files = new DirectoryInfo(rootDirectory).GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    list.Add(files[i]);
                }
            }
            return list;
        }

        public static string ReadFileLength(long fileLength)
        {
            string str = string.Empty;
            if (fileLength <= 0L)
            {
                return str;
            }
            if ((fileLength / 0x40000000L) >= 1L)
            {
                return (Math.Round((decimal) (fileLength / 1073741824M), 2).ToString() + "GB");
            }
            if ((fileLength / 0x100000L) >= 1L)
            {
                return (Math.Round((decimal) (fileLength / 1048576M), 2).ToString() + "MB");
            }
            if ((fileLength / 0x400L) >= 1L)
            {
                return (Math.Round((decimal) (fileLength / 1024M), 2).ToString() + "KB");
            }
            return (fileLength.ToString() + "B");
        }

        public static bool SafeDirectoryName(string directoryName)
        {
            string pattern = @"^\w+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(directoryName);
        }

        public static bool SafeFullDirectoryName(string directoryName)
        {
            string pattern = @"^[\w\/]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(directoryName);
        }
    }
}

