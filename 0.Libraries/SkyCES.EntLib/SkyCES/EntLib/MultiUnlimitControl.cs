namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public sealed class MultiUnlimitControl : Control
    {
        private MultiUnlimitClass multiUnlimitClass = new MultiUnlimitClass();

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(this.multiUnlimitClass.ShowContent());
        }

        public string ClassIDList
        {
            get
            {
                return this.multiUnlimitClass.ClassIDList;
            }
            set
            {
                this.multiUnlimitClass.ClassIDList = value;
            }
        }

        public List<UnlimitClassInfo> DataSource
        {
            set
            {
                this.multiUnlimitClass.DataSource = value;
            }
        }

        public string Prefix
        {
            set
            {
                this.multiUnlimitClass.Prefix = value;
            }
        }
    }
}

