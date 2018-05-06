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


using AutoMapper;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Controllers
{
    public class ShellBaseController : BaseController
    {
        public ShellUserContext UserContext
        {
            get;set;
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
                //重定向到登录页面
                filterContext.Result = new RedirectResult("/Home/Login");
                return;
            }

            ViewBag.UserContext = UserContext;
        }
    }
}