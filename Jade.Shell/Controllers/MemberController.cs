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


using Jade.Core;
using Jade.Shell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Controllers
{
    public class MemberController : ShellBaseController
    {
        #region Member

        public ActionResult Member()
        {
            if (this.UserContext.AuthorizationKeyVerify("Member") == false)
            {
                return RedirectToAction("Password", "PersonalCenter");
            }

            return View();
        }

        public ActionResult MemberEdit()
        {
            return View();
        }

        public ActionResult MemberSelector()
        {
            return View();
        }

        #endregion

        #region Point

        public ActionResult PointExchange()
        {
            return View();
        }

        public ActionResult PointExchangeEdit()
        {
            return View();
        }

        #endregion

        #region PushMessage

        public ActionResult PushMessage()
        {
            return View();
        }

        public ActionResult PushMessageEdit()
        {
            return View();
        }

        public ActionResult PushMessageHistory()
        {
            return View();
        }

        public ActionResult PushMessageHistoryEdit()
        {
            return View();
        }

        public ActionResult PushMessageMobilePage(int id)
        {
            PushMessageMobilePageViewModel model = new PushMessageMobilePageViewModel();
            model.Message = PushMessageManager.Instance.GetPushMessage(id);
            return View(model);
        }

        #endregion
    }
}