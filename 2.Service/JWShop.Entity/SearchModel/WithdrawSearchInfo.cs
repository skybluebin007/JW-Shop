using System;
namespace JWShop.Entity
{
   public  sealed class WithdrawSearchInfo
    {
        public int Distributor_Id { get; set; } = int.MinValue;
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTtime { get; set; } = DateTime.MinValue;
        public int Status { get; set; } = int.MinValue;
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
    }
}
