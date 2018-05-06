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
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto;
using Sheng.Web.Infrastructure;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class OrderController : AppApiBaseController
    {
        private static readonly OrderManager _orderManager     = OrderManager.Instance;
        private static readonly ProductManager productManager = ProductManager.Instance;
        // GET: AppApi/Order
        public ActionResult QueryPage()
        {
            AppGetProductOrderMasterListArgs args = RequestArgs<AppGetProductOrderMasterListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            args.OrderBy = "order_date_time DESC";
            args.PersonIdNo = UserContext.UserId;

            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);
            
            GetListDataResult<Product_Order_Master> productList = _orderManager.GetOrderList(contractArgs);

            AppGetListDataResult<ProductOrderMasterAppDto> result = new AppGetListDataResult<ProductOrderMasterAppDto>();

            result.pageNo = productList.PagingInfo.CurrentPage;
            result.pageCount = productList.PagingInfo.PageSize;
            result.totalPage = productList.PagingInfo.TotalPage;
            result.totalRecord = productList.PagingInfo.TotalCount;

            result.objectList = Mapper.Map<List<Product_Order_Master>, List<ProductOrderMasterAppDto>>(productList.Data); 

            return DataResult(result);
        }

        public ActionResult Save()
        {
            ProductOrderMasterAppDto order = RequestArgs<ProductOrderMasterAppDto>();
            if (order == null)
            {
                return FailedResult("参数无效。");
            }
            order.memberId = UserContext.UserId;

            NormalResult result = _orderManager.CreateOrder(Mapper.Map<ProductOrderMasterAppDto, Product_Order_Master>(order));

            return ApiResult(result.Successful, result.Successful?"下单成功。":result.Message);
        }

    }
}