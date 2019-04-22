namespace SkyCES.EntLib
{
    using System;
    using System.Web;

    public sealed class CookiesHelper
    {
        public static void AddCookie(string name, string value)
        {
            HttpCookie cookie = new HttpCookie(name) {
                Path = "/",
                Value = value,
                HttpOnly=true
            };
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void AddCookie(string name, string value, int time, TimeType timeType)
        {
            HttpCookie cookie = new HttpCookie(name) {
                Path = "/",
                Value = value,
                HttpOnly=true
            };
            switch (timeType)
            {
                case TimeType.Year:
                    cookie.Expires = DateTime.Now.AddYears(time);
                    break;

                case TimeType.Month:
                    cookie.Expires = DateTime.Now.AddMonths(time);
                    break;

                case TimeType.Day:
                    cookie.Expires = DateTime.Now.AddDays((double) time);
                    break;

                case TimeType.Hour:
                    cookie.Expires = DateTime.Now.AddHours((double) time);
                    break;

                case TimeType.Minute:
                    cookie.Expires = DateTime.Now.AddMinutes((double) time);
                    break;

                case TimeType.Second:
                    cookie.Expires = DateTime.Now.AddSeconds((double) time);
                    break;

                case TimeType.Millisecond:
                    cookie.Expires = DateTime.Now.AddMilliseconds((double) time);
                    break;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void DeleteCookie(string name)
        {
            HttpCookie cookie = new HttpCookie(name) {
                Expires = DateTime.Now.AddDays(-10.0)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static HttpCookie ReadCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public static string ReadCookieValue(string name)
        {
            string str = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return (str = cookie.Value);
            }
            return str;
        }
    }
}

