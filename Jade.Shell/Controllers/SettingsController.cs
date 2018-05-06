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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Controllers
{
    public class SettingsController : ShellBaseController
    {
        public new ActionResult User()
        {
            return View();
        }

        public ActionResult UserEdit()
        {
            return View();
        }

        public ActionResult MultipleUserSelector()
        {
            return View();
        }

        public ActionResult Role()
        {
            return View();
        }

        public ActionResult RoleEdit()
        {
            return View();
        }

        public ActionResult RoleUser()
        {
            return View();
        }
    }
}