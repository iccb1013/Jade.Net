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
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class BrowseController : AppApiBaseController
    {
        private static readonly BrowseHistoryManager _browseHistoryManager = BrowseHistoryManager.Instance;
        // GET: AppApi/Browse
        public ActionResult Save()
        {
            AppProductBrowseHistoryArgs args = RequestArgs<AppProductBrowseHistoryArgs>();

            Browse_History browseHistory = new Browse_History();
            browseHistory.member_id = this.UserContext.Member.id;
            browseHistory.product_id = args.ProductId;
            browseHistory.last_browse_time = DateTime.Now;
            browseHistory.browse_count = 1;

            _browseHistoryManager.CreateBrowseHistory(browseHistory);

            return ApiResult(true, "浏览商品记录保存成功！");
        }
    }
}