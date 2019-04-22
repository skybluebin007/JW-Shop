namespace SkyCES.EntLib
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:TextBox runat=server></{0}:TextBox>"), DefaultProperty("")]
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        private RequiredFieldValidator canBeNullRFV = new RequiredFieldValidator();
        private string customErr = string.Empty;
        private int hintHeight = 50;
        private string hintInfo = string.Empty;
        private int hintLeftOffSet = 0;
        private string hintShowType = "up";
        private string hintTitle = string.Empty;
        private int hintTopOffSet = 0;
        private string maximumValue = string.Empty;
        private string minimumValue = string.Empty;
        private RangeValidator numberRV = new RangeValidator();
        private RegularExpressionValidator requiredFieldTypeREV = new RegularExpressionValidator();

        protected override void CreateChildControls()
        {
            if ((this.MaximumValue != string.Empty) || (this.MinimumValue != string.Empty))
            {
                this.numberRV.ControlToValidate = this.ID;
                this.numberRV.Type = ValidationDataType.Double;
                if ((this.MaximumValue != string.Empty) && (this.MinimumValue != string.Empty))
                {
                    this.numberRV.MaximumValue = this.MaximumValue;
                    this.numberRV.MinimumValue = this.MinimumValue;
                    this.numberRV.ErrorMessage = "* 当前输入数据应在" + this.MinimumValue + "和" + this.MaximumValue + "之间!";
                }
                else
                {
                    if (this.MaximumValue != string.Empty)
                    {
                        this.numberRV.MaximumValue = this.MaximumValue;
                        int num = -2147483648;
                        this.numberRV.MinimumValue = num.ToString();
                        this.numberRV.ErrorMessage = "* 当前输入数据允许最大值为" + this.MaximumValue;
                    }
                    if (this.MinimumValue != string.Empty)
                    {
                        this.numberRV.MinimumValue = this.MinimumValue;
                        this.numberRV.MaximumValue = 0x7fffffff.ToString();
                        this.numberRV.ErrorMessage = "* 当前输入数据允许最小值为" + this.MinimumValue;
                    }
                }
                this.numberRV.Display = ValidatorDisplay.Static;
                this.Controls.AddAt(0, this.numberRV);
            }
            if (((this.RequiredFieldType != null) && (this.RequiredFieldType != "")) && (this.RequiredFieldType != "暂无校验"))
            {
                this.requiredFieldTypeREV.Display = ValidatorDisplay.Dynamic;
                this.requiredFieldTypeREV.ControlToValidate = this.ID;
                switch (this.RequiredFieldType)
                {
                    case "数据校验":
                        this.requiredFieldTypeREV.ValidationExpression = @"^[-]?\d+$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 数字的格式不正确";
                        break;

                    case "电子邮箱":
                        this.requiredFieldTypeREV.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 邮箱的格式不正确";
                        break;

                    case "移动手机":
                        this.requiredFieldTypeREV.ValidationExpression = @"^1[3-9]\d{9}$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 手机号码格式不正确!";
                        break;

                    case "家用电话":
                        this.requiredFieldTypeREV.ValidationExpression = @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}|((\(\d{3}\) ?)|(\d{4}-))?\d{4}-\d{4}";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请依 (XXX)XXX-XXXX 格式或 (XXX)XXXX-XXXX 输入电话号码！";
                        break;

                    case "身份证号码":
                        this.requiredFieldTypeREV.ValidationExpression = @"^\d{15}$|^\d{18}$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请依15或18位数据的身份证号！";
                        break;

                    case "网页地址":
                        this.requiredFieldTypeREV.ValidationExpression = @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请输入正确的网址";
                        break;

                    case "日期":
                        this.requiredFieldTypeREV.ValidationExpression = (this.ValidationExpression != null) ? this.ValidationExpression : @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请输入正确的日期,如:2006-1-1";
                        break;

                    case "日期时间":
                        this.requiredFieldTypeREV.ValidationExpression = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))( (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)?$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请输入正确的日期时间,如: 2006-1-1 23:59:59 或者 2006-1-1";
                        break;

                    case "金额":
                        this.requiredFieldTypeREV.ValidationExpression = "^[-]?([0-9]|[0-9].[0-9]{0,2}|[1-9][0-9]*.[0-9]{0,2})$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请输入正确的金额";
                        break;

                    case "IP地址":
                        this.requiredFieldTypeREV.ValidationExpression = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请输入正确的IP地址";
                        break;

                    case "IP地址带端口":
                        this.requiredFieldTypeREV.ValidationExpression = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9]):\d{1,5}?$";
                        this.requiredFieldTypeREV.ErrorMessage = "* 请输入正确的带端口的IP地址";
                        break;

                    case "自定义验证表达式":
                        this.requiredFieldTypeREV.ValidationExpression = this.ValidationExpression;
                        this.requiredFieldTypeREV.ErrorMessage = (this.customErr == string.Empty) ? "* 输入的格式不符合您的要求" : this.customErr;
                        break;
                    case "正整数":
                        this.requiredFieldTypeREV.ValidationExpression = @"^[0-9]*[1-9][0-9]*$";
                        this.requiredFieldTypeREV.ErrorMessage = "*请输入正整数";
                        break;
                    case "百分比":
                        this.requiredFieldTypeREV.ValidationExpression = @"^([1-9][0-9]{0,1}|100)$";
                        this.requiredFieldTypeREV.ErrorMessage = "请输入1%-100%的百分比";
                        break;
                    case "整数":
                        this.requiredFieldTypeREV.ValidationExpression = @"^[0-9]*$";
                        this.requiredFieldTypeREV.ErrorMessage = "请输入整数";
                        break;
                }
                this.Controls.AddAt(0, this.requiredFieldTypeREV);
            }
            string canBeNull = this.CanBeNull;
            if ((canBeNull != null) && ((canBeNull != "可为空") && (canBeNull == "必填")))
            {
                this.canBeNullRFV.Display = ValidatorDisplay.Dynamic;
                this.canBeNullRFV.ControlToValidate = this.ID;
                this.canBeNullRFV.ErrorMessage = "* 不能为空!";
                this.Controls.AddAt(0, this.canBeNullRFV);
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (this.HintInfo != "")
            {
                base.Attributes.Add("onmouseover", string.Concat(new object[] { "showHintInfo(this,", this.HintLeftOffSet, ",", this.HintTopOffSet, ",'", this.HintTitle, "','", this.HintInfo, "','", this.HintHeight, "','", this.HintShowType, "')" }));
                base.Attributes.Add("onmouseout", "hideHintInfo()");
            }
            base.Render(output);
            if (this.CanBeNull == "必填")
            {
                output.Write(" <span style=\"color:red\">*</span>");
            }
            this.RenderChildren(output);
        }

        [TypeConverter(typeof(CanBeNullControlsConverter)), Category("Behavior"), Description("是否为空。"), DefaultValue("可为空"), Bindable(false)]
        public string CanBeNull
        {
            get
            {
                object obj2 = this.ViewState["CanBeNull"];
                return ((obj2 == null) ? "" : obj2.ToString());
            }
            set
            {
                this.ViewState["CanBeNull"] = value;
            }
        }

        [DefaultValue((string) null), Bindable(true), Category("Appearance")]
        public string CustomErr
        {
            get
            {
                return this.customErr;
            }
            set
            {
                this.customErr = value;
            }
        }

        [DefaultValue(130), Category("Appearance"), Bindable(true)]
        public int HintHeight
        {
            get
            {
                return this.hintHeight;
            }
            set
            {
                this.hintHeight = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public string HintInfo
        {
            get
            {
                return this.hintInfo;
            }
            set
            {
                this.hintInfo = value;
            }
        }

        [DefaultValue(0), Bindable(true), Category("Appearance")]
        public int HintLeftOffSet
        {
            get
            {
                return this.hintLeftOffSet;
            }
            set
            {
                this.hintLeftOffSet = value;
            }
        }

        [Bindable(true), DefaultValue("up"), Category("Appearance")]
        public string HintShowType
        {
            get
            {
                return this.hintShowType;
            }
            set
            {
                this.hintShowType = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public string HintTitle
        {
            get
            {
                return this.hintTitle;
            }
            set
            {
                this.hintTitle = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintTopOffSet
        {
            get
            {
                return this.hintTopOffSet;
            }
            set
            {
                this.hintTopOffSet = value;
            }
        }

        [Bindable(true), DefaultValue((string) null), Category("Appearance")]
        public string MaximumValue
        {
            get
            {
                return this.maximumValue;
            }
            set
            {
                this.maximumValue = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue((string) null)]
        public string MinimumValue
        {
            get
            {
                return this.minimumValue;
            }
            set
            {
                this.minimumValue = value;
            }
        }

        [Bindable(false), TypeConverter(typeof(RequiredFieldTypeControlsConverter)), Description("选择输入数据的验证类型。"), Category("Behavior"), DefaultValue("")]
        public string RequiredFieldType
        {
            get
            {
                object obj2 = this.ViewState["RequiredFieldType"];
                return ((obj2 == null) ? "" : obj2.ToString());
            }
            set
            {
                this.ViewState["RequiredFieldType"] = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string ValidationExpression
        {
            get
            {
                object obj2 = this.ViewState["ValidationExpression"];
                if ((obj2 == null) || (obj2.ToString().Trim() == ""))
                {
                    return null;
                }
                return obj2.ToString().ToLower();
            }
            set
            {
                this.ViewState["ValidationExpression"] = value;
            }
        }
    }
}

