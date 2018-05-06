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
    public class PushMessageController : ApiBaseController
    {
        private static readonly PushMessageManager _pushMessageManager = PushMessageManager.Instance;
        private static readonly MemberManager _memberManager = MemberManager.Instance;

        #region PushMessage

        public ActionResult GetPushMessage(int id)
        {
            Push_Message pushMessage = _pushMessageManager.GetPushMessage(id);
            if (pushMessage == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Push_MessageDto pushMessageDto = Mapper.Map<Push_MessageDto>(pushMessage);
            return DataResult(pushMessageDto);
        }

        public ActionResult CreatePushMessage()
        {
            Push_MessageDto args = RequestArgs<Push_MessageDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Push_Message pushMessage = Mapper.Map<Push_Message>(args);

            NormalResult result = _pushMessageManager.CreatePushMessage(pushMessage);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdatePushMessage()
        {
            Push_MessageDto args = RequestArgs<Push_MessageDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Push_Message pushMessage = Mapper.Map<Push_Message>(args);

            NormalResult result = _pushMessageManager.UpdatePushMessage(pushMessage);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemovePushMessage(int id)
        {
            _pushMessageManager.RemovePushMessage(id);
            return SuccessfulResult();
        }

        public ActionResult GetPushMessageList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Push_Message> pushMessageList = _pushMessageManager.GetPushMessageList(args);

            GetListDataResult<Push_MessageInListDto> result = new GetListDataResult<Push_MessageInListDto>();
            result.PagingInfo = pushMessageList.PagingInfo;
            result.Data = Mapper.Map<List<Push_Message>, List<Push_MessageInListDto>>(pushMessageList.Data);

            return DataResult(result);
        }

        /// <summary>
        /// 推送指定 id 的消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Push(int id)
        {
            MyPushService pushService = new MyPushService();


            //推消息
            bool androidSendFlag = false;
            bool iosSendFlag = false;

            Push_Message message = _pushMessageManager.GetPushMessage(id);

            List<Member> memberList = _memberManager.GetAllMemberList();
            StringBuilder memberPhoneNums = new StringBuilder();
           
            foreach (Member jadeMember in memberList)
            {
                memberPhoneNums.Append(jadeMember.phone_num + "\n");
            }

            //memberPhoneNums.Append("15251857421" + "\n");
            if (message.type == 0)
            {
                androidSendFlag = pushService.SendAndroidCustomizedcastFile(message.title, message.description, memberPhoneNums.ToString());
                iosSendFlag = pushService.SendIOSCustomizedcast(message.title, message.description, memberPhoneNums.ToString(), "", "http://app.zsyyd.com/Member/PushMessageMobilePage?id=" + message.id);
            }
            else
            {
                androidSendFlag = pushService.SendAndroidCustomizedcastFile(message.title, message.description, memberPhoneNums.ToString(), "com.android.zhangsy.H5urlActivity", "http://app.zsyyd.com/Member/PushMessageMobilePage?id="+message.id);
                iosSendFlag =pushService.SendIOSCustomizedcast(message.title, message.description, memberPhoneNums.ToString(), "h5url", HttpContext.Request.Url.AbsolutePath + "?id=" + message.id);
            };

            //写历史记录
            _pushMessageManager.CreatePushHistory(id, this.UserContext.UserId, androidSendFlag, iosSendFlag);

            if (androidSendFlag && iosSendFlag)
            {
                return SuccessfulResult();
            }
            else
            {
                return FailedResult("推送失败。");
            }
        }

        #endregion

        #region PushMessageHistory

        public ActionResult GetPushHistory(int id)
        {
            Push_History push_History = _pushMessageManager.GetPushHistory(id);
            if (push_History == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Push_HistoryDto push_HistoryDto = Mapper.Map<Push_HistoryDto>(push_History);
            return DataResult(push_HistoryDto);
        }

        public ActionResult GetPushHistoryList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Push_History> push_HistoryList = _pushMessageManager.GetPushHistoryList(args);

            GetListDataResult<Push_HistoryInListDto> result = new GetListDataResult<Push_HistoryInListDto>();
            result.PagingInfo = push_HistoryList.PagingInfo;
            result.Data = Mapper.Map<List<Push_History>, List<Push_HistoryInListDto>>(push_HistoryList.Data);

            return DataResult(result);
        }

        #endregion
    }
}