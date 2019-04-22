using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Contrib.Extensions;

namespace JWShop.Entity
{
    [Serializable]
    [Table("LotteryActivity")]
   public sealed class LotteryActivityInfo
    {
        public const string TABLENAME = "LotteryActivity";

        public int Id { get; set; }

        public string ActivityDesc { get; set; }

        public string ActivityKey { get; set; }

        public string ActivityName { get; set; }

        public string ActivityPic { get; set; }

        public int ActivityType { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxNum { get; set; }

        public string PrizeSetting { get; set; }

        public List<PrizeSetting> PrizeSettingList { get; set; }

        public DateTime StartTime { get; set; }

        public string GradeIds { get; set; }

        public int MinValue { get; set; }

        public string InvitationCode { get; set; }

        public DateTime OpenTime { get; set; }

        public int IsOpened { get; set; }
    }
}
