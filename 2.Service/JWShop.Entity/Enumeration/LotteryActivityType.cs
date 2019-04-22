using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
namespace JWShop.Entity
{
    public enum LotteryActivityType
    {
        [Enum("刮刮卡")]
        Scratch = 2,
        [Enum("微报名")]
        SignUp = 5,
        [Enum("砸金蛋")]
        SmashEgg = 3,
        [Enum("微抽奖")]
        Ticket = 4,
        [Enum("大转盘")]
        Wheel = 1
    }
}
