namespace SkyCES.EntLib
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;

    [DefaultProperty(""), ToolboxData("<{0}:Page runat=server></{0}:Page>")]
    public class CommonPager : BasePager
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CommonPagerClass class2 = new CommonPagerClass(true);
            base.BasePagerClass = class2;
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(base.BasePagerClass.ShowPages());
        }
    }
}

