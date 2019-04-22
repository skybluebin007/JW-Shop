namespace SkyCES.EntLib
{
    using System;
    using System.Text.RegularExpressions;

    public class ForTag : BaseTag
    {
        private Regex rg1 = new Regex("<html:for init=\"([\\s\\S]+?)\" condtion=\"([\\s\\S]+?)\" expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("</html:for>", RegexOptions.None);

        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%for(" + match.Groups[1].ToString() + ";" + match.Groups[2].ToString() + ";" + match.Groups[3].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}%>");
            }
        }
    }
}

