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
    public class PointController : ApiBaseController
    {
        private static readonly PointManager _pointManager = PointManager.Instance;

        public ActionResult GetPointExchange(int id)
        {
            Point_Exchange pointExchange = _pointManager.GetPointExchange(id);
            if (pointExchange == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Point_ExchangeDto memberDto = Mapper.Map<Point_ExchangeDto>(pointExchange);
            return DataResult(memberDto);
        }

        /// <summary>
        /// 后台管理员添加
        /// </summary>
        /// <returns></returns>
        public ActionResult CreatePointExchange()
        {
            Point_ExchangeDto args = RequestArgs<Point_ExchangeDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Point_Exchange pointExchange = Mapper.Map<Point_Exchange>(args);

            NormalResult result = _pointManager.CreatePointExchange(pointExchange);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdatePointExchange()
        {
            Point_ExchangeDto args = RequestArgs<Point_ExchangeDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Point_Exchange pointExchange = Mapper.Map<Point_Exchange>(args);

            NormalResult result = _pointManager.UpdatePointExchange(pointExchange);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdatePointExchangeStatus(int id)
        {
            UpdatePointExchangeStatusArgs args = RequestArgs<UpdatePointExchangeStatusArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            _pointManager.UpdatePointExchangeStatus(args);
            return SuccessfulResult();
        }

        public ActionResult GetPointExchangeList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Point_Exchange> memberList = _pointManager.GetPointExchangeList(args);

            GetListDataResult<Point_ExchangeDto> result = new GetListDataResult<Point_ExchangeDto>();
            result.PagingInfo = memberList.PagingInfo;
            result.Data = Mapper.Map<List<Point_Exchange>, List<Point_ExchangeDto>>(memberList.Data);

            return DataResult(result);
        }

    }
}