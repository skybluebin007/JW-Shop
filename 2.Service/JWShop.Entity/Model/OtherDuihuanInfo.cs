using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 模型
    /// </summary>
    public sealed class OtherDuihuanInfo
    {
        public const string TABLENAME = "OtherDuihuan";

        #region Model
        private int _id;
        private int _userid;
        private string _truename;
        private string _mobile;
        private string _note;
        private string _integral;
        private int _adminid;
        private DateTime _addtime;
        /// <summary>
        /// ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 兑换用户ID
        /// </summary>
        public int userid
        {
            set { _userid = value; }
            get { return _userid; }
        }
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
        /// <summary>
        /// 备注(兑换了哪些商品)
        /// </summary>
        public string note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 使用积分数
        /// </summary>
        public string integral
        {
            set { _integral = value; }
            get { return _integral; }
        }
        /// <summary>
        /// 操作人员
        /// </summary>
        public int adminid
        {
            set { _adminid = value; }
            get { return _adminid; }
        }
        /// <summary>
        /// 兑换时间
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        #endregion Model
    }
}
