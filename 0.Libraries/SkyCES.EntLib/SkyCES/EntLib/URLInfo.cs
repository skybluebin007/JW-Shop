namespace SkyCES.EntLib
{
    using System;

    public class URLInfo
    {
        private int id;
        private bool isEffect;
        private string realPath;
        private string vitualPath;

        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public bool IsEffect
        {
            get
            {
                return this.isEffect;
            }
            set
            {
                this.isEffect = value;
            }
        }

        public string RealPath
        {
            get
            {
                return this.realPath;
            }
            set
            {
                this.realPath = value;
            }
        }

        public string VitualPath
        {
            get
            {
                return this.vitualPath;
            }
            set
            {
                this.vitualPath = value;
            }
        }
    }
}

