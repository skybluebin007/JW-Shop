namespace SkyCES.EntLib
{
    using System;

    public class ProcessingEventArgs : EventArgs
    {
        private int currentPage = 0;
        private int pageCount = 0;

        public ProcessingEventArgs(int pageCount, int currentPage)
        {
            this.pageCount = pageCount;
            this.currentPage = currentPage;
        }

        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }
        }

        public int PageCount
        {
            get
            {
                return this.pageCount;
            }
        }
    }
}

