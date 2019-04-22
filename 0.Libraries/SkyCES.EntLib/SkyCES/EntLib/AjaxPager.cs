namespace SkyCES.EntLib
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;

    [ToolboxData("<{0}:Page runat=server></{0}:Page>"), DefaultProperty("")]
    public class AjaxPager : BasePager
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AjaxPagerClass class2 = new AjaxPagerClass();
            base.BasePagerClass = class2;
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(base.BasePagerClass.ShowPage());
        }
    }
}

