using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    public enum KeyValueType
    {
        /// <summary>
        /// 友情链接
        /// </summary>
        [Enum("友情链接")]
        Link = 1,
        [Enum("办公地址")]
        Address = 2,
        [Enum("电话")]
        Tel = 3,
        [Enum("香港电话")]
        HKTel = 4,
        [Enum("二维码图片")]
        QRCode = 5,
        [Enum("QQ")]
        QQ = 6
    }
}
