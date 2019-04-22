namespace SkyCES.EntLib
{
    using System;

    public class MailInfo
    {
        private string content = string.Empty;
        private bool isBodyHtml;
        private string passowrd = string.Empty;
        private string server = string.Empty;
        private int serverPort;
        private string title = string.Empty;
        private string toEmail;
        private string userName = string.Empty;

        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        public bool IsBodyHtml
        {
            get
            {
                return this.isBodyHtml;
            }
            set
            {
                this.isBodyHtml = value;
            }
        }

        public string Password
        {
            get
            {
                return this.passowrd;
            }
            set
            {
                this.passowrd = value;
            }
        }

        public string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        public int ServerPort
        {
            get
            {
                return this.serverPort;
            }
            set
            {
                this.serverPort = value;
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

        public string ToEmail
        {
            get
            {
                return this.toEmail;
            }
            set
            {
                this.toEmail = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }
    }
}

