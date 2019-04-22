namespace SkyCES.EntLib
{
    using System;
    using System.IO;

    public sealed class AdClass
    {
        private SkyCES.EntLib.AdType adType;
        private string display = string.Empty;
        private DateTime endDate = DateTime.Now;
        private string fileName = string.Empty;
        private int height;
        private int isEnabled;
        private DateTime startDate = DateTime.Now;
        private string title = string.Empty;
        private string url = string.Empty;
        private int width;

        public void MakeAdFile()
        {
            string str = string.Empty;
            if (this.isEnabled != 1)
            {
                str = str + "document.write('广告被关闭')";
            }
            else
            {
                string str2 = str + "var myDate=new Date();\r\n" + "var nowDate=myDate.getFullYear()+\"-\"+(myDate.getMonth()+1)+\"-\"+(myDate.getDate()+1);\r\n";
                str = (str2 + "if(compareDate(nowDate,\"" + this.StartDate.ToString("yyyy-MM-dd") + "\") && compareDate(\"" + this.EndDate.AddDays(1.0).ToString("yyyy-MM-dd") + "\",nowDate))\r\n") + "{\r\n";
                switch (this.adType)
                {
                    case SkyCES.EntLib.AdType.Text:
                        str2 = str;
                        str2 = str2 + "document.write('<div style=\"width:" + this.Width.ToString() + "px;height:" + this.Height.ToString() + "px\">');\r\n";
                        str = (str2 + "document.write('<a href=\"" + this.Url + "\"  target=\"_blank\">" + this.Display + "</a>');\r\n") + "document.write('</div>');\r\n";
                        break;

                    case SkyCES.EntLib.AdType.Picture:
                        str2 = str;
                        str = str2 + "document.write('<a href=\"" + this.Url + "\" target=\"_blank\" title=" + this.Title + "><img src=\"" + this.Display + "\"  border=\"0\" width=\"" + this.Width.ToString() + "\" height=\"" + this.Height.ToString() + "\"></a>');\r\n";
                        break;

                    case SkyCES.EntLib.AdType.Flash:
                        str2 = str;
                        str2 = ((str2 + "document.write('<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0\" width=\"" + this.Width.ToString() + "\" height=\"" + this.Height.ToString() + "\">');\r\n") + "document.write('<param name=\"movie\" value=\"" + this.Display + "\">');\r\n") + "document.write('<param name=\"quality\" value=\"high\">');\r\n";
                        str = (str2 + "document.write('<embed src=\"" + this.Display + "\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"" + this.Width.ToString() + "\" height=\"" + this.Height.ToString() + "\"></embed>');\r\n") + "document.write('</object>')\r\n";
                        break;

                    case SkyCES.EntLib.AdType.Code:
                        str2 = str;
                        str = ((str2 + "document.write('<div style=\"width:" + this.Width.ToString() + "px;height:" + this.Height.ToString() + "px\">');\r\n") + "document.write('" + this.Display + "');\r\n") + "document.write('</div>');\r\n";
                        break;
                }
                str = ((((((((((str + "}\r\n" + "else\r\n") + "{\r\n" + "document.write('广告过期');\r\n") + "}\r\n" + "function compareDate(dateOne,dateTwo)\r\n") + "{ \r\n" + "var monthOne = dateOne.substring(5,dateOne.lastIndexOf (\"-\"))\r\n") + "var dayOne = dateOne.substring(dateOne.length,dateOne.lastIndexOf (\"-\")+1)\r\n" + "var yearOne = dateOne.substring(0,dateOne.indexOf (\"-\"))\r\n") + "var monthTwo = dateTwo.substring(5,dateTwo.lastIndexOf (\"-\"))\r\n" + "var dayTwo = dateTwo.substring(dateTwo.length,dateTwo.lastIndexOf (\"-\")+1)\r\n") + "var yearTwo = dateTwo.substring(0,dateTwo.indexOf (\"-\"))\r\n" + "if (Date.parse(monthOne+\" / \"+dayOne+\" / \"+yearOne) >Date.parse(monthTwo+\"/\"+dayTwo+\"/\"+yearTwo))\r\n") + "{\r\n" + "return true;\r\n") + "}\r\n" + "else\r\n") + "{\r\n" + "return false;\r\n") + "}\r\n" + "}\r\n";
            }
            using (StreamWriter writer = File.CreateText(this.FileName))
            {
                writer.Write(str);
                writer.Close();
            }
        }

        public SkyCES.EntLib.AdType AdType
        {
            get
            {
                return this.adType;
            }
            set
            {
                this.adType = value;
            }
        }

        public string Display
        {
            get
            {
                return this.display;
            }
            set
            {
                this.display = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public int IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

