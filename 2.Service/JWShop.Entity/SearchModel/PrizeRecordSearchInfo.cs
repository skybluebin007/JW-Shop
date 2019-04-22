using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
  public sealed  class PrizeRecordSearchInfo
    {
        public int ActivityID { get; set; }

        public LotteryActivityType ActivityType { get; set; }

        public string CellPhone { get; set; }

        public int IsPrize { get; set; }

        public int UserID { get; set; }
    }
}
