namespace SkyCES.EntLib
{
    using System;
    using System.Text.RegularExpressions;

    public class IfTag : BaseTag
    {
        private Regex rg1 = new Regex("<html:if expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("<html:elseif expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg3 = new Regex(@"<html:else>([\s\S]+?)", RegexOptions.None);
        private Regex rg4 = new Regex("</html:if>", RegexOptions.None);

        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%if(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}\r\nelse if(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg3.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}\r\nelse\r\n{%>" + match.Groups[1].ToString());
            }
            foreach (Match match in this.rg4.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<% }%>");
            }
        }
    }
}

