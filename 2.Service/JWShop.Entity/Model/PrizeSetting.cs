using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
 public  class PrizeSetting
    {
        public string PrizeLevel { get; set; }

        public string PrizeTitle { get; set; }

        public string PrizeName { get; set; }

        public int PrizeNum { get; set; }

        public decimal Probability { get; set; }
    }
}
