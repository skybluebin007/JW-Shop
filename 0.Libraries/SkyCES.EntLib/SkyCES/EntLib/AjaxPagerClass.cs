namespace SkyCES.EntLib
{
    using System;
    using System.Text;

    public class AjaxPagerClass : BasePagerClass
    {
        public AjaxPagerClass()
        {
            this.ShowType = 1;
            this.PagerCSS = "pager";
            this.CurrentCSS = "current";
            this.PreviewPage = "<<上一页";
            this.NextPage = "下一页>>";
        }
        public AjaxPagerClass(bool isMobile)
        {
            if (isMobile)
            {
                this.ShowType = 2;
                this.NumType = true;
                this.PagerCSS = "pager";
                this.PreviewPage = "上一页";
                this.NextPage = "下一页";
            }
        }

        public override string ShowLoadPage()
        { return ""; }

        public override string ShowLoadPages()
        { return ""; }
        public override string ShowPage()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;

                //PC端
                if (base.ShowType == 1)
                {
                    builder.Append("<div class=\"" + base.PagerCSS + "\">");
                    builder.Append("<div class=\"pagin\">");
                    if (base.PrenextType)
                    {
                        if ((base.CurrentPage - 1) > 0)
                        {
                            strArray = new string[5];
                            strArray[0] = "<a href=javascript:goPage(";
                            num2 = base.CurrentPage - 1;
                            strArray[1] = num2.ToString();
                            strArray[2] = ") class=\"link\">";
                            strArray[3] = base.PreviewPage;
                            strArray[4] = "</a>";
                            builder.Append(string.Concat(strArray));
                        }
                        else
                        {
                            builder.Append("<a class=\"link\" href=\"javascript:;\">" + base.PreviewPage + "</a>");
                        }
                    }
                    if (base.NumType)
                    {
                        base.CountStartEndPage();
                        for (num = base.StartPage; num <= base.EndPage; num++)
                        {
                            if (base.CurrentPage != num)
                            {
                                builder.Append(string.Concat(new object[] { "<a href=javascript:goPage(", num.ToString(), ")>", num, "</a>" }));
                            }
                            else
                            {
                                builder.Append("<a class=\"" + base.CurrentCSS + "\" href=\"javascript:;\">" + num + "</a>");
                            }
                        }
                    }
                    if (base.PrenextType)
                    {
                        if ((base.CurrentPage + 1) <= base.PageCount)
                        {
                            strArray = new string[5];
                            strArray[0] = "<a href=javascript:goPage(";
                            num2 = base.CurrentPage + 1;
                            strArray[1] = num2.ToString();
                            strArray[2] = ") class=\"link\">";
                            strArray[3] = base.NextPage;
                            strArray[4] = "</a>";
                            builder.Append(string.Concat(strArray));
                        }
                        else
                        {
                            builder.Append("<a class=\"link\" href=\"javascript:;\">" + base.NextPage + "</a>");
                        }
                    }
                    builder.Append("</div>");
                    builder.Append("</div>");
                }

                //移动端
                if (base.ShowType == 2)
                {
                    builder.Append("<div class=\"" + base.PagerCSS + "\">");
                    if (base.PrenextType)
                    {
                        if ((base.CurrentPage - 1) > 0)
                        {
                            strArray = new string[5];
                            strArray[0] = "<a href=javascript:goPage(";
                            num2 = base.CurrentPage - 1;
                            strArray[1] = num2.ToString();
                            strArray[2] = ") class=\"prev\">";
                            strArray[3] = base.PreviewPage;
                            strArray[4] = "</a>";
                            builder.Append(string.Concat(strArray));
                        }
                        else
                        {
                            builder.Append("<a class=\"prev\" href=\"javascript:;\">" + base.PreviewPage + "</a>");
                        }
                    }
                    if (base.NumType)
                    {
                        builder.Append("<div>" + base.CurrentPage + "/" + base.PageCount + "</div>");
                        //builder.Append("<div>" + base.CurrentPage + "/" + base.PageCount + " v");
                        //builder.Append("<ul style=\"display:none\">");
                        //for (int i = 1; i <= base.PageCount; i++)
                        //{
                        //    builder.Append("<li><a href=javascript:goPage(" + i + ")>" + "</a></li>");
                        //}
                        //builder.Append("</ul>");
                        //builder.Append("</div>");
                    }
                    if (base.PrenextType)
                    {
                        if ((base.CurrentPage + 1) <= base.PageCount)
                        {
                            strArray = new string[5];
                            strArray[0] = "<a href=javascript:goPage(";
                            num2 = base.CurrentPage + 1;
                            strArray[1] = num2.ToString();
                            strArray[2] = ") class=\"next\">";
                            strArray[3] = base.NextPage;
                            strArray[4] = "</a>";
                            builder.Append(string.Concat(strArray));
                        }
                        else
                        {
                            builder.Append("<a class=\"next\" href=\"javascript:;\">" + base.NextPage + "</a>");
                        }
                    }
                    builder.Append("</div>");
                }
            }
            return builder.ToString();
        }

        public override string ShowPages()
        {
            throw new NotImplementedException();
        }
        public override string ShowHelpPage()
        { return ""; }
    }
}

