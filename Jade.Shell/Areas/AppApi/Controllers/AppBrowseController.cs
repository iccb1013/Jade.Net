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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto;
using Sheng.Web.Infrastructure;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class AppBrowseController : AppApiBaseController
    {
        private static readonly AppBrowseManager _appBrowseManager = AppBrowseManager.Instance;
        // GET: AppApi/AppBrowse
        public ActionResult Save()
        {
            AppBrowseRecordedArgs args = RequestArgs<AppBrowseRecordedArgs>();

            App_Browse appBrowse  = new App_Browse();
            appBrowse.member_id = this.UserContext.Member.id;
            appBrowse.client_type = args.ClientType;
            appBrowse.create_time = DateTime.Now;

            _appBrowseManager.CreateBrowse(appBrowse);
            
            return ApiResult(true,"浏览记录保存成功！");
        }
    }
}