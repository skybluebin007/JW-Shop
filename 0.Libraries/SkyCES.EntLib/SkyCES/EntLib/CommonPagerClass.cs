namespace SkyCES.EntLib
{
    using System;
    using System.Text;

    public class CommonPagerClass : BasePagerClass
    {
        public CommonPagerClass()
        {
            this.FirstLastType = false;
            this.ListType = false;
            this.DisCount = false;
        }
        public CommonPagerClass(bool show)
        {
            this.FirstLastType = show;
            this.ListType = show;
            this.DisCount = show;
        }

        public void Init(int currentPage, int pageSize, int count, bool isMobile)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.Count = count;
            if (isMobile)
            {
                this.ShowType = 4;
                this.PagerCSS = "pageCss";
                this.PreviewPage = "上一页";
                this.NextPage = "下一页";
            }
            else
            {
                this.ShowType = 2;
                this.PagerCSS = "pager";
                this.CurrentCSS = "current";
                this.PreviewPage = "<<上一页";
                this.NextPage = "下一页>>";
            }
        }



        public override string ShowPage()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;
                builder.Append("<div class=\"" + base.PagerCSS + "\">");
                if (base.ShowType == 1)
                {
                    if (base.DisCount)
                    {
                        builder.Append(string.Concat(new object[] { "<ul class=\"disCount\"><li>共有", base.Count, "条</li><li>当前", base.CurrentPage, "/", base.PageCount, "页</li></ul>" }));
                    }

                    if (base.PrenextType || base.FirstLastType)
                    {
                        builder.Append("<ul class=\"prenextType\">");
                        if (base.FirstLastType)
                        {
                            if (base.CurrentPage > 1)
                            {
                                builder.Append("<li><a href=" + base.URL.Replace("$Page", "1") + ">" + base.FirstPage + "</a></li>");
                            }
                            else
                            {
                                builder.Append("<li>" + base.FirstPage + "</li>");
                            }
                        }
                        if (base.PrenextType)
                        {
                            if ((base.CurrentPage - 1) > 0)
                            {
                                strArray = new string[5];
                                strArray[0] = "<li><a href=";
                                num2 = base.CurrentPage - 1;
                                strArray[1] = base.URL.Replace("$Page", num2.ToString());
                                strArray[2] = ">";
                                strArray[3] = base.PreviewPage;
                                strArray[4] = "</a></li>";
                                builder.Append(string.Concat(strArray));
                            }
                            else
                            {
                                builder.Append("<li>" + base.PreviewPage + "</li>");
                            }
                        }
                        builder.Append("</ul>");
                    }
                    if (base.NumType)
                    {
                        base.CountStartEndPage();
                        builder.Append("<ul class=\"numType\">");
                        for (num = base.StartPage; num <= base.EndPage; num++)
                        {
                            if (base.CurrentPage != num)
                            {
                                builder.Append(string.Concat(new object[] { "<li><a href=", base.URL.Replace("$Page", num.ToString()), ">", num, "</a></li>" }));
                            }
                            else
                            {
                                builder.Append("<li id=\"currentPage\">" + num + "</li>");
                            }
                        }
                        builder.Append("</ul>");
                    }
                    if (base.PrenextType || base.FirstLastType)
                    {
                        builder.Append("<ul class=\"prenextType\">");
                        if (base.PrenextType)
                        {
                            if ((base.CurrentPage + 1) <= base.PageCount)
                            {
                                strArray = new string[5];
                                strArray[0] = "<li><a href=";
                                num2 = base.CurrentPage + 1;
                                strArray[1] = base.URL.Replace("$Page", num2.ToString());
                                strArray[2] = ">";
                                strArray[3] = base.NextPage;
                                strArray[4] = "</a></li>";
                                builder.Append(string.Concat(strArray));
                            }
                            else
                            {
                                builder.Append("<li>" + base.NextPage + "</li>");
                            }
                        }
                        if (base.FirstLastType)
                        {
                            if (base.CurrentPage < base.PageCount)
                            {
                                builder.Append("<li><a href=" + base.URL.Replace("$Page", base.PageCount.ToString()) + ">" + base.LastPage + "</a></li>");
                            }
                            else
                            {
                                builder.Append("<li>" + base.LastPage + "</li>");
                            }
                        }
                        builder.Append("</ul>");
                    }
                    if (base.ListType)
                    {
                        builder.Append("<ul class=\"listType\">");
                        builder.Append("<li>跳转 ");
                        builder.Append("<select name=select onchange=\"window.location.href=this.value\">");
                        for (num = 1; num <= base.PageCount; num++)
                        {
                            if (num == base.CurrentPage)
                            {
                                builder.Append(string.Concat(new object[] { "<option value=", base.URL.Replace("$Page", num.ToString()), " selected=selected>", num, "</option>" }));
                            }
                            else
                            {
                                builder.Append(string.Concat(new object[] { "<option value=", base.URL.Replace("$Page", num.ToString()), ">", num, "</option>" }));
                            }
                        }
                        builder.Append("</select></li>");
                        builder.Append("</ul>");
                    }
                }
                else if (base.ShowType == 2)
                {
                    builder.Append("<div class=\"pagin\">");
                    //Page参数保留在最后
                    string page = System.Web.HttpContext.Current.Request.QueryString["Page"];
                    if (!string.IsNullOrEmpty(page))
                    {
                        string rawUrl = System.Web.HttpContext.Current.Request.RawUrl;
                        rawUrl = rawUrl.ToLower();
                        if (rawUrl.IndexOf("&page=") > -1)
                        {
                            rawUrl = rawUrl.Replace("&page=" + page, "");
                            base.URL = rawUrl + "&Page=$Page";
                        }
                        if (rawUrl.IndexOf("?page=") > -1)
                        {
                            rawUrl = rawUrl.Replace("?page=" + page, "");
                            base.URL = rawUrl + "?Page=$Page";
                        }
                    }

                    if (base.DisCount)
                    {
                        builder.Append(string.Concat(new object[] { "<a>共 ", base.Count, " 条 页次 ", base.CurrentPage, "/", base.PageCount, " 页</a>" }));
                    }
                    if (base.PrenextType || base.FirstLastType)
                    {
                        if (base.FirstLastType)
                        {
                            if (base.CurrentPage > 1)
                            {
                                builder.Append("<a href=" + base.URL.Replace("$Page", "1") + ">" + base.FirstPage + "</a>");
                            }
                            else
                            {
                                builder.Append("<a>" + base.FirstPage + "</a>");
                            }
                        }
                        if (base.PrenextType)
                        {
                            if ((base.CurrentPage - 1) > 0)
                            {
                                strArray = new string[5];
                                strArray[0] = "<a href=";
                                num2 = base.CurrentPage - 1;
                                strArray[1] = base.URL.Replace("$Page", num2.ToString());
                                strArray[2] = ">";
                                strArray[3] = base.PreviewPage;
                                strArray[4] = "</a>";
                                builder.Append(string.Concat(strArray));
                            }
                            else
                            {
                                builder.Append("<a>" + base.PreviewPage + "</a>");
                            }
                        }
                    }
                    if (base.NumType)
                    {
                        base.CountStartEndPage();
                        for (num = base.StartPage; num <= base.EndPage; num++)
                        {
                            if (base.CurrentPage != num)
                            {
                                builder.Append(string.Concat(new object[] { "<a href=", base.URL.Replace("$Page", num.ToString()), ">", num, "</a>" }));
                            }
                            else
                            {
                                builder.Append("<a class=\"" + base.CurrentCSS + "\">" + num + "</a>");
                            }
                        }
                    }
                    if (base.PrenextType || base.FirstLastType)
                    {
                        if (base.PrenextType)
                        {
                            if ((base.CurrentPage + 1) <= base.PageCount)
                            {
                                strArray = new string[5];
                                strArray[0] = "<a href=";
                                num2 = base.CurrentPage + 1;
                                strArray[1] = base.URL.Replace("$Page", num2.ToString());
                                strArray[2] = ">";
                                strArray[3] = base.NextPage;
                                strArray[4] = "</a>";
                                builder.Append(string.Concat(strArray));
                            }
                            else
                            {
                                builder.Append("<a>" + base.NextPage + "</a>");
                            }
                        }
                        if (base.FirstLastType)
                        {
                            if (base.CurrentPage < base.PageCount)
                            {
                                builder.Append("<a href=" + base.URL.Replace("$Page", base.PageCount.ToString()) + ">" + base.LastPage + "</a>");
                            }
                            else
                            {
                                builder.Append("<a>" + base.LastPage + "</a>");
                            }
                        }
                    }
                    builder.Append("</div>");
                }
                else if (base.ShowType == 3)
                {
                    if (base.PrenextType)
                    {
                        if ((base.CurrentPage - 1) > 0)
                        {
                            strArray = new string[5];
                            strArray[0] = "<a class=\"prev\" href=";
                            num2 = base.CurrentPage - 1;
                            strArray[1] = base.URL.Replace("$Page", num2.ToString());
                            strArray[2] = ">";
                            strArray[3] = base.PreviewPage;
                            strArray[4] = "</a>";
                            builder.Append(string.Concat(strArray));
                        }
                        else
                        {
                            builder.Append("<a class=\"prev\" href=\"javascript:void(0);\" onclick=\"alert('没有了!');\">" + base.PreviewPage + "</a>");
                        }
                        if ((base.CurrentPage + 1) <= base.PageCount)
                        {
                            strArray = new string[5];
                            strArray[0] = "<a class=\"next\" href=";
                            num2 = base.CurrentPage + 1;
                            strArray[1] = base.URL.Replace("$Page", num2.ToString());
                            strArray[2] = ">";
                            strArray[3] = base.NextPage;
                            strArray[4] = "</a>";
                            builder.Append(string.Concat(strArray));
                        }
                        else
                        {
                            builder.Append("<a class=\"next\" href=\"javascript:void(0);\" onclick=\"alert('没有了!');\">" + base.NextPage + "</a>");

                        }
                    }
                }
                else if (base.ShowType == 4)
                {
                    builder.Append("<ul data-am-widget=\"pagination\" class=\"am-pagination am-pagination-select\">");

                    if ((base.CurrentPage - 1) > 0)
                    {
                        strArray = new string[5];
                        strArray[0] = "<li class=\am-pagination-prev\"><a href=";
                        num2 = base.CurrentPage - 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.PreviewPage;
                        strArray[4] = "</a></li>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append(string.Concat("<li class=\am-pagination-prev\"><a href=\"javascript:void(0);\" >", base.PreviewPage, "</a></li>"));
                    }

                    builder.Append("<li class=\"am-pagination-select\">");
                    builder.Append("<select onchange=\"window.location.href=this.value\">");
                    for (int i = 1; i <= base.PageCount; i++)
                    {
                        strArray = new string[8];
                        strArray[0] = "<option value=\"";
                        strArray[1] = base.URL.Replace("$Page", i.ToString()) + "\"";
                        strArray[2] = base.CurrentPage == i ? " selected" : "";
                        strArray[3] = ">";
                        strArray[4] = i.ToString();
                        strArray[5] = "/";
                        strArray[6] = base.PageCount.ToString();
                        strArray[7] = "</option>";
                        builder.Append(string.Concat(strArray));
                    }
                    builder.Append("</select>");
                    builder.Append("</li>");

                    if ((base.CurrentPage + 1) <= base.PageCount)
                    {
                        strArray = new string[5];
                        strArray[0] = "<li class=\"am-pagination-next\"><a href=";
                        num2 = base.CurrentPage + 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.NextPage;
                        strArray[4] = "</a></li>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append(string.Concat("<li class=\"am-pagination-next\"><a href=\"javascript:void(0);\" >", base.NextPage, "</a></li>"));
                    }
                }
                builder.Append("</div>");
            }
            return builder.ToString();
        }


        public override string ShowLoadPage()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;
                builder.Append("<div class=\"page fr\">");
                //if (base.DisCount)
                //{
                //    builder.Append(string.Concat(new object[] { "<span>共" + base.PageCount + "页，" + base.Count + "条记录</span>" }));
                //}
                if (base.FirstLastType)
                {
                    if (base.CurrentPage > 1)
                    {
                        builder.Append("<span class=\"p-num fl\"> <a href=" + base.URL.Replace("$Page", "1") + ">" + base.FirstPage + "</a></span>");
                    }
                    else
                    {
                        builder.Append("<span class=\"p-num fl\"><a >" + base.FirstPage + "</a></span>");
                    }
                }
                if (base.PrenextType)
                {
                    if ((base.CurrentPage - 1) > 0)
                    {
                        strArray = new string[5];
                        strArray[0] = "<a href=";
                        num2 = base.CurrentPage - 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.PreviewPage;
                        strArray[4] = "</a>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<a class=\"prev disabled\">" + base.PreviewPage + "</a>");
                    }
                }
                if (base.NumType)
                {
                    base.CountStartEndPage();
                    for (num = base.StartPage; num <= base.EndPage; num++)
                    {
                        if (base.CurrentPage != num)
                        {
                            builder.Append(string.Concat(new object[] { "<a href=", base.URL.Replace("$Page", num.ToString()), ">", num, "</a>" }));
                        }
                        else
                        {
                            builder.Append("<a class=\"cur\">" + num + "</a>");
                        }
                    }
                }
                if (base.PrenextType)
                {
                    if ((base.CurrentPage + 1) <= base.PageCount)
                    {
                        strArray = new string[5];
                        strArray[0] = "<a href=";
                        num2 = base.CurrentPage + 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.NextPage;
                        strArray[4] = "</a>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<a class=\"prev disabled\">" + base.NextPage + "</a>");
                    }
                }
                if (base.FirstLastType)
                {
                    if (base.CurrentPage < base.PageCount)
                    {
                        builder.Append("<a href=" + base.URL.Replace("$Page", base.PageCount.ToString()) + ">" + base.LastPage + "</a>");
                    }
                    else
                    {
                        builder.Append("<a>" + base.LastPage + "</a>");
                    }
                }

                builder.Append("<span class=\"p-skip fl\"> <em>共<b>" + PageCount + "</b>页&nbsp;到第</em></span>");
                builder.Append("<input type=\"text\" name=\"\" id=\"userpagenum\" value=\"" + base.CurrentPage + "\" /><em>页</em>");
                builder.Append("<a class=\"btn btn-default\" href=\"javascript:;\">确定</a> </span>");
                builder.Append("<input type=\"hidden\" value=\"" + base.URL + "\" id=\"pagewz\" />");
                builder.Append("<input type=\"hidden\" value=\"" + base.PageCount + "\" id=\"pagemaxCount\" />");
                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public override string ShowLoadPages()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;
                builder.Append("<div class=\"f_pager fr\">");

                #region 上一页
                if ((base.CurrentPage - 1) > 0)
                {
                    builder.Append("<a href=\"" + base.URL.Replace("$Page", (base.CurrentPage - 1).ToString()) + "\" title=\"上一页\" class=\"next\">&lt;</a> ");
                }
                else
                {
                    builder.Append("<a href=\"\" title=\"\" class=\"prev\">&lt;</a> ");
                }
                #endregion
                
                #region 下一页
                if ((base.CurrentPage + 1) <= base.PageCount)
                {
                    builder.Append("<a href=\"" + base.URL.Replace("$Page", (base.CurrentPage + 1).ToString()) + "\" title=\"\" class=\"next\">&gt;</a>");
                }
                else
                {
                    builder.Append("<a href=\"\" title=\"\" class=\"prev\">&gt;</a>");
                }
                #endregion
                builder.Append("</div>");

                builder.Append("<div class=\"f_num fr\">");
                builder.Append("<p class=\"p1 fl\"> 共<span>" + Count + "</span>件商品</p>");
                builder.Append(" <p class=\"p2 fl\">  <span>" + base.CurrentPage + "</span>/" + base.PageCount + "</p>");
                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public override string ShowPages()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;
                builder.Append("<div class=\"pager\">");
                if (base.DisCount)
                {
                    builder.Append(string.Concat(new object[] { "<span>共" + base.PageCount + "页，" + base.Count + "条记录</span>" }));
                }
                if (base.FirstLastType)
                {
                    if (base.CurrentPage > 1)
                    {
                        builder.Append("<a href=" + base.URL.Replace("$Page", "1") + ">" + base.FirstPage + "</a>");
                    }
                    else
                    {
                        builder.Append("<span>" + base.FirstPage + "</span>");
                    }
                }
                if (base.PrenextType)
                {
                    if ((base.CurrentPage - 1) > 0)
                    {
                        strArray = new string[5];
                        strArray[0] = "<a href=";
                        num2 = base.CurrentPage - 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.PreviewPage;
                        strArray[4] = "</a>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<span>" + base.PreviewPage + "</span>");
                    }
                }
                if (base.NumType)
                {
                    base.CountStartEndPage();
                    for (num = base.StartPage; num <= base.EndPage; num++)
                    {
                        if (base.CurrentPage != num)
                        {
                            builder.Append(string.Concat(new object[] { "<a href=", base.URL.Replace("$Page", num.ToString()), ">", num, "</a>" }));
                        }
                        else
                        {
                            builder.Append("<span class=\"cur\">" + num + "</span>");
                        }
                    }
                }
                if (base.PrenextType)
                {
                    if ((base.CurrentPage + 1) <= base.PageCount)
                    {
                        strArray = new string[5];
                        strArray[0] = "<a href=";
                        num2 = base.CurrentPage + 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.NextPage;
                        strArray[4] = "</a>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<span>" + base.NextPage + "</span>");
                    }
                }
                if (base.FirstLastType)
                {
                    if (base.CurrentPage < base.PageCount)
                    {
                        builder.Append("<a href=" + base.URL.Replace("$Page", base.PageCount.ToString()) + ">" + base.LastPage + "</a>");
                    }
                    else
                    {
                        builder.Append("<span>" + base.LastPage + "</span>");
                    }
                }
                if (base.ListType)
                {
                    //builder.Append("<ul class=\"listType\">");
                    //builder.Append("<li>跳转 ");               
                    //builder.Append("<select name=select onchange=\"window.location.href=this.value\">");
                    //for (num = 1; num <= base.PageCount; num++)
                    //{
                    //    if (num == base.CurrentPage)
                    //    {
                    //        builder.Append(string.Concat(new object[] { "<option value=", base.URL.Replace("$Page", num.ToString()), " selected=selected>", num, "</option>" }));
                    //    }
                    //    else
                    //    {
                    //        builder.Append(string.Concat(new object[] { "<option value=", base.URL.Replace("$Page", num.ToString()), ">", num, "</option>" }));
                    //    }
                    //}                
                    //builder.Append("</select></li>");
                    //builder.Append("</ul>");
                    builder.Append("<em>转到第</em>");
                    builder.Append("<input type=\"text\" class=\"pnum\" value=\"" + base.CurrentPage + "\" onkeyup=\"value=value.replace(/[^\\d]/g,'')\" onbeforepaste=\"clipboardData.setData('text',clipboardData.getData('text').replace(/[^\\d]/g,''))\" onchange=\"if(this.value<=0){window.location.href='" + base.URL.Replace("$Page", "1") + "'}else if(this.value>" + base.PageCount + "){window.location.href='" + base.URL.Replace("$Page", base.PageCount.ToString()) + "'}else{window.location.href='" + base.URL.Replace("$Page", "'+this.value") + "}\" />");
                    builder.Append(" 页");
                }
                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public override string ShowHelpPage()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;
                builder.Append("<div class=\"page fr\">");
                //if (base.DisCount)
                //{
                //    builder.Append(string.Concat(new object[] { "<span>共" + base.PageCount + "页，" + base.Count + "条记录</span>" }));
                //}
                if (base.FirstLastType)
                {
                    if (base.CurrentPage > 1)
                    {
                        builder.Append("<span class=\"p-num fl\"> <a href=" + base.URL.Replace("$Page", "1") + ">" + base.FirstPage + "</a></span>");
                    }
                    else
                    {
                        builder.Append("<span class=\"p-num fl\"><a >" + base.FirstPage + "</a></span>");
                    }
                }
                if (base.PrenextType)
                {
                    if ((base.CurrentPage - 1) > 0)
                    {
                        strArray = new string[5];
                        strArray[0] = "<a href=";
                        num2 = base.CurrentPage - 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.PreviewPage;
                        strArray[4] = "</a>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<a class=\"prev disabled\">" + base.PreviewPage + "</a>");
                    }
                }
                if (base.NumType)
                {
                    base.CountStartEndPage();
                    for (num = base.StartPage; num <= base.EndPage; num++)
                    {
                        if (base.CurrentPage != num)
                        {
                            builder.Append(string.Concat(new object[] { "<a href=", base.URL.Replace("$Page", num.ToString()), ">", num, "</a>" }));
                        }
                        else
                        {
                            builder.Append("<a class=\"cur\">" + num + "</a>");
                        }
                    }
                }
                if (base.PrenextType)
                {
                    if ((base.CurrentPage + 1) <= base.PageCount)
                    {
                        strArray = new string[5];
                        strArray[0] = "<a href=";
                        num2 = base.CurrentPage + 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.NextPage;
                        strArray[4] = "</a>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<a class=\"prev disabled\">" + base.NextPage + "</a>");
                    }
                }
                if (base.FirstLastType)
                {
                    if (base.CurrentPage < base.PageCount)
                    {
                        builder.Append("<a href=" + base.URL.Replace("$Page", base.PageCount.ToString()) + ">" + base.LastPage + "</a>");
                    }
                    else
                    {
                        builder.Append("<a>" + base.LastPage + "</a>");
                    }
                }

           
                builder.Append("</div>");
            }
            return builder.ToString();
        }
    }
}