namespace SkyCES.EntLib
{
    using System;

    public abstract class DataBaseLog : LogBase
    {
        private string connectionString = string.Empty;

        protected DataBaseLog()
        {
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }
    }
}

