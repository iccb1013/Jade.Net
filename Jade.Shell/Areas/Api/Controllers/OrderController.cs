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
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class OrderController : ApiBaseController
    {
        private static readonly OrderManager _orderManager = OrderManager.Instance;

        public ActionResult GetOrder(string id)
        {
            Product_Order_Master order = _orderManager.GetOrder(id);
            if (order == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Product_Order_MasterDto orderDto = Mapper.Map<Product_Order_MasterDto>(order);
            return DataResult(orderDto);
        }

        public ActionResult RemoveOrder(string id)
        {
            _orderManager.RemoveOrder(id);
            return SuccessfulResult();
        }

        public ActionResult GetOrderList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Product_Order_Master> orderList = _orderManager.GetOrderList(args);

            GetListDataResult<Product_Order_MasterInListDto> result = new GetListDataResult<Product_Order_MasterInListDto>();
            result.PagingInfo = orderList.PagingInfo;
            result.Data = Mapper.Map<List<Product_Order_Master>, List<Product_Order_MasterInListDto>>(orderList.Data);

            return DataResult(result);
        }

        public ActionResult CreateOrderRecord()
        {
            CreateOrderRecordArgs args = RequestArgs<CreateOrderRecordArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            args.modify_user_id = this.UserContext.User.id;

            NormalResult result = _orderManager.CreateOrderRecord(args);
            return ApiResult(result.Successful, result.Message);
        }
    }
}