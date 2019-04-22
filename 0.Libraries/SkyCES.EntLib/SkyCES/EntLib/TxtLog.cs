namespace SkyCES.EntLib
{
    using System;
    using System.IO;

    public class TxtLog : FileLog
    {
        public TxtLog(string folderName) : base(folderName)
        {
            base.FileExtension = ".txt";
        }

        public TxtLog(string folderName, bool needMonthFolder, LogFileNameType logFileNameType, long fileSize) : base(folderName, needMonthFolder, logFileNameType, fileSize)
        {
            base.FileExtension = ".txt";
        }

        public override void Write(string message)
        {
            StreamWriter writer = File.AppendText(base.GetFileName(message));
            writer.WriteLine(message);
            writer.Flush();
            writer.Close();
        }
    }
}

