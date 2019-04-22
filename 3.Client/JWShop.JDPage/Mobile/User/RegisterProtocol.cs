using JWShop.Common;

namespace JWShop.Page.Mobile
{
    public class RegisterProtocol : CommonBasePage
    {
        protected string protocol = "";

        protected override void PageLoad()
        {
            base.PageLoad();

            protocol = ShopConfig.ReadConfigInfo().Agreement;
            Title = "注册协议";
        }

    }
}