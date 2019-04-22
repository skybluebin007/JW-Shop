namespace SkyCES.EntLib
{
    using System;
    using System.Text.RegularExpressions;

    public class WhileTag : BaseTag
    {
        private Regex rg1 = new Regex("<html:while expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("</html:while>", RegexOptions.None);

        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%while(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<% }%>");
            }
        }
    }
}

