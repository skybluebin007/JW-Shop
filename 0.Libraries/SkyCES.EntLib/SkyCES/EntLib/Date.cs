namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [DefaultProperty(""), ToolboxData("<{0}:Date runat=server></{0}:Date>")]
    public class Date : WebControl, IPostBackDataHandler
    {
        private int endYear = 0x7d8;
        private string postDate = string.Empty;
        private int startYear = 0x7c0;

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string str = postCollection[postDataKey];
            if (str != string.Concat(new object[] { this.Year, ",", this.Month, ",", this.Day }))
            {
                this.postDate = str;
                return true;
            }
            return false;
        }

        public virtual void RaisePostDataChangedEvent()
        {
            if (this.postDate != string.Empty)
            {
                string[] strArray = this.postDate.Split(new char[] { ',' });
                this.Year = Convert.ToInt32(strArray[0]);
                this.Month = Convert.ToInt32(strArray[1]);
                this.Day = Convert.ToInt32(strArray[2]);
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<select name=\"" + this.UniqueID + "\" id=\"Year\" onchange=\"getDays()\">\r\n");
            for (int i = this.startYear; i < this.EndYear; i++)
            {
                if (this.Year == i)
                {
                    builder.Append(string.Concat(new object[] { "<option value=\"", i, "\" selected=selected>", i, "</option>\r\n" }));
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "<option value=\"", i, "\">", i, "</option>\r\n" }));
                }
            }
            builder.Append("</select>");
            builder.Append(" 年 \r\n");
            builder.Append("<select name=\"" + this.UniqueID + "\" id=\"Month\" onchange=\"getDays()\">\r\n");
            for (int j = 1; j < 13; j++)
            {
                if (this.Month == j)
                {
                    builder.Append(string.Concat(new object[] { "<option value=\"", j, "\" selected=selected>", j, "</option>\r\n" }));
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "<option value=\"", j, "\">", j, "</option>\r\n" }));
                }
            }
            builder.Append("</select>");
            builder.Append(" 月 \r\n");
            builder.Append("<select name=\"" + this.UniqueID + "\" id=\"Day\">\r\n");
            object[] objArray = new object[] { this.Year, "-", (Convert.ToInt16(this.Month) + 1).ToString(), "-1" };
            int num3 = Convert.ToInt16(Convert.ToDateTime(string.Concat(objArray)).AddDays(-1.0).Day);
            for (int k = 1; k <= num3; k++)
            {
                if (this.Day == k)
                {
                    builder.Append(string.Concat(new object[] { "<option value=\"", k, "\" selected=selected>", k, "</option>\r\n" }));
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "<option value=\"", k, "\">", k, "</option>\r\n" }));
                }
            }
            builder.Append("</select>");
            builder.Append(" 日 \r\n");
            output.Write(builder.ToString());
        }

        [Category("Appearance"), DefaultValue(""), Bindable(true)]
        public int Day
        {
            get
            {
                return ((this.ViewState["Day"] != null) ? Convert.ToInt32(this.ViewState["Day"]) : 1);
            }
            set
            {
                this.ViewState["Day"] = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public int EndYear
        {
            get
            {
                return this.endYear;
            }
            set
            {
                this.endYear = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public int Month
        {
            get
            {
                return ((this.ViewState["Month"] != null) ? Convert.ToInt32(this.ViewState["Month"]) : 1);
            }
            set
            {
                this.ViewState["Month"] = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public int StartYear
        {
            get
            {
                return this.startYear;
            }
            set
            {
                this.startYear = value;
                this.Year = this.startYear;
            }
        }

        [Bindable(true), DefaultValue(""), Category("Appearance")]
        public string Text
        {
            get
            {
                return string.Concat(new object[] { this.Year, "-", this.Month, "-", this.Day });
            }
            set
            {
                string str = value;
                if (str != string.Empty)
                {
                    this.ViewState["Year"] = str.Split(new char[] { '-' })[0];
                    this.ViewState["Month"] = str.Split(new char[] { '-' })[1];
                    this.ViewState["Day"] = str.Split(new char[] { '-' })[2];
                }
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public int Year
        {
            get
            {
                return ((this.ViewState["Year"] != null) ? Convert.ToInt32(this.ViewState["Year"]) : 0x7d0);
            }
            set
            {
                this.ViewState["Year"] = value;
            }
        }
    }
}

