using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 实体模型
    /// </summary>
    public sealed class OtherShiyaInfo
    {
        public const string TABLENAME = "OtherShiya";

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
        private string _housetype;
        private string _kaifashang;
        private string _jiazhuang;
        private string _kongtiao;
        private string _guanjian;
        private string _pushe;
        private string _baohu;
        private string _beizhu;
        private string _zongfa;
        private string _hunzhuang;
        private string _huaheng;
        private string _xiangqian;
        private string _guolvqi;
        private string _jietou;
        private string _baoyastart;
        private string _baoyaend;
        private string _yunxing;
        private string _jiance;
        private string _shiyaname;
        private string _shiyamobile;
        private string _gaozhi;
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
        /// 房屋类型
        /// </summary>
        public string housetype
        {
            set { _housetype = value; }
            get { return _housetype; }
        }
        /// <summary>
        /// 开发商名称
        /// </summary>
        public string kaifashang
        {
            set { _kaifashang = value; }
            get { return _kaifashang; }
        }
        /// <summary>
        /// 家装公司
        /// </summary>
        public string jiazhuang
        {
            set { _jiazhuang = value; }
            get { return _jiazhuang; }
        }
        /// <summary>
        /// 空调公司
        /// </summary>
        public string kongtiao
        {
            set { _kongtiao = value; }
            get { return _kongtiao; }
        }
        /// <summary>
        /// 管件
        /// </summary>
        public string guanjian
        {
            set { _guanjian = value; }
            get { return _guanjian; }
        }
        /// <summary>
        /// 管道铺设方式
        /// </summary>
        public string pushe
        {
            set { _pushe = value; }
            get { return _pushe; }
        }
        /// <summary>
        /// 管道保护
        /// </summary>
        public string baohu
        {
            set { _baohu = value; }
            get { return _baohu; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string beizhu
        {
            set { _beizhu = value; }
            get { return _beizhu; }
        }
        /// <summary>
        /// 总阀
        /// </summary>
        public string zongfa
        {
            set { _zongfa = value; }
            get { return _zongfa; }
        }
        /// <summary>
        /// 混装
        /// </summary>
        public string hunzhuang
        {
            set { _hunzhuang = value; }
            get { return _hunzhuang; }
        }
        /// <summary>
        /// 划痕
        /// </summary>
        public string huaheng
        {
            set { _huaheng = value; }
            get { return _huaheng; }
        }
        /// <summary>
        /// 带铜镶嵌个数
        /// </summary>
        public string xiangqian
        {
            set { _xiangqian = value; }
            get { return _xiangqian; }
        }
        /// <summary>
        /// 过滤器安装个数
        /// </summary>
        public string guolvqi
        {
            set { _guolvqi = value; }
            get { return _guolvqi; }
        }
        /// <summary>
        /// 焊接接头是否正常
        /// </summary>
        public string jietou
        {
            set { _jietou = value; }
            get { return _jietou; }
        }
        /// <summary>
        /// 保压开始日期
        /// </summary>
        public string baoyastart
        {
            set { _baoyastart = value; }
            get { return _baoyastart; }
        }
        /// <summary>
        /// 保压结束
        /// </summary>
        public string baoyaend
        {
            set { _baoyaend = value; }
            get { return _baoyaend; }
        }
        /// <summary>
        /// 管道运行压力
        /// </summary>
        public string yunxing
        {
            set { _yunxing = value; }
            get { return _yunxing; }
        }
        /// <summary>
        /// 管道检测压力
        /// </summary>
        public string jiance
        {
            set { _jiance = value; }
            get { return _jiance; }
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
        /// 管道使用注意事项告知
        /// </summary>
        public string gaozhi
        {
            set { _gaozhi = value; }
            get { return _gaozhi; }
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
