using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop.XcxApi.Pay
{
    /// <summary>
    ///Access_token 的摘要说明
    /// </summary>
    public class Access_token
    {
        public Access_token()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        string _access_token;
        int _expires_in;

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }
}
