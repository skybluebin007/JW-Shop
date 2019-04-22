namespace SkyCES.EntLib
{
    using System;
    using System.Text.RegularExpressions;

    public class SwitchTag : BaseTag
    {
        private Regex rg1 = new Regex("<html:switch name=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("<html:case value=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg3 = new Regex("<html:default>", RegexOptions.None);
        private Regex rg4 = new Regex("</html:switch>", RegexOptions.None);

        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%switch(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%case " + match.Groups[1].ToString() + ":%>");
            }
            foreach (Match match in this.rg3.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%default:%>");
            }
            foreach (Match match in this.rg4.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%\r\n }%>");
            }
        }
    }
}

