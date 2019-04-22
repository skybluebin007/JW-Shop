using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.XcxApi.Pay
{
    public class Ticket
    {
        string _ticket;
        int _ticketExpire;

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int TicketExpire
        {
            get { return _ticketExpire; }
            set { _ticketExpire = value; }
        }
    }
}
