namespace SkyCES.EntLib
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;

    [DefaultProperty(""), ToolboxData("<{0}:Page runat=server></{0}:Page>")]
    public abstract class BasePager : Control
    {
        private SkyCES.EntLib.BasePagerClass basePagerClass;

        protected BasePager()
        {
        }

        protected SkyCES.EntLib.BasePagerClass BasePagerClass
        {
            get
            {
                return this.basePagerClass;
            }
            set
            {
                this.basePagerClass = value;
            }
        }

        [DefaultValue(""), Bindable(true), Category("Appearance")]
        public int Count
        {
            get
            {
                return this.basePagerClass.Count;
            }
            set
            {
                this.basePagerClass.Count = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public int CurrentPage
        {
            get
            {
                return this.basePagerClass.CurrentPage;
            }
            set
            {
                this.basePagerClass.CurrentPage = value;
            }
        }

        [Bindable(true), DefaultValue(""), Category("Appearance")]
        public bool DisCount
        {
            get
            {
                return this.basePagerClass.DisCount;
            }
            set
            {
                this.basePagerClass.DisCount = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public bool FirstLastType
        {
            get
            {
                return this.basePagerClass.FirstLastType;
            }
            set
            {
                this.basePagerClass.FirstLastType = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string FirstPage
        {
            get
            {
                return this.basePagerClass.FirstPage;
            }
            set
            {
                this.basePagerClass.FirstPage = value;
            }
        }

        [Bindable(true), DefaultValue(""), Category("Appearance")]
        public string LastPage
        {
            get
            {
                return this.basePagerClass.LastPage;
            }
            set
            {
                this.basePagerClass.LastPage = value;
            }
        }

        [DefaultValue(""), Bindable(true), Category("Appearance")]
        public bool ListType
        {
            get
            {
                return this.basePagerClass.ListType;
            }
            set
            {
                this.basePagerClass.ListType = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public string NextPage
        {
            get
            {
                return this.basePagerClass.NextPage;
            }
            set
            {
                this.basePagerClass.NextPage = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public bool NumType
        {
            get
            {
                return this.basePagerClass.NumType;
            }
            set
            {
                this.basePagerClass.NumType = value;
            }
        }

        public int PageCount
        {
            get
            {
                return this.basePagerClass.PageCount;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public int PageSize
        {
            get
            {
                return this.basePagerClass.PageSize;
            }
            set
            {
                this.basePagerClass.PageSize = value;
            }
        }

        [Category("Appearance"), DefaultValue(""), Bindable(true)]
        public int PageStep
        {
            get
            {
                return this.basePagerClass.PageStep;
            }
            set
            {
                this.basePagerClass.PageStep = value;
            }
        }

        [Category("Appearance"), DefaultValue(""), Bindable(true)]
        public bool PrenextType
        {
            get
            {
                return this.basePagerClass.PrenextType;
            }
            set
            {
                this.basePagerClass.PrenextType = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public string PreviewPage
        {
            get
            {
                return this.basePagerClass.PreviewPage;
            }
            set
            {
                this.basePagerClass.PreviewPage = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string URL
        {
            get
            {
                return this.basePagerClass.URL;
            }
            set
            {
                this.basePagerClass.URL = value;
            }
        }


        public string PagerCSS
        {
            get
            {
                return this.basePagerClass.PagerCSS;
            }
            set
            {
                this.basePagerClass.PagerCSS = value;
            }
        }

        public int ShowType
        {
            get
            {
                return this.basePagerClass.ShowType;
            }
            set
            {
                this.basePagerClass.ShowType = value;
            }
        }

        public string CurrentCSS
        {
            get
            {
                return this.basePagerClass.CurrentCSS;
            }
            set
            {
                this.basePagerClass.CurrentCSS = value;
            }
        }

        public string GoPageFun
        {
            get
            {
                return this.basePagerClass.GoPageFun;
            }
            set
            {
                this.basePagerClass.GoPageFun = value;
            }
        }
    }
}

