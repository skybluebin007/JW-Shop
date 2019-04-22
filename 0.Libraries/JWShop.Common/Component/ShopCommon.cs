using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Net;
using System.Text.RegularExpressions;


namespace JWShop.Common
{
    public sealed class ShopCommon
    {
        /// <summary>
        /// 得到bool型的图形表示
        /// </summary>
        /// <param name="oj"></param>
        /// <returns></returns>
        public static string GetBoolString(object oj)
        {
            string content = string.Empty;
            if (oj != null)
            {
                if (Convert.ToInt32(oj) == (int)BoolType.True)
                {
                    content = "√";
                }
                else
                {
                    content = "X";
                }
            }
            return content;
        }
        /// <summary>
        /// 得到bool型的汉字表示
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetBoolText(object i)
        {
            string content = string.Empty;
            if (i != null)
            {
                if (Convert.ToInt32(i) == (int)BoolType.True)
                {
                    content = "是";
                }
                else
                {
                    content = "否";
                }
            }
            return content;
        }
        /// <summary>
        /// 读取文件名的文件夹
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string ReadDirectory(string fullName, string name, string path)
        {
            string directory = string.Empty;
            if (fullName != string.Empty && fullName.IndexOf(path) > -1)
            {
                directory = fullName.Substring(fullName.IndexOf(path) - 1);
                directory = directory.Replace(name, string.Empty).Replace("\\", "/");
                directory = directory.Replace(path + "/", string.Empty);
            }
            return directory;
        }
        /// <summary>
        /// 读取文件名的文件名
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string ReadFileName(string fullName)
        {
            string fileName = string.Empty;
            if (fullName != string.Empty && fullName.IndexOf("Plugins") > -1)
            {
                fileName = fullName.Substring(fullName.IndexOf("Plugins") - 1);
                fileName = fileName.Replace("\\", "/");
            }
            return fileName;
        }
        /// <summary>
        /// 读取文件的图标
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadFileIcon(FileInfo file)
        {
            string result = "/Admin/Style/File/" + file.Extension.Substring(1) + ".gif";
            return result;
        }
        /// <summary>
        /// 搜索的结束时间处理
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime SearchEndDate(DateTime dt)
        {
            if (dt != DateTime.MinValue)
            {
                dt = dt.AddDays(1);
            }
            return dt;
        }
        /// <summary>
        /// 读取字典键值
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static F ReadValue<T, F>(Dictionary<T, F> dic, T key) where F : new()
        {
            F result = new F();
            if (dic.ContainsKey(key))
            {
                result = dic[key];
            }
            return result;
        }
        /// <summary>
        /// 绑定年份和月份
        /// </summary>
        public static void BindYearMonth(DropDownList Year, DropDownList Month)
        {
            Year.Items.Insert(0, new ListItem("请选择", ""));
            for (int i = ShopConfig.ReadConfigInfo().StartYear; i <= ShopConfig.ReadConfigInfo().EndYear; i++)
            {
                Year.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            Month.Items.Insert(0, new ListItem("请选择", ""));
            for (int i = 1; i <= 12; i++)
            {
                Month.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        /// <summary>
        /// 计算某月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int CountMonthDays(int year, int month)
        {
            return Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1).Day;
        }
        /// <summary>
        /// 比较多个数组的长度是否一致
        /// </summary>
        public static bool CompareLength(List<string[]> ss)
        {
            if (ss[0].Length < 1) return false;

            for (int i = 1; i < ss.Count; i++)
            {
                if (ss[0].Length != ss[i].Length) return false;
            }
            return true;
        }
        /// <summary>
        /// 取得默认图片
        /// </summary>
        public static string ShowImage(string imageStr)
        {
            
                return !string.IsNullOrEmpty(imageStr)?imageStr:"/Admin/Images/nopic.gif";
           
        }
        /// <summary>
        /// 取得用户默认头像
        /// </summary>
        public static string ShowUserPhoto(string imageStr)
        {

            return !string.IsNullOrEmpty(imageStr) ? imageStr : "/Admin/Images/nophoto.jpg";

        }
        public static T ConvertToT<T>(string str)
        {
            object typeDefaultValue = new object();
            Type conversionType = typeof(T);
            try
            {
                if ((conversionType.FullName == "System.String") && (str == null))
                {
                    str = string.Empty;
                }
                typeDefaultValue = Convert.ChangeType(str, conversionType);
            }
            catch
            {
                typeDefaultValue = GetTypeDefaultValue(conversionType);
            }
            return (T)typeDefaultValue;
        }

        private static object GetTypeDefaultValue(Type type)
        {
            object obj2 = new object();
            string fullName = type.FullName;
            if (fullName == null)
            {
                return obj2;
            }
            if (!(fullName == "System.String"))
            {
                if (fullName != "System.Int32")
                {
                    if (fullName == "System.Decimal")
                    {
                        return -79228162514264337593543950335M;
                    }
                    if (fullName == "System.Double")
                    {
                        return -1.7976931348623157E+308;
                    }
                    if (fullName != "System.DateTime")
                    {
                        return obj2;
                    }
                    return DateTime.MinValue;
                }
            }
            else
            {
                return string.Empty;
            }
            return -2147483648;
        }

        /// <summary>
        /// 产生优惠券号
        /// </summary>
        /// <param name="couponID">优惠劵ID</param>
        /// <param name="i">序号</param>
        /// <returns></returns>
        public static string CreateCouponNo(int couponId, int i)
        {
            string couponIDStr = string.Empty;
            couponIDStr = "000" + couponId.ToString();
            couponIDStr = couponIDStr.Substring(couponIDStr.Length - 3);
            string number = string.Empty;
            number = "00000" + i.ToString();
            number = number.Substring(number.Length - 5);
            Random rd = new Random(i);
            return couponIDStr + number + rd.Next(1000, 9999).ToString();
        }
        /// <summary>
        /// 产生优惠券号密码
        /// </summary>
        /// <param name="i">序号</param>
        /// <returns></returns>
        public static string CreateCouponPassword(int i)
        {
            Random rd = new Random(i);
            return rd.Next(100000, 999999).ToString();
        }
        /// <summary>
        /// 产生订单编号
        /// </summary>
        /// <returns></returns>
        public static string CreateOrderNumber()
        {
            Random rd = new Random();
            return DateTime.Now.ToString("yyMMddhhmmssfff") + rd.Next(10, 99).ToString();
        }
        /// <summary>
        /// 产生退款服务工单号
        /// </summary>
        /// <returns></returns>
        public static string CreateOrderRefundNumber()
        {
            Random rd = new Random();
            return DateTime.Now.ToString("yyMMddhhmmssfff") + rd.Next(10, 99).ToString();
        }

        /// <summary>
        /// 输出星号遮挡的字符串
        /// </summary>
        /// <param name="originalStr">原始字符串</param>
        /// <returns></returns>
        public static string GetStarString(string originalStr)
        {
            string result = originalStr;
            if (originalStr.Length == 1)
            {
                result = "*";
            }
            else if (originalStr.Length == 2)
            {
                result = "**";
            }
            else
            {
                string replaceStr = "";
                int replaceLenth = (int)Math.Ceiling(Convert.ToDecimal(originalStr.Length) / 3);
                for (int i = 1; i <= replaceLenth; i++)
                {
                    replaceStr += "*";
                }
                result = originalStr.Replace(originalStr.Substring((originalStr.Length / 3), replaceLenth), replaceStr);
            }
            return result;
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }
        /// <summary>
        /// 检查手机号码是否正确格式
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool CheckMobile(string mobile)
        {
            bool result = false;
            //电信手机号码正则        
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);
            //联通手机号正则        
            string liantong = @"^1[34578][01256]\d{8}$";
            Regex tReg = new Regex(liantong);
            //移动手机号正则        
            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);
            //Regex rg = new Regex(@"^1[3-9]\d{9}$;");
            if (dReg.IsMatch(mobile) || tReg.IsMatch(mobile) || yReg.IsMatch(mobile))
            {
                //验证通过
                result = true;
            }

            return result;
        }
        /// <summary>
        /// 检查座机号码是否正确格式
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool CheckTel(string tel)
        {          
            Regex reg =new  Regex(@"^0?(10|(2|3[1,5,7]|4[1,5,7]|5[1,3,5,7]|7[1,3,5,7,9]|8[1,3,7,9])[0-9]|91[0-7,9]|(43|59|85)[1-9]|39[1-8]|54[3,6]|(701|580|349|335)|54[3,6]|69[1-2]|44[0,8]|48[2,3]|46[4,7,8,9]|52[0,3,7]|42[1,7,9]|56[1-6]|63[1-5]|66[0-3,8]|72[2,4,8]|74[3-6]|76[0,2,3,5,6,8,9]|82[5-7]|88[1,3,6-8]|90[1-3,6,8,9])\d{7,8}|^\d{3,4}-\d{7,8}(-\d{3,4})?$");
           
            return reg.IsMatch(tel);          
        }
        /// <summary>
        /// 检查邮箱是否正确格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckEmail(string email)
        {
            String EmailReg = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(email, EmailReg);
        }
        ///将数据流转为byte[]  
        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
        }
        /// <summary>
        /// 砍价状态文字显示
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string ActiveStatus(DateTime start, DateTime end,int status)
        {
            string result = string.Empty;

            if (start > DateTime.Now)
            {
                result = "未开始";
            }
            else if (DateTime.Now > end)
            {
                result = "已结束";
            }
            else
            {
                if (status == 0)
                {
                    result = "已关闭";
                }
                else
                {
                    result = "进行中";
                }
            }

            return result;
        }
    }
}