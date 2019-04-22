using System;

namespace JWShop.Entity
{
    /// <summary>
    /// 邮件发送记录搜索模型。
    /// </summary>
    public sealed class EmailSendRecordSearchInfo
    {
        private int isSystem = int.MinValue;

        public int IsSystem
        {
            set { this.isSystem = value; }
            get { return this.isSystem; }
        }
    }
}

