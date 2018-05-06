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
    public class CouponController : AppApiBaseController
    {
        private static readonly CouponManager _couponManager = CouponManager.Instance;

        //获取我的优惠券
        public ActionResult queryPage()
        {
            AppGetMemberCouponAccountListArgs args = RequestArgs<AppGetMemberCouponAccountListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            
            args.MemberId = this.UserContext.Member.id;
            args.IsUse = 0;
            args.OrderBy = "create_time DESC";
            if (args.OrderByType == 1)
            {
                args.OrderBy = "create_time ASC";
            }
            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);

            //查询符合条件的优惠券集合
            GetListDataResult<Member_Coupon_Account> memberCouponList = _couponManager.GetMemberCouponList(contractArgs);

            AppGetListDataResult<MemberCouponAccountAppDto> result = new AppGetListDataResult<MemberCouponAccountAppDto>();

            result.pageNo = memberCouponList.PagingInfo.CurrentPage;
            result.pageCount = memberCouponList.PagingInfo.PageSize;
            result.totalPage = memberCouponList.PagingInfo.TotalPage;
            result.totalRecord = memberCouponList.PagingInfo.TotalCount;
            result.objectList = Mapper.Map<List<Member_Coupon_Account>, List<MemberCouponAccountAppDto>>(memberCouponList.Data);

            return DataResult(result);
        }

    }
}