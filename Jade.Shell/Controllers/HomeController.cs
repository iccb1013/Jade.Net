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

namespace Jade.Shell.Controllers
{
    public class HomeController : ShellBaseController
    {
        [AllowedAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowedAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Login");
        }

    }
}