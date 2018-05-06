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
using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto;
using Sheng.Web.Infrastructure;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class RebateController : AppApiBaseController
    {
        OrderManager _orderManager = OrderManager.Instance;
        // GET: AppApi/Rebate
        public ActionResult QueryPoint()
        {
            AppGetOrderRebateListArgs args = RequestArgs<AppGetOrderRebateListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            args.OrderBy = " rebate_time DESC";
            args.distributeId = this.UserContext.UserId;
            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);

            GetListDataResult<Order_Rebate_New> rebateList = _orderManager.GetOrderRebateList(contractArgs);

            AppGetListDataResult<OrderRebateAppDto> result = new AppGetListDataResult<OrderRebateAppDto>();

            result.pageNo = rebateList.PagingInfo.CurrentPage;
            result.pageCount = rebateList.PagingInfo.PageSize;
            result.totalPage = rebateList.PagingInfo.TotalPage;
            result.totalRecord = rebateList.PagingInfo.TotalCount;

            result.objectList = Mapper.Map<List<Order_Rebate_New>, List<OrderRebateAppDto>>(rebateList.Data);

            return DataResult(result);
        }
    }
}