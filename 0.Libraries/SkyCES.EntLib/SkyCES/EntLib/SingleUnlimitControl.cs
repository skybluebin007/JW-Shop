namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public sealed class SingleUnlimitControl : WebControl
    {
        private SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(this.singleUnlimitClass.ShowContent());
        }

        public string ClassID
        {
            get
            {
                return this.singleUnlimitClass.ClassID;
            }
            set
            {
                this.singleUnlimitClass.ClassID = value;
            }
        }

        public List<UnlimitClassInfo> DataSource
        {
            get
            {
                return this.singleUnlimitClass.DataSource;
            }
            set
            {
                this.singleUnlimitClass.DataSource = value;
            }
        }

        public int FatherID
        {
            get
            {
                return this.singleUnlimitClass.FatherID;
            }
        }

        public string FunctionName
        {
            set
            {
                this.singleUnlimitClass.FunctionName = value;
            }
        }

        public string Prefix
        {
            set
            {
                this.singleUnlimitClass.Prefix = value;
            }
        }

        public int RootID
        {
            get
            {
                return this.singleUnlimitClass.RootID;
            }
        }
    }
}

