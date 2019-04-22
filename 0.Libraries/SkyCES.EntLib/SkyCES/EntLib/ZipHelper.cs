namespace SkyCES.EntLib
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip;
    using System;
    using System.IO;

    public sealed class ZipHelper
    {
        public static void UnZip(string zipfilepath, string unzippath)
        {
            ZipEntry entry;
            ZipInputStream stream = new ZipInputStream(File.OpenRead(zipfilepath));
            while ((entry = stream.GetNextEntry()) != null)
            {
                bool flag;
                string name = entry.Name;
                if (name != string.Empty)
                {
                    name = entry.Name.Substring(entry.Name.IndexOf("/"));
                }
                string directoryName = Path.GetDirectoryName(unzippath);
                if (!(Path.GetFileName(name) != string.Empty))
                {
                    continue;
                }
                if (entry.CompressedSize == 0L)
                {
                    break;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(unzippath + name));
                FileStream stream2 = File.Create(unzippath + name);
                int count = 0x800;
                byte[] buffer = new byte[0x800];
                goto Label_00F6;
            Label_00C3:
                count = stream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                {
                    stream2.Write(buffer, 0, count);
                }
                else
                {
                    goto Label_00FB;
                }
            Label_00F6:
                flag = true;
                goto Label_00C3;
            Label_00FB:
                stream2.Close();
            }
            stream.Close();
        }

        private static void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile = strFile + Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            string[] fileSystemEntries = Directory.GetFileSystemEntries(strFile);
            foreach (string str in fileSystemEntries)
            {
                if (Directory.Exists(str))
                {
                    zip(str, s, staticFile);
                }
                else
                {
                    FileStream stream = File.OpenRead(str);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    int startIndex = staticFile.LastIndexOf(".") + 1;
                    ZipEntry entry = new ZipEntry(str.Substring(startIndex)) {
                        DateTime = DateTime.Now,
                        Size = stream.Length
                    };
                    stream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public static void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile = strFile + Path.DirectorySeparatorChar;
            }
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6);
            zip(strFile, s, strZip);
            s.Finish();
            s.Close();
        }
    }
}

