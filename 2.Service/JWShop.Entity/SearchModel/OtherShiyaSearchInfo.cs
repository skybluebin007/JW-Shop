using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
	/// 搜索模型。
	/// </summary>
	public sealed class OtherShiyaSearchInfo
    {
        #region Model
        private int _id;
        private int _saleid;
        private int _shuigongid;
        private int _shiyaid;
        private int _customerid;
        private int _orderid;
        private string _truename;
        private string _mobile;
        private string _address;
        private string _anname;
        private string _anmobile;
        private string _shiyaname;
        private string _shiyamobile;
        private int _stat;
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
        /// 经销商ID
        /// </summary>
        public int saleid
        {
            set { _saleid = value; }
            get { return _saleid; }
        }
        /// <summary>
        /// 水工ID
        /// </summary>
        public int shuigongid
        {
            set { _shuigongid = value; }
            get { return _shuigongid; }
        }
        /// <summary>
        /// 试压用户ID
        /// </summary>
        public int shiyaid
        {
            set { _shiyaid = value; }
            get { return _shiyaid; }
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int customerid
        {
            set { _customerid = value; }
            get { return _customerid; }
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int orderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string truename
        {
            set { _truename = value; }
            get { return _truename; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 业主地址
        /// </summary>
        public string address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 安装人姓名
        /// </summary>
        public string anname
        {
            set { _anname = value; }
            get { return _anname; }
        }
        /// <summary>
        /// 安装人电话
        /// </summary>
        public string anmobile
        {
            set { _anmobile = value; }
            get { return _anmobile; }
        }
        /// <summary>
        /// 试压人姓名
        /// </summary>
        public string shiyaname
        {
            set { _shiyaname = value; }
            get { return _shiyaname; }
        }
        /// <summary>
        /// 试压人电话
        /// </summary>
        public string shiyamobile
        {
            set { _shiyamobile = value; }
            get { return _shiyamobile; }
        }
        /// <summary>
        /// 检测(0合格1不合格)
        /// </summary>
        public int stat
        {
            set { _stat = value; }
            get { return _stat; }
        }
        /// <summary>
        /// 提交试压日期
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        #endregion Model
    }
}
