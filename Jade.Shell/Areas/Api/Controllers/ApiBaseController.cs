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


using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class ApiBaseController : BaseController
    {
        public ShellUserContext UserContext
        {
            get; set;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            object[] objAllowedAnonymousArray =
               filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowedAnonymous), false);
            if (objAllowedAnonymousArray.Length > 0)
                return;

            UserContext = SessionContainer.GetUserContext(HttpContext) as ShellUserContext;

            if (UserContext == null)
            {
                //返回登录超时的消息
            }
        }
    }
}