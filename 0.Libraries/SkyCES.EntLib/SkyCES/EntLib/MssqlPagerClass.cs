namespace SkyCES.EntLib
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public abstract class MssqlPagerClass
    {
        private int currentPage = 1;
        private string fields;
        private SkyCES.EntLib.MssqlCondition mssqlCondition = new SkyCES.EntLib.MssqlCondition();
        private string orderField = "ID";
        private SkyCES.EntLib.OrderType orderType = SkyCES.EntLib.OrderType.Desc;
        private int pageSize = 10;
        private string tableName;

        protected MssqlPagerClass()
        {
        }

        public abstract DataTable ExecuteDataTable();
        public abstract SqlDataReader ExecuteReader();
        protected SqlParameter[] PrepareCountParameter()
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@tableName", SqlDbType.NVarChar), new SqlParameter("@condition", SqlDbType.NVarChar) };
            parameterArray[0].Value = this.TableName;
            parameterArray[1].Value = this.mssqlCondition.ToString();
            return parameterArray;
        }

        protected SqlParameter[] PrepareParameter()
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@tableName", SqlDbType.NVarChar), new SqlParameter("@fields", SqlDbType.NVarChar), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@currentPage", SqlDbType.Int), new SqlParameter("@fieldName", SqlDbType.NVarChar), new SqlParameter("@orderType", SqlDbType.Bit), new SqlParameter("@condition", SqlDbType.NVarChar) };
            parameterArray[0].Value = this.TableName;
            parameterArray[1].Value = this.Fields;
            parameterArray[2].Value = this.PageSize;
            parameterArray[3].Value = this.CurrentPage;
            parameterArray[4].Value = this.OrderField;
            parameterArray[5].Value = this.OrderType;
            parameterArray[6].Value = this.mssqlCondition.ToString();
            return parameterArray;
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

        public SkyCES.EntLib.MssqlCondition MssqlCondition
        {
            get
            {
                return this.mssqlCondition;
            }
            set
            {
                this.mssqlCondition = value;
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

