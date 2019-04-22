using System;
using System.Collections.Generic;

namespace JWShop.Entity
{
    public sealed class HotSearchKeyInfo
    {
        public const string TABLENAME = "HotSearchKey";
        public int Id { get; set; }
        public string Name { get; set; }
        public int SearchTimes { get; set; }
        public string AddCol1 { get; set; }
        public string AddCol2 { get; set; }
    }
}
