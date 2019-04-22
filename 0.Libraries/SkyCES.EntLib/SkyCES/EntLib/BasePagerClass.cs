namespace SkyCES.EntLib
{
    using System;
    using System.Web;

    public abstract class BasePagerClass
    {
        private int count;
        private int currentPage = 1;
        private bool disCount = true;
        private int endPage;
        private string firstPage = "第一页";
        private string lastPage = "最后页";
        private bool listType = true;
        private string nextPage = "下一页";
        private bool numType = true;
        private int pageSize = 10;
        private int pageStep = 4;
        private bool prenextType = true;
        private string previewPage = "上一页";
        private int startPage;
        private string url = string.Empty;
        private bool firstLastType = true;
        private string pagerCSS = "pageCss";
        private int showType = 1;
        private  string currentCSS = "current";
        private string goPageFun = "goPage";

        protected BasePagerClass()
        {
        }

        public void CountStartEndPage()
        {
            if (this.PageCount <= ((2 * this.pageStep) + 1))
            {
                this.startPage = 1;
                this.endPage = this.PageCount;
            }
            else
            {
                if (this.currentPage > this.pageStep)
                {
                    this.startPage = this.currentPage - this.pageStep;
                }
                else
                {
                    this.startPage = 1;
                }
                this.endPage = this.startPage + (2 * this.pageStep);
                if ((this.startPage + (2 * this.pageStep)) > this.PageCount)
                {
                    this.startPage = this.PageCount - (2 * this.pageStep);
                    this.endPage = this.PageCount;
                }
            }
        }

        public abstract string ShowPage();
        public abstract string ShowLoadPage();
        public abstract string ShowLoadPages();
        public abstract string ShowPages();
        public abstract string ShowHelpPage();
        public int Count
        {
            get
            {
                return this.count;
            }
            set
            {
                this.count = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }
            set
            {
                this.currentPage = value;
            }
        }

        public bool DisCount
        {
            get
            {
                return this.disCount;
            }
            set
            {
                this.disCount = value;
            }
        }

        public int EndPage
        {
            get
            {
                return this.endPage;
            }
        }

        public string FirstPage
        {
            get
            {
                return this.firstPage;
            }
            set
            {
                this.firstPage = value;
            }
        }

        public string LastPage
        {
            get
            {
                return this.lastPage;
            }
            set
            {
                this.lastPage = value;
            }
        }

        public bool ListType
        {
            get
            {
                return this.listType;
            }
            set
            {
                this.listType = value;
            }
        }

        public string NextPage
        {
            get
            {
                return this.nextPage;
            }
            set
            {
                this.nextPage = value;
            }
        }

        public bool NumType
        {
            get
            {
                return this.numType;
            }
            set
            {
                this.numType = value;
            }
        }

        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.Count / (decimal)this.PageSize);
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }

        public int PageStep
        {
            get
            {
                return this.pageStep;
            }
            set
            {
                this.pageStep = value;
            }
        }

        public bool PrenextType
        {
            get
            {
                return this.prenextType;
            }
            set
            {
                this.prenextType = value;
            }
        }

        public bool FirstLastType
        {
            get
            {
                return this.firstLastType;
            }
            set
            {
                this.firstLastType = value;
            }
        }

        public string PreviewPage
        {
            get
            {
                return this.previewPage;
            }
            set
            {
                this.previewPage = value;
            }
        }

        public int StartPage
        {
            get
            {
                return this.startPage;
            }
        }

        public string URL
        {
            get
            {
                if (this.url != string.Empty)
                {
                    return this.url;
                }
                string rawUrl = HttpContext.Current.Request.RawUrl;
                if (rawUrl.ToLower().IndexOf("&page=") > -1)
                {
                    return (rawUrl.Substring(0, rawUrl.ToLower().IndexOf("&page=")) + "&Page=$Page");
                }
                if (rawUrl.ToLower().IndexOf("?page=") > -1)
                {
                    return (rawUrl.Substring(0, rawUrl.ToLower().IndexOf("?page=")) + "?Page=$Page");
                }
                if (rawUrl.ToLower().IndexOf("?") > -1)
                {
                    if (rawUrl.EndsWith("?"))
                    {
                        return (rawUrl + "Page=$Page");
                    }
                    return (rawUrl + "&Page=$Page");
                }
                return (rawUrl + "?Page=$Page");
            }
            set
            {
                this.url = value;
            }
        }

        public string PagerCSS
        {
            get
            {
                return this.pagerCSS;
            }
            set
            {
                this.pagerCSS = value;
            }
        }

        public int ShowType
        {
            get
            {
                return this.showType;
            }
            set
            {
                this.showType = value;
            }
        }
        
        public string CurrentCSS {
            get
            {
                return this.currentCSS;
            }
            set
            {
                this.currentCSS = value;
            } }

        public string GoPageFun
        {
            get
            {
                return this.goPageFun;
            }
            set
            {
                this.goPageFun = value;
            }
        }
    }
}

