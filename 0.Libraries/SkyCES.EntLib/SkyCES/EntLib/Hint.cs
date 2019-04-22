namespace SkyCES.EntLib
{
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [DefaultProperty("Text"), ToolboxData("<{0}:Hint runat=server></{0}:Hint>"), DefaultEvent("Click")]
    public class Hint : WebControl
    {
        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<!--提示层部分开始-->");
            builder.Append("<span id=\"hintdivup\" class=\"hintbox\">\r\n");
            builder.Append("<div class=\"hintboxdiv\">\r\n");
            builder.Append("<div><img src=\"" + this.HintImageUrl + "/commandbg.gif\" /></div>\r\n");
            builder.Append("<div class=\"messagetext\"><img src=\"" + this.HintImageUrl + "/dot.gif\" /><span id=\"hintinfoup\" ></span></div>\r\n");
            builder.Append("<div><img src=\"" + this.HintImageUrl + "/commandbg2.gif\" /></div>\r\n");
            builder.Append("</div>\r\n");
            builder.Append("<iframe id=\"hintiframeup\" class=\"hintframe\" frameborder=\"0\"></iframe>\r\n");
            builder.Append("</span>\r\n");
            builder.Append("<span id=\"hintdivdown\" class=\"hintbox\">\r\n");
            builder.Append("<div class=\"hintboxdiv\">\r\n");
            builder.Append("<div><img src=\"" + this.HintImageUrl + "/commandbg3.gif\" /></div>\r\n");
            builder.Append("<div class=\"messagetext\"><img src=\"" + this.HintImageUrl + "/dot.gif\" /><span id=\"hintinfodown\" ></span></div>\r\n");
            builder.Append("<div><img src=\"" + this.HintImageUrl + "/commandbg4.gif\" /></div>\r\n");
            builder.Append("</div>\r\n");
            builder.Append("<iframe id=\"hintiframedown\" class=\"hintframe\" frameborder=\"0\"></iframe>\r\n");
            builder.Append("</span>\r\n");
            builder.Append("<!--提示层部分结束-->\r\n");
            output.Write(builder.ToString());
        }

        [Bindable(true), DefaultValue(""), Category("Appearance")]
        public string HintImageUrl
        {
            get
            {
                if (base.ViewState["hintimageurl"] != null)
                {
                    return (string) base.ViewState["hintimageurl"];
                }
                return "/Admin/Style/Control";
            }
            set
            {
                base.ViewState["hintimageurl"] = value;
            }
        }
    }
}

