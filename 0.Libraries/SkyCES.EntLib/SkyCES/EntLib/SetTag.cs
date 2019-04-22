namespace SkyCES.EntLib
{
    using System;
    using System.Text.RegularExpressions;

    public class SetTag : BaseTag
    {
        private Regex rg = new Regex(@"<\$([\s\S]+?)\$>", RegexOptions.None);

        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%=" + match.Groups[1].ToString() + "%>");
            }
        }
    }
}

