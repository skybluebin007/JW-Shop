namespace SkyCES.EntLib
{
    using System;
    using System.Data;
    using System.Data.OleDb;

    public abstract class AccessPagerClass
    {
        private SkyCES.EntLib.AccessCondition accessCondition = new SkyCES.EntLib.AccessCondition();
        private int currentPage = 1;
        private string fields;
        private string orderField = "ID";
        private SkyCES.EntLib.OrderType orderType = SkyCES.EntLib.OrderType.Desc;
        private int pageSize = 10;
        private string tableName;

        protected AccessPagerClass()
        {
        }

        public abstract DataTable ExecuteDataTable();
        public abstract OleDbDataReader ExecuteReader();
        protected string PrepareCountSQL()
        {
            string str = "SELECT COUNT(*) FROM " + this.TableName;
            if (this.accessCondition.ToString() != string.Empty)
            {
                str = "SELECT COUNT(*) FROM " + this.TableName + " WHERE " + this.accessCondition.ToString();
            }
            return str;
        }

        protected string PrepareSQL()
        {
            string str2 = this.accessCondition.ToString();
            string str3 = string.Empty;
            string str4 = string.Empty;
            if (str2 != string.Empty)
            {
                str3 = " WHERE " + str2;
                str4 = " AND " + str2;
            }
            string orderField = this.orderField;
            if (orderField.IndexOf('.') > -1)
            {
                orderField = orderField.Substring(orderField.IndexOf('.'));
            }
            string str6 = string.Empty;
            string str7 = string.Empty;
            if (this.orderType == SkyCES.EntLib.OrderType.Desc)
            {
                str7 = "< (SELECT MIN (";
                str6 = " ORDER BY " + this.orderField.Replace(",", " DESC ,") + " DESC";
            }
            else
            {
                str7 = "> (SELECT MAX (";
                str6 = " ORDER BY " + this.orderField.Replace(",", " ASC ,") + " ASC";
            }
            if (this.orderField.IndexOf(',') == -1)
            {
                if (this.currentPage == 1)
                {
                    return string.Concat(new object[] { "SELECT TOP ", this.pageSize.ToString(), " ", this.fields, " FROM ", this.tableName, str3, ' ', str6 });
                }
                object[] objArray = new object[] { 
                    "SELECT TOP ", this.pageSize.ToString(), " ", this.fields, " FROM ", this.tableName, " WHERE ", this.orderField, str7, orderField, ") FROM (SELECT TOP ", (this.pageSize * (this.currentPage - 1)).ToString(), " ", this.orderField, " FROM ", this.tableName, 
                    str3, str6, ") TEMP) ", str4, ' ', str6
                 };
                return string.Concat(objArray);
            }
            if (this.currentPage == 1)
            {
                return ("SELECT TOP " + this.pageSize.ToString() + " " + this.fields + " FROM " + this.tableName + str3 + str6);
            }
            string[] strArray = new string[] { "SELECT TOP ", this.pageSize.ToString(), " ", this.fields, " FROM ", this.tableName, " WHERE [ID] NOT IN (SELECT TOP ", (this.pageSize * (this.currentPage - 1)).ToString(), "  [ID] FROM ", this.tableName, str3, str6, ") ", str4, str6 };
            return string.Concat(strArray);
        }

        public SkyCES.EntLib.AccessCondition AccessCondition
        {
            get
            {
                return this.accessCondition;
            }
            set
            {
                this.accessCondition = value;
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
                if (value > 0)
                {
                    this.currentPage = value;
                }
            }
        }

        public string Fields
        {
            get
            {
                return this.fields;
            }
            set
            {
                this.fields = value;
            }
        }

        public string OrderField
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        public SkyCES.EntLib.OrderType OrderType
        {
            get
            {
                return this.orderType;
            }
            set
            {
                this.orderType = value;
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

        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }
    }
}

