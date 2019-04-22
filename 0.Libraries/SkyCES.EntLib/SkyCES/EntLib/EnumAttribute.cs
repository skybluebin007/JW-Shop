namespace SkyCES.EntLib
{
    using System;

    public class EnumAttribute : Attribute
    {
        private string chineseName;

        public EnumAttribute(string chineseName)
        {
            this.chineseName = chineseName;
        }

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
    }
}

