namespace SkyCES.EntLib
{
    using System;
    using System.IO;
    using System.Text;

    public abstract class FileLog : LogBase
    {
        private string fileExtension;
        private long fileSize;
        private string folderName;
        private SkyCES.EntLib.LogFileNameType logFileNameType;
        private bool needMonthFolder;

        public FileLog(string folderName)
        {
            this.folderName = string.Empty;
            this.needMonthFolder = true;
            this.logFileNameType = SkyCES.EntLib.LogFileNameType.ByDay;
            this.fileSize = -9223372036854775808L;
            this.fileExtension = string.Empty;
            this.folderName = folderName;
            this.CreateFolder();
        }

        public FileLog(string folderName, bool needMonthFolder, SkyCES.EntLib.LogFileNameType logFileNameType, long fileSize)
        {
            this.folderName = string.Empty;
            this.needMonthFolder = true;
            this.logFileNameType = SkyCES.EntLib.LogFileNameType.ByDay;
            this.fileSize = -9223372036854775808L;
            this.fileExtension = string.Empty;
            this.folderName = folderName;
            this.needMonthFolder = needMonthFolder;
            this.logFileNameType = logFileNameType;
            this.fileSize = fileSize;
            this.CreateFolder();
        }

        private void CreateFolder()
        {
            if (!Directory.Exists(this.folderName))
            {
                Directory.CreateDirectory(this.folderName);
            }
            if (!(Directory.Exists(this.folderName + DateTime.Now.ToString("yyyy-MM-dd") + @"\") || !this.needMonthFolder))
            {
                Directory.CreateDirectory(this.folderName + DateTime.Now.ToString("yyyy-MM-dd") + @"\");
            }
        }

        protected string GetFileName(string message)
        {
            string folderName = this.folderName;
            if (this.needMonthFolder)
            {
                folderName = folderName + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
            }
            if (this.logFileNameType == SkyCES.EntLib.LogFileNameType.ByDay)
            {
                folderName = folderName + DateTime.Now.ToString("yyyy-MM-dd");
                string path = string.Empty;
                bool flag = false;
                int num = 0;
                while (!flag)
                {
                    path = (num > 0) ? (folderName + "-" + num.ToString() + this.fileExtension) : (folderName + this.fileExtension);
                    if (File.Exists(path) && (this.FileSize != -9223372036854775808L))
                    {
                        FileInfo info = new FileInfo(path);
                        long num2 = info.Length + Encoding.Default.GetBytes(message).Length;
                        if (num2 > (this.FileSize * 0x400L))
                        {
                            num++;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                return path;
            }
            return (folderName + Guid.NewGuid().ToString() + this.fileExtension);
        }

        protected string FileExtension
        {
            get
            {
                return this.fileExtension;
            }
            set
            {
                this.fileExtension = value;
            }
        }

        public long FileSize
        {
            get
            {
                return this.fileSize;
            }
            set
            {
                this.fileSize = value;
            }
        }

        public string FolderName
        {
            get
            {
                return this.folderName;
            }
            set
            {
                this.folderName = value;
            }
        }

        public SkyCES.EntLib.LogFileNameType LogFileNameType
        {
            get
            {
                return this.logFileNameType;
            }
            set
            {
                this.logFileNameType = value;
            }
        }

        public bool NeedMonthFolder
        {
            get
            {
                return this.needMonthFolder;
            }
            set
            {
                this.needMonthFolder = value;
            }
        }
    }
}

