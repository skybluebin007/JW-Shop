namespace SkyCES.EntLib
{
    using System;

    public class PreWriteEventArgs : EventArgs
    {
        private string content = string.Empty;

        public PreWriteEventArgs(string content)
        {
            this.content = content;
        }

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
    }
}

