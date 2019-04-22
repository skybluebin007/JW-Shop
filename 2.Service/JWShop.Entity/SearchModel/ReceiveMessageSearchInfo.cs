using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.Entity
{
    public sealed class ReceiveMessageSearchInfo
    {

        public string Title = string.Empty;
        public string Content = string.Empty;
        public DateTime StartDate = DateTime.MinValue;
        public DateTime EndDate = DateTime.MinValue;
        public int IsRead = int.MinValue;
        public int IsAdmin = int.MinValue;
        public int FromUserID = int.MinValue;
        public string FromUserName = string.Empty;
        public int UserID = int.MinValue;
        public string UserName = string.Empty;
    }
}
