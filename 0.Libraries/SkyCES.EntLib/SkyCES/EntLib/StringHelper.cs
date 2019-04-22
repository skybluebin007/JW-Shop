namespace SkyCES.EntLib
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public sealed class StringHelper
    {
        public static string AddSafe(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = ClearJS(ClearIframe(KillJapan(ClearHTML(content.Replace("'", string.Empty)))));
            }
            return content;
        }

        public static string AdminAddSafe(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = ClearJS(ClearIframe(content));
            }
            return content;
        }

        public static string[] BubbleSortASC(string[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (string.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        string str = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = str;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

        public static int BuildPow(int Number)
        {
            return (int) Math.Pow(2.0, (double) Number);
        }

        public static int BuildPow(string Number)
        {
            int num = 0;
            if (Number != string.Empty)
            {
                foreach (string str in Number.Split(new char[] { ',' }))
                {
                    num += BuildPow(Convert.ToInt32(str));
                }
            }
            return num;
        }

        public static bool CheckPow(int totalNumber, int checkNumber)
        {
            int num = (int) Math.Pow(2.0, (double) checkNumber);
            return ((totalNumber & num) == num);
        }

        public static string ClearHTML(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = content.Replace("<", "&lt;");
                content = content.Replace(">", "&gt;");
            }
            return content;
        }

        public static string ClearIframe(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                string str = "<iframe|</iframe|<frame|</frame";
                foreach (string str2 in str.Split(new char[] { '|' }))
                {
                    content = Regexs(content, str2, "&lt;" + str2.Substring(1));
                }
            }
            return content;
        }

        public static string ClearJS(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = ClearIframe(content);
                content = content.Replace("/*", @"\*");
                content = content.Replace("*/", @"*\");
                string str = "position|left|top";
                foreach (string str2 in str.Split(new char[] { '|' }))
                {
                    content = Regexs(content, @"( style[\s]*=[\s\S]*?)" + str2 + @"[\s]*:[\s]*\w+[\s;]*", "$1");
                }
                string str3 = "<script|</script|cookie|alert|expression|eval|escape|write";
                foreach (string str2 in str3.Split(new char[] { '|' }))
                {
                    content = Regexs(content, str2, "<span>" + str2.Substring(0, 1) + "</span>" + str2.Substring(1));
                }
            }
            return content;
        }

        public static string ClearUBB(string content)
        {
            string input = string.Empty;
            if ((content != null) && (content != string.Empty))
            {
                Match match;
                input = content;
                Regex regex = new Regex(@"(\[b\])((.|\n)*?)(\[\/b\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[i\])((.|\n)*?)(\[\/i\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[u\])([ \S\t]*?)(\[\/u\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[sup\])([ \S\t]*?)(\[\/sup\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[sub\])([ \S\t]*?)(\[\/sub\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[email\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[email=([ \S\t]+)\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[size=([1-7])\])([ \S\t]*?)(\[\/size\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[color=([\S]+)\])([ \S\t]*?)(\[\/color\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[font=([\S]+)\])([ \S\t]*?)(\[\/font\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[picture\])([ \S\t]*?)(\[\/picture\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[align=([\S]+)\])([ \S\t]*?)(\[\/align\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[h=([1-6])\])([ \S\t]*?)(\[\/h\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[list(=(A|a|I|i| ))?\]([ \S\t]*)\r\n)((\[\*\]([ \S\t]*\r\n))*?)(\[\/list\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    string str2 = match.Groups[5].ToString();
                    Regex regex2 = new Regex(@"\[\*\]([ \S\t]*\r\n?)", RegexOptions.IgnoreCase);
                    for (Match match2 = regex2.Match(str2); match2.Success; match2 = match2.NextMatch())
                    {
                        str2 = str2.Replace(match2.Groups[0].ToString(), "<LI>" + match2.Groups[1]);
                    }
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[4].ToString());
                }
                regex = new Regex(@"(\[shadow=)(\d*?),(#*\w*?),(\d*?)\]([ \S\t]*?)(\[\/shadow\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[5].ToString());
                }
                regex = new Regex(@"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([ \S\t]*?)(\[\/glow\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[5].ToString());
                }
                regex = new Regex(@"(\[center\])([ \S\t]*?)(\[\/center\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[em([\S\t]*?)\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[img\])([ \S\t]*?)(\[\/img\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[file\])([ \S\t]*?)(\[\/file\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[flash=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[4].ToString());
                }
                regex = new Regex(@"(\[flash\])([ \S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[swf\])([ \S\t]*?)(\[\/swf\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[dir=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/dir\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[4].ToString());
                }
                regex = new Regex(@"(\[rm=)(\d*?),(\d*?),([\S\t]*?)\]([ \S\t]*?)(\[\/rm\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[5].ToString());
                }
                regex = new Regex(@"(\[mp=)(\d*?),(\d*?),([\S\t]*?)\]([ \S\t]*?)(\[\/mp\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[5].ToString());
                }
                regex = new Regex(@"(\[qt=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/qt\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[4].ToString());
                }
                regex = new Regex(@"(\[QUOTE\])((.|\n)*?)(\[\/QUOTE\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[CODE\])((.|\n)*?)(\[\/CODE\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[move\])((.|\n)*?)(\[\/move\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[fly\])((.|\n)*?)(\[\/fly\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[image\])([ \S\t]*?)(\[\/image\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[url\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[2].ToString());
                }
                regex = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
                regex = new Regex(@"(\[download=([ \S\t]*?)\])([ \S\t]*?)(\[\/download\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), match.Groups[3].ToString());
                }
            }
            return input;
        }

        public static string Decode(string content, string key)
        {
            string str = string.Empty;
            if (content != string.Empty)
            {
                try
                {
                    key = PrepareKey(key);
                    string[] strArray = content.Split("-".ToCharArray());
                    byte[] inputBuffer = new byte[strArray.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        inputBuffer[i] = byte.Parse(strArray[i], NumberStyles.HexNumber);
                    }
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider {
                        Key = Encoding.ASCII.GetBytes(key),
                        IV = Encoding.ASCII.GetBytes(key)
                    };
                    byte[] bytes = provider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                    str = Encoding.UTF8.GetString(bytes);
                }
                catch
                {
                }
            }
            return str;
        }

        public static string Decrypt(string encryptfilename)
        {
            byte[] bytes = Convert.FromBase64String(encryptfilename);
            return HttpContext.Current.Request.ContentEncoding.GetString(bytes);
        }

        public static string Encode(string content, string key)
        {
            string str = string.Empty;
            if (content != string.Empty)
            {
                try
                {
                    key = PrepareKey(key);
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider {
                        Key = Encoding.ASCII.GetBytes(key),
                        IV = Encoding.ASCII.GetBytes(key)
                    };
                    str = BitConverter.ToString(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
                }
                catch
                {
                }
            }
            return str;
        }

        public static string Encrypt(string filename)
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(HttpContext.Current.Request.ContentEncoding.GetBytes(filename)));
        }

        public static bool HasUBB(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                Regex regex = new Regex(@"(\[b\])((.|\n)*?)(\[\/b\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[i\])((.|\n)*?)(\[\/i\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[u\])([ \S\t]*?)(\[\/u\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[sup\])([ \S\t]*?)(\[\/sup\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[sub\])([ \S\t]*?)(\[\/sub\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[url\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[email\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[email=([ \S\t]+)\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[size=([1-7])\])([ \S\t]*?)(\[\/size\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[color=([\S]+)\])([ \S\t]*?)(\[\/color\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[font=([\S]+)\])([ \S\t]*?)(\[\/font\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[picture\])([ \S\t]*?)(\[\/picture\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[align=([\S]+)\])([ \S\t]*?)(\[\/align\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[h=([1-6])\])([ \S\t]*?)(\[\/h\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[list(=(A|a|I|i| ))?\]([ \S\t]*)\r\n)((\[\*\]([ \S\t]*\r\n))*?)(\[\/list\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[shadow=)(\d*?),(#*\w*?),(\d*?)\]([ \S\t]*?)(\[\/shadow\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([ \S\t]*?)(\[\/glow\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[center\])([ \S\t]*?)(\[\/center\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[em([\S\t]*?)\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[img\])([ \S\t]*?)(\[\/img\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[file\])([ \S\t]*?)(\[\/file\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[flash=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[flash\])([ \S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[swf\])([ \S\t]*?)(\[\/swf\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[dir=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/dir\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[rm=)(\d*?),(\d*?),([\S\t]*?)\]([ \S\t]*?)(\[\/rm\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[mp=)(\d*?),(\d*?),([\S\t]*?)\]([ \S\t]*?)(\[\/mp\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[qt=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/qt\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[quote\])((.|\n)*?)(\[\/quote\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[code\])((.|\n)*?)(\[\/code\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[move\])((.|\n)*?)(\[\/move\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[fly\])((.|\n)*?)(\[\/fly\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[image\])([ \S\t]*?)(\[\/image\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
                regex = new Regex(@"(\[download=([ \S\t]*?)\])([ \S\t]*?)(\[\/download\])", RegexOptions.IgnoreCase);
                if (regex.IsMatch(content))
                {
                    return true;
                }
            }
            return false;
        }

        public static byte[] HMACSHA1(string content, string key)
        {
            System.Security.Cryptography.HMACSHA1 hmacsha = new System.Security.Cryptography.HMACSHA1 {
                Key = Encoding.ASCII.GetBytes(key)
            };
            byte[] bytes = Encoding.ASCII.GetBytes(content);
            return hmacsha.ComputeHash(bytes);
        }

        public static bool IsEmpty(string content)
        {
            bool flag = false;
            if (content != null)
            {
                if (content.ToLower().Replace("&nbsp;", string.Empty).Replace("<br>", string.Empty).Replace("<p>", string.Empty).Replace("</p>", string.Empty).Trim() == string.Empty)
                {
                    flag = true;
                }
                return flag;
            }
            return true;
        }

        public static string KillHTML(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = Regex.Replace(content, "<[^>]+>", "");
            }
            return content;
        }

        public static string KillHTMLButKeepWrap(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = content.Replace("<p>", "[p]");
                content = content.Replace("</p>", "[/p]");
                content = content.Replace("<P>", "[p]");
                content = content.Replace("</P>", "[/p]");
                Regex BrHtml = new Regex("<br(.*?)>", RegexOptions.IgnoreCase);
                content = BrHtml.Replace(content, "[br/]");

                content = Regex.Replace(content, "<[^>]+>", "");

                content = content.Replace("[p]", "<p>");
                content = content.Replace("[/p]", "</p>");
                content = content.Replace("[br/]", "<br/>");
            }
            return content;
        }

        public static string KillJapan(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = ClearIframe(content);
                content = content.Replace("ガ", "&#12460;");
                content = content.Replace("ギ", "&#12462;");
                content = content.Replace("ア", "&#12450;");
                content = content.Replace("ゲ", "&#12466;");
                content = content.Replace("ゴ", "&#12468;");
                content = content.Replace("ザ", "&#12470;");
                content = content.Replace("ジ", "&#12472;");
                content = content.Replace("ズ", "&#12474;");
                content = content.Replace("ゼ", "&#12476;");
                content = content.Replace("ゾ", "&#12478;");
                content = content.Replace("ダ", "&#12480;");
                content = content.Replace("ヂ", "&#12482;");
                content = content.Replace("ヅ", "&#12485;");
                content = content.Replace("デ", "&#12487;");
                content = content.Replace("ド", "&#12489;");
                content = content.Replace("バ", "&#12496;");
                content = content.Replace("パ", "&#12497;");
                content = content.Replace("ビ", "&#12499;");
                content = content.Replace("ピ", "&#12500;");
                content = content.Replace("ブ", "&#12502;");
                content = content.Replace("ブ", "&#12502;");
                content = content.Replace("プ", "&#12503;");
                content = content.Replace("ベ", "&#12505;");
                content = content.Replace("ペ", "&#12506;");
                content = content.Replace("ボ", "&#12508;");
                content = content.Replace("ポ", "&#12509;");
                content = content.Replace("ヴ", "&#12532;");
            }
            return content;
        }

        public static int MachCount(string content, string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase).Matches(content).Count;
        }

        public static string MD5(string content, int startPossition, int length)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(content, "MD5").Substring(startPossition, length);
        }

        public static string MD516(string content)
        {
            return MD5(content, 8, 0x10);
        }

        public static string MD532(string content)
        {
            return MD5(content, 0, 0x20);
        }

        public static string Password(string content, PasswordType passwordType)
        {
            string str = string.Empty;
            switch (passwordType)
            {
                case PasswordType.Clear:
                    return content;

                case PasswordType.MD516:
                    return MD516(content);

                case PasswordType.MD532:
                    return MD532(content);

                case PasswordType.MD5Twice:
                    return MD532(MD532(content));

                case PasswordType.SHA1:
                    return SHA1(content);
            }
            return str;
        }

        private static string PrepareKey(string key)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(key, "MD5").Substring(12, 8);
        }

        public static string Regexs(string html, string str1, string str2)
        {
            Regex regex = new Regex(str1, RegexOptions.IgnoreCase);
            return regex.Replace(html, str2);
        }

        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            int num2;
            int num3;
            int num5;
            int length = num2 = num3 = 0;
            string str = original.ToUpper();
            string str2 = pattern.ToUpper();
            int num4 = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chArray = new char[original.Length + Math.Max(0, num4)];
            while ((num3 = str.IndexOf(str2, num2)) != -1)
            {
                num5 = num2;
                while (num5 < num3)
                {
                    chArray[length++] = original[num5];
                    num5++;
                }
                num5 = 0;
                while (num5 < replacement.Length)
                {
                    chArray[length++] = replacement[num5];
                    num5++;
                }
                num2 = num3 + pattern.Length;
            }
            if (num2 == 0)
            {
                return original;
            }
            for (num5 = num2; num5 < original.Length; num5++)
            {
                chArray[length++] = original[num5];
            }
            return new string(chArray, 0, length);
        }

        public static string SearchSafe(string content)
        {
            if ((content != null) && (content != string.Empty))
            {
                content = KillJapan(content.Replace("'", string.Empty));
            }
            return content;
        }

        public static string SHA1(string content)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(content, "SHA1");
        }

        public static string Substring(string content, int length)
        {
            Regex regex = new Regex("[一-龥]|[︰-ﾠ]+", RegexOptions.Compiled);
            char[] chArray = content.ToCharArray();
            StringBuilder builder = new StringBuilder();
            int num = 0;
            bool flag = false;
            for (int i = 0; i < chArray.Length; i++)
            {
                if (regex.IsMatch(chArray[i].ToString()))
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                builder.Append(chArray[i]);
                if (num > (2 * length))
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                return (builder.ToString() + "..");
            }
            return builder.ToString();
        }

        public static string Substring(string content, int length, bool withPot)
        {
            Regex regex = new Regex("[一-龥]|[︰-ﾠ]+", RegexOptions.Compiled);
            char[] chArray = content.ToCharArray();
            StringBuilder builder = new StringBuilder();
            int num = 0;
            bool flag = false;
            for (int i = 0; i < chArray.Length; i++)
            {
                if (regex.IsMatch(chArray[i].ToString()))
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                builder.Append(chArray[i]);
                if (num > (2 * length))
                {
                    flag = true;
                    break;
                }
            }
            if (flag && withPot)
            {
                return (builder.ToString() + "..");
            }
            return builder.ToString();
        }

        public static string UBB(string content)
        {
            string input = string.Empty;
            if ((content != null) && (content != string.Empty))
            {
                Match match;
                input = content;
                Regex regex = new Regex(@"(\[b\])((.|\n)*?)(\[\/b\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<B>" + match.Groups[2].ToString() + "</B>");
                }
                regex = new Regex(@"(\[i\])((.|\n)*?)(\[\/i\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<I>" + match.Groups[2].ToString() + "</I>");
                }
                regex = new Regex(@"(\[u\])([ \S\t]*?)(\[\/u\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<U>" + match.Groups[2].ToString() + "</U>");
                }
                regex = new Regex(@"(\[sup\])([ \S\t]*?)(\[\/sup\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<SUP>" + match.Groups[2].ToString() + "</SUP>");
                }
                regex = new Regex(@"(\[sub\])([ \S\t]*?)(\[\/sub\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<SUB>" + match.Groups[2].ToString() + "</SUB>");
                }
                regex = new Regex(@"(\[email\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<A href=\"mailto:" + match.Groups[2].ToString() + "\" target=\"_blank\">" + match.Groups[2].ToString() + "</A>");
                }
                regex = new Regex(@"(\[email=([ \S\t]+)\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<A href=\"mailto:" + match.Groups[2].ToString() + "\" target=\"_blank\">" + match.Groups[3].ToString() + "</A>");
                }
                regex = new Regex(@"(\[size=([1-7])\])([ \S\t]*?)(\[\/size\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<FONT SIZE=" + match.Groups[2].ToString() + ">" + match.Groups[3].ToString() + "</FONT>");
                }
                regex = new Regex(@"(\[color=([\S]+)\])([ \S\t]*?)(\[\/color\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<FONT COLOR=" + match.Groups[2].ToString() + ">" + match.Groups[3].ToString() + "</FONT>");
                }
                regex = new Regex(@"(\[font=([\S]+)\])([ \S\t]*?)(\[\/font\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<FONT FACE=" + match.Groups[2].ToString() + ">" + match.Groups[3].ToString() + "</FONT>");
                }
                regex = new Regex(@"(\[picture\])([ \S\t]*?)(\[\/picture\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<a target=_blank href='" + match.Groups[2].ToString() + "'><IMG SRC='" + match.Groups[2].ToString() + "' border=0 onload='javascript:if(screen.width-333<this.width)this.width=screen.width-333'></a>");
                }
                regex = new Regex(@"(\[align=([\S]+)\])([ \S\t]*?)(\[\/align\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<P align=" + match.Groups[2].ToString() + ">" + match.Groups[3].ToString() + "</P>");
                }
                regex = new Regex(@"(\[h=([1-6])\])([ \S\t]*?)(\[\/h\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<H" + match.Groups[2].ToString() + ">" + match.Groups[3].ToString() + "</H" + match.Groups[2].ToString() + ">");
                }
                regex = new Regex(@"(\[list(=(A|a|I|i| ))?\]([ \S\t]*)\r\n)((\[\*\]([ \S\t]*\r\n))*?)(\[\/list\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    string str2 = match.Groups[5].ToString();
                    Regex regex2 = new Regex(@"\[\*\]([ \S\t]*\r\n?)", RegexOptions.IgnoreCase);
                    for (Match match2 = regex2.Match(str2); match2.Success; match2 = match2.NextMatch())
                    {
                        str2 = str2.Replace(match2.Groups[0].ToString(), "<LI>" + match2.Groups[1]);
                    }
                    input = input.Replace(match.Groups[0].ToString(), "<UL TYPE=\"" + match.Groups[3].ToString() + "\"><B>" + match.Groups[4].ToString() + "</B>" + str2 + "</UL>");
                }
                regex = new Regex(@"(\[shadow=)(\d*?),(#*\w*?),(\d*?)\]([ \S\t]*?)(\[\/shadow\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<TABLE WIDTH=" + match.Groups[2].ToString() + "STYLE=FILTER:SHADOW(COLOR=" + match.Groups[3].ToString() + ",STRENGTH=" + match.Groups[4].ToString() + ")>" + match.Groups[5].ToString() + "</TABLE>");
                }
                regex = new Regex(@"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([ \S\t]*?)(\[\/glow\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<TABLE WIDTH=" + match.Groups[2].ToString() + "  STYLE=FILTER:GLOW(COLOR=" + match.Groups[3].ToString() + ", STRENGTH=" + match.Groups[4].ToString() + ")>" + match.Groups[5].ToString() + "</TABLE>");
                }
                regex = new Regex(@"(\[center\])([ \S\t]*?)(\[\/center\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<CENTER>" + match.Groups[2].ToString() + "</CENTER>");
                }
                regex = new Regex(@"(\[em([\S\t]*?)\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<img src=emot/em" + match.Groups[2].ToString() + ".gif border=0 align=middle>");
                }
                regex = new Regex(@"(\[img\])([ \S\t]*?)(\[\/img\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<a target=_blank href='" + match.Groups[2].ToString() + "'><IMG SRC='" + match.Groups[2].ToString() + "' border=0 onload='javascript:if(screen.width-333<this.width)this.width=screen.width-333'></a>");
                }
                regex = new Regex(@"(\[file\])([ \S\t]*?)(\[\/file\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<img src=images/download.gif>&nbsp;<a target=_blank href='" + match.Groups[2].ToString() + "'>" + match.Groups[2].ToString() + "</a>");
                }
                regex = new Regex(@"(\[flash=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<a href='" + match.Groups[4].ToString() + "' TARGET=_blank><OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + "><PARAM NAME=movie VALUE='" + match.Groups[4].ToString() + "'><PARAM NAME=quality VALUE=high><param name=menu value=false><embed src='" + match.Groups[4].ToString() + "' quality=high menu=false pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + ">" + match.Groups[4].ToString() + "</embed></OBJECT>");
                }
                regex = new Regex(@"(\[flash\])([ \S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<a href='" + match.Groups[2].ToString() + "' TARGET=_blank><OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=400 height=400><PARAM NAME=movie VALUE='" + match.Groups[2].ToString() + "'><PARAM NAME=quality VALUE=high><param name=menu value=false><embed src='" + match.Groups[2].ToString() + "' quality=high menu=false pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash width=400 height=400>" + match.Groups[2].ToString() + "</embed></OBJECT>");
                }
                regex = new Regex(@"(\[swf\])([ \S\t]*?)(\[\/swf\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<a href='" + match.Groups[2].ToString() + "' TARGET=_blank><OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=400 height=400><PARAM NAME=movie VALUE='" + match.Groups[2].ToString() + "'><PARAM NAME=quality VALUE=high><param name=menu value=false><embed src='" + match.Groups[2].ToString() + "' quality=high menu=false pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash width=400 height=400>" + match.Groups[2].ToString() + "</embed></OBJECT>");
                }
                regex = new Regex(@"(\[dir=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/dir\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<object classid=clsid:166B1BCA-3F9C-11CF-8075-444553540000 codebase=http://download.macromedia.com/pub/shockwave/cabs/director/sw.cab#version=7,0,2,0 width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + "><param name=src value=" + match.Groups[4].ToString() + "><embed src=" + match.Groups[4].ToString() + " pluginspage=http://www.macromedia.com/shockwave/download/ width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + "></embed></object>");
                }
                regex = new Regex(@"(\[rm=)(\d*?),(\d*?),([\S\t]*?)\]([ \S\t]*?)(\[\/rm\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<OBJECT classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=OBJECT id=RAOCX width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + "><PARAM NAME=SRC VALUE='" + match.Groups[5].ToString() + "'><PARAM NAME=CONSOLE VALUE=Clip1><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=" + match.Groups[4].ToString() + "></OBJECT><br><OBJECT classid=CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=" + match.Groups[2].ToString() + "><PARAM NAME=SRC VALUE='" + match.Groups[5].ToString() + "'><PARAM NAME=AUTOSTART VALUE=" + match.Groups[4].ToString() + "><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=Clip1></OBJECT>");
                }
                regex = new Regex(@"(\[mp=)(\d*?),(\d*?),([\S\t]*?)\]([ \S\t]*?)(\[\/mp\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<object align=middle classid=CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + " ><PARAM NAME=AUTOSTART VALUE=" + match.Groups[4].ToString() + "><param name=Filename value='" + match.Groups[5].ToString() + "'><PARAM NAME=showstatusbar VALUE=1><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src='" + match.Groups[5].ToString() + "'  width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + " showstatusbar=1></embed></object>");
                }
                regex = new Regex(@"(\[qt=)(\d*?),(\d*?)\]([ \S\t]*?)(\[\/qt\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<embed src=" + match.Groups[4].ToString() + " width=" + match.Groups[2].ToString() + " height=" + match.Groups[3].ToString() + " autoplay=true loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/>");
                }
                regex = new Regex(@"(\[QUOTE\])((.|\n)*?)(\[\/QUOTE\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<DIV class=Quote>" + match.Groups[2].ToString() + "</DIV>");
                }
                regex = new Regex(@"(\[CODE\])((.|\n)*?)(\[\/CODE\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<DIV class=Code>" + match.Groups[2].ToString() + "</DIV>");
                }
                regex = new Regex(@"(\[move\])((.|\n)*?)(\[\/move\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<MARQUEE scrollamount=3>" + match.Groups[2].ToString() + "</MARQUEE>");
                }
                regex = new Regex(@"(\[fly\])((.|\n)*?)(\[\/fly\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<MARQUEE width=80% behavior=alternate scrollamount=3>" + match.Groups[2].ToString() + "</MARQUEE>");
                }
                regex = new Regex(@"(\[image\])([ \S\t]*?)(\[\/image\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<img src=\"" + match.Groups[2].ToString() + "\" border=0 align=middle><br>");
                }
                regex = new Regex(@"(\[url\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<A href=\"" + match.Groups[2].ToString() + "\" target=\"_blank\">" + match.Groups[2].ToString() + "</A>");
                }
                regex = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<A href=\"" + match.Groups[2].ToString() + "\" target=\"_blank\">" + match.Groups[3].ToString() + "</A>");
                }
                regex = new Regex(@"(\[download=([ \S\t]*?)\])([ \S\t]*?)(\[\/download\])", RegexOptions.IgnoreCase);
                for (match = regex.Match(input); match.Success; match = match.NextMatch())
                {
                    input = input.Replace(match.Groups[0].ToString(), "<img src=images/download.gif>&nbsp;<a target=_blank href='" + match.Groups[3].ToString() + "'>" + match.Groups[2].ToString() + "</a>");
                }
            }
            return input;
        }

        public static string GetUserName(string userName, int hideName)
        {
            if (hideName == 1)
            {
                if (userName != "")
                {
                    StringBuilder newStr = new StringBuilder();
                    char[] nameArr = userName.ToCharArray();
                    for (int si = 0; si < nameArr.Length; si++)
                    {
                        if (si < nameArr.Length - 1)
                        {
                            newStr.Append('*');
                        }
                        else
                        {
                            newStr.Append(nameArr[si]);
                        }
                    }
                    return newStr.ToString();
                }
                else
                {
                    return "游客";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    return userName;
                }
                else
                {
                    return "游客";
                }
            }
        }

        /// <summary>   
        /// 过滤xss攻击脚本   
        /// </summary>   
        /// <param name="input">传入字符串</param>   
        /// <returns>过滤后的字符串</returns>   
        public static string FilterXSS(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.    
            // 目的防止这样形式的入侵 ＜java\0script＞   
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到   
            string ret = System.Text.RegularExpressions.Regex.Replace(
                html, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            //替换所有可能的16进制构建的恶意代码   
            //<IMG SRC=&#X40&#X61&#X76&#X61&#X73&#X63&#X72&#X69&#X70&#X74&#X3A&#X61&_#X6C&#X65&#X72&#X74&#X28&#X27&#X58&#X53&#X53&#X27&#X29>   
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()~`;:?+/={}[]-_|'\"\\";
            for (int i = 0; i < chars.Length; i++)
            {
                ret = System.Text.RegularExpressions.Regex.Replace(ret, string.Concat("(&#[x|X]0{0,}", Convert.ToString((int)chars[i], 16).ToLower(), ";?)"),
                    chars[i].ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            //过滤\t, \n, \r构建的恶意代码  
            string[] keywords = {"javascript", "vbscript", "expression", "applet", "meta", "xml", "blink", "link", "style", "script", "embed", "object", "iframe", "frame", "frameset", "ilayer", "layer", "bgsound", "title", "base"  
        ,"onabort", "onactivate", "onafterprint", "onafterupdate", "onbeforeactivate", "onbeforecopy", "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus", "onbeforepaste", "onbeforeprint", "onbeforeunload", "onbeforeupdate", "onblur", "onbounce", "oncellchange", "onchange", "onclick", "oncontextmenu", "oncontrolselect", "oncopy", "oncut", "ondataavailable", "ondatasetchanged", "ondatasetcomplete", "ondblclick", "ondeactivate", "ondrag", "ondragend", "ondragenter", "ondragleave", "ondragover", "ondragstart", "ondrop", "onerror", "onerrorupdate", "onfilterchange", "onfinish", "onfocus", "onfocusin", "onfocusout", "onhelp", "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete", "onload", "onlosecapture", "onmousedown", "onmouseenter", "onmouseleave", "onmousemove", "onmouseout", "onmouseover", "onmouseup", "onmousewheel", "onmove", "onmoveend", "onmovestart", "onpaste", "onpropertychange", "onreadystatechange", "onreset", "onresize", "onresizeend", "onresizestart", "onrowenter", "onrowexit", "onrowsdelete", "onrowsinserted", "onscroll", "onselect", "onselectionchange", "onselectstart", "onstart", "onstop", "onsubmit", "onunload"};

            bool found = true;
            while (found)
            {
                var retBefore = ret;
                for (int i = 0; i < keywords.Length; i++)
                {
                    string pattern = "/";
                    for (int j = 0; j < keywords[i].Length; j++)
                    {
                        if (j > 0)
                            pattern = string.Concat(pattern, '(', "(&#[x|X]0{0,8}([9][a][b]);?)?", "|(&#0{0,8}([9][10][13]);?)?",
                                ")?");
                        pattern = string.Concat(pattern, keywords[i][j]);
                    }
                    string replacement = string.Concat(keywords[i].Substring(0, 2), "＜x＞", keywords[i].Substring(2));
                    ret = System.Text.RegularExpressions.Regex.Replace(ret, pattern, replacement, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (ret == retBefore)
                        found = false;
                }

            }

            return ret;
        }  
    }
}

