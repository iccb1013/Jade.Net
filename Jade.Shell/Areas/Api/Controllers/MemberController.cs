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
    public class MemberController : ApiBaseController
    {
        private static readonly MemberManager _memberManager = MemberManager.Instance;

        public ActionResult GetMember(int id)
        {
            Member member = _memberManager.GetMember(id);
            if (member == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            MemberDto memberDto = Mapper.Map<MemberDto>(member);
            return DataResult(memberDto);
        }

        /// <summary>
        /// 后台管理员添加
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMember()
        {
            MemberDto args = RequestArgs<MemberDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Member member = Mapper.Map<Member>(args);

            NormalResult result = _memberManager.CreateMember(member);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateMember()
        {
            MemberDto args = RequestArgs<MemberDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Member member = Mapper.Map<Member>(args);

            NormalResult result = _memberManager.UpdateMember(member);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveMember(int id)
        {
            _memberManager.RemoveMember(id);
            return SuccessfulResult();
        }

        public ActionResult GetMemberList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Member> memberList = _memberManager.GetMemberList(args);

            GetListDataResult<MemberDto> result = new GetListDataResult<MemberDto>();
            result.PagingInfo = memberList.PagingInfo;
            result.Data = Mapper.Map<List<Member>, List<MemberDto>>(memberList.Data);

            return DataResult(result);
        }

        public ActionResult ChangeStatus()
        {
            ChangeMemberStatusArgs args = RequestArgs<ChangeMemberStatusArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            NormalResult result = _memberManager.ChangeStatus(args);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult ResetPassword(int id)
        {
            _memberManager.ResetPassword(id);
            return SuccessfulResult();
        }

    }
}