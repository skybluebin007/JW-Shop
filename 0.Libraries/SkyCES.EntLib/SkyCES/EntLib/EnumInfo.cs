namespace SkyCES.EntLib
{
    using System;

    public sealed class EnumInfo
    {
        private string chineseName = string.Empty;
        private string englishName = string.Empty;
        private int value = 0;

        public string ChineseName
        {
            get
            {
                return this.chineseName;
            }
            set
            {
                this.chineseName = value;
            }
        }

        public string EnglishName
        {
            get
            {
                return this.englishName;
            }
            set
            {
                this.englishName = value;
            }
        }

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}

