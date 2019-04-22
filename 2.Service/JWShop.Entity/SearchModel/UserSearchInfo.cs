using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 用户搜索模型。
    /// </summary>
    public sealed class UserSearchInfo
    {
        private string userName = string.Empty;
        private string email = string.Empty;
        private int sex = int.MinValue;
        private DateTime startRegisterDate = DateTime.MinValue;
        private DateTime endRegisterDate = DateTime.MinValue;
        private int status = int.MinValue;
        private string inUserId = string.Empty;
        private int userType = int.MinValue;
        private string cardNo = string.Empty;
        private string providerNo = string.Empty;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        public string UserName
        {
            set { this.userName = value; }
            get { return this.userName; }
        }
        public string Email
        {
            set { this.email = value; }
            get { return this.email; }
        }
        public int Sex
        {
            set { this.sex = value; }
            get { return this.sex; }
        }
        public DateTime StartRegisterDate
        {
            set { this.startRegisterDate = value; }
            get { return this.startRegisterDate; }
        }
        public DateTime EndRegisterDate
        {
            set { this.endRegisterDate = value; }
            get { return this.endRegisterDate; }
        }
        public int Status
        {
            set { this.status = value; }
            get { return this.status; }
        }
        public string InUserId
        {
            set { this.inUserId = value; }
            get { return this.inUserId; }
        }
        public int UserType
        {
            set { this.userType = value; }
            get { return this.userType; }
        }
        public string CardNo
        {
            set { this.cardNo = value; }
            get { return this.cardNo; }
        }
        public string ProviderNo
        {
            set { this.providerNo = value; }
            get { return this.providerNo; }
        }
        /// <summary>
        /// 分销商状态
        ///  -1-冻结  0--待审核  1--正常
        /// </summary>
        public int Distributor_Status { get; set; } = int.MinValue;
    }
}

