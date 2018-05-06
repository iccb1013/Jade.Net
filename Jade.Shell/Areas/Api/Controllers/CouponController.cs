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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class CouponController : ApiBaseController
    {
        private static readonly CouponManager _couponManager = CouponManager.Instance;
        private static readonly MemberManager _memberManager = MemberManager.Instance;


        public ActionResult GetCoupon(int id)
        {
            Coupon_Info coupon = _couponManager.GetCoupon(id);
            if (coupon == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Coupon_InfoDto couponDto = Mapper.Map<Coupon_InfoDto>(coupon);
            return DataResult(couponDto);
        }

        public ActionResult CreateCoupon()
        {
            Coupon_InfoDto args = RequestArgs<Coupon_InfoDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Coupon_Info coupon = Mapper.Map<Coupon_Info>(args);

            NormalResult result = _couponManager.CreateCoupon(coupon);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateCoupon()
        {
            Coupon_InfoDto args = RequestArgs<Coupon_InfoDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Coupon_Info coupon = Mapper.Map<Coupon_Info>(args);

            NormalResult result = _couponManager.UpdateCoupon(coupon);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveCoupon(int id)
        {
            _couponManager.RemoveCoupon(id);
            return SuccessfulResult();
        }

        public ActionResult GetCouponList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Coupon_Info> couponList = _couponManager.GetCouponList(args);

            GetListDataResult<Coupon_InfoDto> result = new GetListDataResult<Coupon_InfoDto>();
            result.PagingInfo = couponList.PagingInfo;
            result.Data = Mapper.Map<List<Coupon_Info>, List<Coupon_InfoDto>>(couponList.Data);

            return DataResult(result);
        }

        public ActionResult DistributeCouponToAllMember()
        {
            DistributeCouponToAllMemberArgs args = RequestArgs<DistributeCouponToAllMemberArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            List<Member> memberList = _memberManager.GetAllMemberList();
            StringBuilder memberPhoneNums = new StringBuilder();
            foreach (Member jadeMember in memberList)
            {
                memberPhoneNums.Append(jadeMember.phone_num + "\n");
            } 

            MyPushService pushService = new MyPushService();

            string strTile = "优惠券到账通知！";
            string strMsg = "【张寿宴玉雕】送您一张优惠券：" + _couponManager.GetCoupon(args.CouponId).name + "，这里·才是和田玉的源头。";
             
            pushService.SendAndroidCustomizedcastFile(strTile, strMsg, memberPhoneNums.ToString(), "com.android.zhangsy.MyCouponActivity", "");
            pushService.SendIOSCustomizedcast(strMsg, strMsg, memberPhoneNums.ToString(), "mycoupon", "");

            NormalResult result = _couponManager.DistributeCouponToAllMember(args);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult DistributeCouponToMember()
        {
            DistributeCouponToMemberArgs args = RequestArgs<DistributeCouponToMemberArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Member member = _memberManager.GetMember(args.MemberId);
            if (member == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            MyPushService pushService = new MyPushService();

            string strTile = "优惠券到账通知！";
            string strMsg = "【张寿宴玉雕】送您一张优惠券：" + _couponManager.GetCoupon(args.CouponId).name + "，这里·才是和田玉的源头。";

            string phoneNum =member.phone_num+"\n";

            pushService.SendAndroidCustomizedcastFile(strTile, strMsg, phoneNum, "com.android.zhangsy.MyCouponActivity", "");
            pushService.SendIOSCustomizedcast(strMsg, strMsg, phoneNum, "mycoupon", "");

            NormalResult result = _couponManager.DistributeCouponToMember(args);
            return ApiResult(result.Successful, result.Message);
        }
    }
}