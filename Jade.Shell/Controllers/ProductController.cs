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
    public class ProductController : ShellBaseController
    {
        #region Product

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult ProductEdit()
        {
            return View();
        }

        public ActionResult ProductCatalog()
        {
            return View();
        }

        public ActionResult ProductCatalogEdit()
        {
            return View();
        }

        public ActionResult ProductCatalogSelector()
        {
            return View();
        }

        public ActionResult ProductAttribute()
        {
            return View();
        }

        public ActionResult ProductAttributeEdit()
        {
            return View();
        }

        public ActionResult ProductAttributeSelector()
        {
            return View();
        }


        #endregion

        #region ProductQRCode

        public ActionResult ProductQRCode()
        {
            return View();
        }

        public ActionResult ProductQRCodeEdit()
        {
            return View();
        }

        #endregion

        #region Order

        public ActionResult Order()
        {
            return View();
        }

        public ActionResult OrderEdit()
        {
            return View();
        }

        public ActionResult CreateOrderRecord()
        {
            return View();
        }

        #endregion

        #region Coupon

        public ActionResult Coupon()
        {
            return View();
        }

        public ActionResult CouponEdit()
        {
            return View();
        }

        public ActionResult DistributeCoupon()
        {
            return View();
        }


        #endregion
    }
}