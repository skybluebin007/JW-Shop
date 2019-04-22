namespace SkyCES.EntLib
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public sealed class ClientHelper
    {
        private static string GetBrowser(string UserAgent)
        {
            if (UserAgent.IndexOf("spider") > -1)
            {
                if (UserAgent.IndexOf("Baiduspider") > -1)
                {
                    return "Baiduspider";
                }
                return "UnknownBot";
            }
            if (UserAgent.IndexOf("DreamPassport") > -1)
            {
                return Regex.Replace(UserAgent, @".*DreamPassport/(\d+[.\d]*)+.*", "DreamPassport $1");
            }
            if (UserAgent.IndexOf("Firefox") > -1)
            {
                return Regex.Replace(UserAgent, @".*Firefox/(\d+[.\d]*[.\d]*\+*)+.*", "Firefox $1");
            }
            if (UserAgent.IndexOf("Safari") > -1)
            {
                return Regex.Replace(UserAgent, @".*Safari/(\d+[.\d]*).*", "Safari $1");
            }
            if (UserAgent.IndexOf("Netscape") > -1)
            {
                return Regex.Replace(UserAgent, @".*Netscape[\d+/| ]*(\d+[.\d]*).*", "Netscape $1");
            }
            if (UserAgent.ToLower().IndexOf("konqueror") > -1)
            {
                return Regex.Replace(UserAgent, @".*konqueror[/| ]*(\d+[.\d+]*)*.*", "Konqueror $1", RegexOptions.IgnoreCase);
            }
            if (UserAgent.IndexOf("Gecko") > -1)
            {
                return Regex.Replace(UserAgent, @".*Gecko/(\d+).*", "Gecko $1");
            }
            if (UserAgent.IndexOf("Opera") > -1)
            {
                return Regex.Replace(UserAgent, @".*Opera[/| ]+(\d+[.\d]*)+.*", "Opera $1");
            }
            if (UserAgent.IndexOf("ZoneSurf") > -1)
            {
                return Regex.Replace(UserAgent, @".*ZoneSurf/(\d+[.\d]*)+.*", "ZoneSurf $1");
            }
            if (UserAgent.IndexOf("IBrowse") > -1)
            {
                return Regex.Replace(UserAgent, @".*IBrowse/(\d+[.\d]*)+.*", "IBrowse $1");
            }
            if (UserAgent.IndexOf("iCab") > -1)
            {
                return Regex.Replace(UserAgent, @".*iCab \w*[/| ]*\w*(\d+[\.\d+]*).*", "iCab $1");
            }
            if (UserAgent.IndexOf("Lynx") > -1)
            {
                return Regex.Replace(UserAgent, @".*Lynx[/| ]*([^ |^;|^\)|^\(]*).*", "Lynx $1");
            }
            if (UserAgent.IndexOf("WebCapture") > -1)
            {
                return Regex.Replace(UserAgent, @".*WebCapture[/| ]*([^ |^;|^\)|^\(]*).*", "WebCapture $1");
            }
            if (UserAgent.IndexOf("Maxthon") > -1)
            {
                return ("Maxthon " + Regex.Replace(UserAgent, @".*MSIE (\d+[.\d]*)+.*", "IE $1"));
            }
            if (UserAgent.IndexOf("TencentTraveler") > -1)
            {
                return ("TencentTraveler " + Regex.Replace(UserAgent, @".*MSIE (\d+[.\d]*)+.*", "IE$1"));
            }
            if (UserAgent.IndexOf("MSIE") > -1)
            {
                return Regex.Replace(UserAgent, @".*MSIE (\d+[.\d]*)+.*", "MSIE $1");
            }
            if (UserAgent.IndexOf("BTRON") > -1)
            {
                return "BTRON";
            }
            if (UserAgent.IndexOf("Mozilla") > -1)
            {
                return Regex.Replace(UserAgent, @".*Mozilla/(\d+[.\da-zA-Z]*)+.*", "Mozilla $1");
            }
            return "Unknown";
        }

        private static string GetSpider(string UserAgent)
        {
            if (((UserAgent.ToLower().IndexOf("bot") > -1) || (UserAgent.ToLower().IndexOf("spider") > -1)) || (UserAgent.ToLower().IndexOf("slurp") > -1))
            {
                return "spider";
            }
            return string.Empty;
        }

        private static string GetSystem(string UserAgent)
        {
            if (UserAgent != string.Empty)
            {
                if (UserAgent.IndexOf("Win") > -1)
                {
                    if (UserAgent.IndexOf("Windows NT CE") > -1)
                    {
                        return "Windows CE";
                    }
                    if (UserAgent.IndexOf("Windows NT 5.2") > -1)
                    {
                        return "Windows 2003";
                    }
                    if (UserAgent.IndexOf("Windows NT 5.1") > -1)
                    {
                        return "Windows XP";
                    }
                    if (UserAgent.IndexOf("Windows NT 5.0") > -1)
                    {
                        return "Windows 2000";
                    }
                    if (UserAgent.IndexOf("Windows NT") > -1)
                    {
                        return "Windows NT";
                    }
                    if (UserAgent.IndexOf("Windows 9x") > -1)
                    {
                        return "Windows ME";
                    }
                    if (UserAgent.IndexOf("Windows 98") > -1)
                    {
                        return "Windows 98";
                    }
                    if (UserAgent.IndexOf("Windows 95") > -1)
                    {
                        return "Windows 95";
                    }
                    if (UserAgent.IndexOf("Windows ME") > -1)
                    {
                        return "Windows ME";
                    }
                    if (UserAgent.IndexOf("Win98") > -1)
                    {
                        return "Windows 98";
                    }
                    if (UserAgent.IndexOf("Win98Lite") > -1)
                    {
                        return "Windows 98 Lite";
                    }
                    if (UserAgent.IndexOf("Windows XP") > -1)
                    {
                        return "Windows XP";
                    }
                    if (UserAgent.IndexOf("WinNT") > -1)
                    {
                        return "Windows NT";
                    }
                    if (UserAgent.IndexOf("Win95") > -1)
                    {
                        return "Windows 95";
                    }
                    if (UserAgent.IndexOf("Win 9x 4.90") > -1)
                    {
                        return "Windows 98";
                    }
                    return "Windows";
                }
                if (UserAgent.ToLower().IndexOf("linux") > -1)
                {
                    if (UserAgent.IndexOf("X11") > -1)
                    {
                        return Regex.Replace(UserAgent, @".*linux[/| ]*([^;|^\)]*).*", "Linux $1 X11", RegexOptions.IgnoreCase);
                    }
                    return Regex.Replace(UserAgent, @".*linux[/| ]*([^;|^\)]*).*", "Linux $1", RegexOptions.IgnoreCase);
                }
                if (UserAgent.IndexOf("GNUTLS") > -1)
                {
                    return Regex.Replace(UserAgent, @".*GNUTLS[/| ]*([^;|^\)]*).*", "GNUTLS $1");
                }
                if (UserAgent.IndexOf("FreeBSD") > -1)
                {
                    if (UserAgent.IndexOf("X11") > -1)
                    {
                        return Regex.Replace(UserAgent, @".*FreeBSD (\d+[.\d]*[--RELEASE]*)*.*", "FreeBSD $1 X11");
                    }
                    return Regex.Replace(UserAgent, @".*FreeBSD (\d+[.\d]*[--RELEASE]*)*.*", "FreeBSD $1");
                }
                if (UserAgent.IndexOf("OpenBSD") > -1)
                {
                    if (UserAgent.IndexOf("X11") > -1)
                    {
                        return "OpenBSD X11";
                    }
                    return "OpenBSD";
                }
                if (UserAgent.IndexOf("SunOS") > -1)
                {
                    if (UserAgent.IndexOf("X11") > -1)
                    {
                        return Regex.Replace(UserAgent, @".*SunOS( \d+[.\d]*)*.*", "SunOS$1 X11");
                    }
                    return Regex.Replace(UserAgent, @".*SunOS( \d+[.\d]*)*.*", "SunOS$1");
                }
                if (UserAgent.IndexOf("SGI") > -1)
                {
                    if (UserAgent.IndexOf("X11") > -1)
                    {
                        return "SGI X11";
                    }
                    return "SGI";
                }
                if (UserAgent.IndexOf("Mac_PowerPC") > -1)
                {
                    return "Macintosh";
                }
                if (UserAgent.IndexOf("Macintosh") > -1)
                {
                    return "Macintosh";
                }
                if (UserAgent.IndexOf("PPC") > -1)
                {
                    if (UserAgent.IndexOf("Mac OS X Mach-O") > -1)
                    {
                        return "Mac OS X Mach-O";
                    }
                    return "Macintosh";
                }
                if (UserAgent.IndexOf("Mac OS") > -1)
                {
                    return Regex.Replace(UserAgent, @".*Mac OS (\d+[.\d]*)+.*", "Mac OS $1");
                }
                if (UserAgent.IndexOf("AmigaOS") > -1)
                {
                    return Regex.Replace(UserAgent, @".*AmigaOS (\d+[.\d]*)+.*", "AmigaOS $1");
                }
                if (UserAgent.IndexOf("B-right") > -1)
                {
                    return Regex.Replace(UserAgent, ".*B-right/([a-zA-Z]+).*", "B-right $1");
                }
                if (UserAgent.IndexOf("BeOS") > -1)
                {
                    return "BeOS";
                }
                if (UserAgent.IndexOf("MS-DOS") > -1)
                {
                    return "MS-DOS";
                }
                if (UserAgent.IndexOf("DOS") > -1)
                {
                    return "DOS";
                }
                if (UserAgent.IndexOf("DreamPassport") > -1)
                {
                    return "Dreamcast";
                }
                if (UserAgent.IndexOf("Commodore64") > -1)
                {
                    return "Commodore64";
                }
                if (UserAgent.IndexOf("MS-DOS") > -1)
                {
                    return "MS-DOS";
                }
                if (UserAgent.IndexOf("DOS") > -1)
                {
                    return "DOS";
                }
                return "Unknown";
            }
            return "Unknown";
        }

        public static string Agent
        {
            get
            {
                return HttpContext.Current.Request.UserAgent;
            }
        }

        public static string Browser
        {
            get
            {
                return GetBrowser(HttpContext.Current.Request.UserAgent);
            }
        }

        public static string HostName
        {
            get
            {
                return HttpContext.Current.Request.UserHostName;
            }
        }

        public static string IP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        public static string Key
        {
            get
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(Agent + IP, "MD5").Substring(8, 0x10).ToLower();
            }
        }

        public static string OS
        {
            get
            {
                return GetSystem(HttpContext.Current.Request.UserAgent);
            }
        }

        public static string Spider
        {
            get
            {
                return GetSpider(HttpContext.Current.Request.UserAgent);
            }
        }
    }
}

