using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Contrib.Extensions;
namespace JWShop.Entity
{
   public sealed class PrizeRecordInfo
    {
       public const string TABLENAME = "PrizeRecord";

       public int ActivityID { get; set; }

       public string ActivityName { get; set; }

       public string CellPhone { get; set; }

       public int IsPrize { get; set; }

       public string Prizelevel { get; set; }

       public string PrizeName { get; set; }

       public DateTime? PrizeTime { get; set; }

       public string RealName { get; set; }

       public int RecordId { get; set; }

       public int UserID { get; set; }

       public string UserName { get; set; }

       public string Company { get; set; }
    }
}
