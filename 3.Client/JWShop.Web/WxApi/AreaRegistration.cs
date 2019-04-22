using System.Web.Mvc;

namespace JWShop.XcxApi
{
    public class XcxApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "XcxApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "XcxApi_default",
                url: "XcxApi/{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "List", area = "XcxApi", id = UrlParameter.Optional },
                namespaces: new[] { "JWShop.XcxApi.Controllers" }
            );
        }
    }
}