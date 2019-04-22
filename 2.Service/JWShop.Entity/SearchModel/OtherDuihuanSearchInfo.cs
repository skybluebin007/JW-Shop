using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 搜索模型。
    /// </summary>
    public sealed class OtherDuihuanSearchInfo
    {
        private string _truename;
        private string _mobile;

        /// <summary>
        /// 姓名
        /// </summary>
        public string truename
        {
            set { _truename = value; }
            get { return _truename; }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }

    }
}
