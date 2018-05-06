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
    public class ExchangeController : AppApiBaseController
    {
        private static readonly PointManager _pointManager = PointManager.Instance;
        private static readonly MemberManager _memberManager = MemberManager.Instance; 

        // GET: AppApi/Exchange
        //会员申请提点  
        public ActionResult ConsumePoint()
        { 
            PointExchangeAppDto args = RequestArgs<PointExchangeAppDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            if (UserContext.Member.status.Equals("2") == false)
            {
                return FailedResult("账号未通过审核。");
            }

            //判断是否有未结束的提点
            bool isExistNotFinishFlag = _pointManager.CheckExistNotFinishPointExchange(UserContext.UserId);
            if (isExistNotFinishFlag)
            {
                return FailedResult("存在未结束提点。");
            }


            //用户积分需要实时查询
            Member member = _memberManager.GetMember(UserContext.Member.id);
             
            Point_Exchange pointExchange = new Point_Exchange();
            pointExchange.member_id = member.id;
            pointExchange.status = 1;
            pointExchange.amount = member.total_point.HasValue?member.total_point.Value:0;
            pointExchange.exchange_time = DateTime.Now;

            _pointManager.CreatePointExchange(pointExchange);
             

            NormalResult result = _pointManager.CreatePointExchange(pointExchange); 

            return ApiResult(result.Successful, "申请提点成功。", Mapper.Map<PointExchangeAppDto>(pointExchange)); 
        }
    }
} 

