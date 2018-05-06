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
using Jade.Model.Dto.AppDto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sheng.Kernal;


namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class MemberController : AppApiBaseController
    {
        private static readonly MemberManager _memberManager = MemberManager.Instance; 
        private static readonly CachingService _cachingService = CachingService.Instance;

        private const double VALIDATIONCODE_EXPRESSTIME = 15;//验证码过期时间
        /// <summary>
        /// APP端注册
        /// </summary>
        /// <returns></returns>
        [AllowedAnonymous]
        public ActionResult RegisterNoSuper()
        {
            MemberRegisterArgs args = RequestArgs<MemberRegisterArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Member member = new Member();
            member.phone_num = args.mobilephone;
            member.wechat_no = args.wechatNo;
            member.password = IOHelper.GetMD5HashFromString(args.password);
            member.status = 1;
            member.type = 3;

            NormalResult result = _memberManager.CreateMember(member);

            if (result.Successful)
            {
                //注册使用模版
                Yuntongxun.SendTemplateSMS(args.mobilephone, "197936", new string[] { "18994422279" });

                //新用户注册等待审核
                Yuntongxun.SendTemplateSMS("18994422239", "241364",
                    new string[] { member.phone_num, _memberManager.GetMemberCount(1).ToString() });


                return ApiResult(true, "申请开户成功", Mapper.Map<MemberAppDto>(member));
            }
            else
            {
                return FailedResult(result.Message);
            }
        }

        /// <summary>
        /// 会员开户
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            MemberRegisterArgs args = RequestArgs<MemberRegisterArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Member member = new Member();
            member.name = args.userName;
            member.wechat_no = args.wechatNo;
            member.phone_num = args.mobilephone;
            member.superior_agent_id = this.UserContext.UserId;
            member.status = 1;
            member.type = 3;

            NormalResult result = _memberManager.CreateMember(member);

            if (result.Successful)
            {
                //发短信
                Yuntongxun.SendTemplateSMS(args.mobilephone, "197936", new string[] { "18994422279" });

                //新用户注册等待审核
                Yuntongxun.SendTemplateSMS("18994422239", "241364",
                    new string[] { member.phone_num, _memberManager.GetMemberCount(1).ToString() });


                return ApiResult(true, "申请开户成功");
            }
            else
            {
                return FailedResult(result.Message);
            }
        }
        /// <summary>
        /// 会员登录
        /// </summary>
        /// <returns></returns>
        [AllowedAnonymous]
        public ActionResult Login()
        {
            MemberLoginArgs args = RequestArgs<MemberLoginArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            args.password = IOHelper.GetMD5HashFromString(args.password);
            Member member = _memberManager.GetMemberByMobilePhoneAndPassword(args.mobilephone,args.password);

            if (member == null == false)
            {
                //TODO:初始化 memberDto
                MemberAppDto memberAppDto = Mapper.Map<Member, MemberAppDto>(member);

                memberAppDto.primaryDistributionCount = _memberManager.GetMemberDistributionCount(memberAppDto.id,null); 
                memberAppDto.secondDistributionCount  = _memberManager.GetMemberDistributionCount(null, Convert.ToInt32(memberAppDto.superiorAgentId)); 
                MemberUserContext userContext = new MemberUserContext()
                {
                    LoginTime = DateTime.Now,
                    Token = Guid.NewGuid().ToString(),
                    UserId = member.id,
                    Member = memberAppDto
                };
                NormalResult<MemberUserContext> result = new NormalResult<MemberUserContext>();
                result.Data = userContext;

                _cachingService.Set(userContext.Token, userContext);

                //一般返回结果不需要自己new AppApiResult，看 return ApiResult 这个方法的几个重载
                AppApiResult apiResult = new AppApiResult()
                {
                    result = "success",
                    message = "登录成功",
                    token = result.Data.Token,
                    data = memberAppDto
                };
                return ApiResult(apiResult);
            }
            else
            { 
                return FailedResult("手机号或密码无效。");
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [AllowedAnonymous]
        public ActionResult Logout()
        {
            if (UserContext != null)
            {
                _cachingService.Remove(UserContext.Token);
            }

            return SuccessfulResult();

            //MemberLoginArgs args = RequestArgs<MemberLoginArgs>();
            //if (args == null)
            //{
            //    return FailedResult("参数无效。");
            //}
            ////根据用户名注销用户


            //bool flag = _cachingService.Remove(UserContext.Token);

            //return ApiResult(flag, flag ? "注销成功" : "注销失败");
        }

        [ActionName("/get/directMember")]
        [HttpPost]
        public ActionResult GetDirectMember()
        {
            List<Member> memberList = _memberManager.GetDirectMemberList(this.UserContext.UserId);
            AppApiResult apiResult = new AppApiResult()
            {
                result = "success",
                message = "查询下级列表信息成功!",
                token = this.UserContext.Token,
                data = Mapper.Map<List<Member>, List<MemberAppDto>>(memberList)
             };

            return ApiResult(apiResult); 
        }
        /// <summary>
        /// 根据路由获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get(int id)
        {
            if (id == 0)
            {
                return FailedResult("参数无效");
            }
            MemberAppDto member = Mapper.Map<Member, MemberAppDto> (_memberManager.GetMember(id));
            AppApiResult apiResult = new AppApiResult()
            {
                result = "success",
                message = "查询用户信息成功!",
                token = this.UserContext.Token,
                data = member
            };
            return ApiResult(apiResult);
        }

        /// <summary>
        /// 会员修改
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            MemberAppDto args = RequestArgs<MemberAppDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            Member member = _memberManager.GetMember(UserContext.UserId);
            if (string.IsNullOrEmpty(args.name) ==false)
            {
                member.name = args.name;
            } 
            if (string.IsNullOrEmpty(args.wechatNo) == false)
            {
                member.wechat_no = args.wechatNo;
            } 
            if (string.IsNullOrEmpty(args.cardHolder) == false)
            {
                member.card_holder = args.cardHolder;
            } 
            if (string.IsNullOrEmpty(args.cardNo) == false)
            {
                member.card_no = args.cardNo;
            } 
            if (string.IsNullOrEmpty(args.depositBank) == false)
            {
                member.deposit_bank = args.depositBank;
            } 
            if (string.IsNullOrEmpty(args.depositBranchBank) == false)
            {
                member.deposit_branch_bank = args.depositBranchBank;
            } 

            if (string.IsNullOrEmpty(args.secretStatus) == false)
            {
                member.secret_status = Convert.ToInt32(args.secretStatus);
            }

            if (string.IsNullOrEmpty(args.picPath) == false)
            {
                member.pic_path = args.picPath;
            }
            NormalResult result = _memberManager.UpdateMember(member);

            return ApiResult(result.Successful, result.Message);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePwd()
        {
            MemberChangePwdArgs args = RequestArgs<MemberChangePwdArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            
            Member member = _memberManager.GetMemberByMobilePhoneAndPassword(args.mobilephone, IOHelper.GetMD5HashFromString(args.oldPwd));
            if (member == null)
            {
                return FailedResult("手机号或密码不正确。");
            }
            
            NormalResult result = _memberManager.UpdateMemberPassword(member.id, IOHelper.GetMD5HashFromString(args.newPwd));

            return ApiResult(result.Successful, "修改密码成功。");
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [AllowedAnonymous]
        public ActionResult ResetPwd()
        {

            MemberResetPasswordArgs args = RequestArgs<MemberResetPasswordArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            //校验验证码

            string code = _cachingService.Get(args.mobilephone + "ValidationCode");
            if (string.IsNullOrEmpty(code) == true)
            {
                return FailedResult("验证码已失效。");
            }

            if (code.Equals(args.code) == false)
            {
                return FailedResult("错误的验证码。");
            }

            Member member = _memberManager.GetMemberByMobilePhone(args.mobilephone);
            if (member == null)
            {
                return FailedResult("指定的会员不存在！");
            }
            
            NormalResult result = _memberManager.UpdateMemberPassword(member.id, IOHelper.GetMD5HashFromString(args.password)); 

            return ApiResult(result.Successful, "修改密码成功。");
        }

        /// <summary>
        /// 发送找回密码验证码
        /// </summary>
        /// <returns></returns>
        [AllowedAnonymous]
        public ActionResult SendCode() {

            MemberSendCodeArgs args = RequestArgs<MemberSendCodeArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
            string code = new Random().Next(1000, 9999).ToString();
            
            Yuntongxun.SendTemplateSMS(args.mobilephone, "192218", new string[] { code});

            _cachingService.Set(args.mobilephone + "ValidationCode", code, TimeSpan.FromMinutes(VALIDATIONCODE_EXPRESSTIME));

            return ApiResult(true,"验证码已发送，15分钟内有效。");
        }
    }
}