/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com r
*

*
********************************************************************/


using System.Web.Mvc;

namespace Jade.Shell.Areas.AppApi
{
    public class AppApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AppApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AppApi_default",
                "jade-api/{controller}/{action}/{id}",
                new { controller = "Member", action = "Login", id = UrlParameter.Optional }, 
                namespaces: new[] { "Jade.Shell.Areas.AppApi.Controllers" }
            );
        }
    }
}