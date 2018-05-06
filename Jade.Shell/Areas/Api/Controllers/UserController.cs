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
    public class UserController : ApiBaseController
    {
        private static readonly UserManager _userManager = UserManager.Instance;

        #region User

        public ActionResult GetUser(int id)
        {
            User user = _userManager.GetUser(id);
            if (user == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            UserDto userDto = Mapper.Map<UserDto>(user);
            return DataResult(userDto);
        }

        public ActionResult CreateUser()
        {
            UserDto args = RequestArgs<UserDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            User user = Mapper.Map<User>(args);

            NormalResult result = _userManager.CreateUser(user);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateUser()
        {
            UserDto args = RequestArgs<UserDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            User user = Mapper.Map<User>(args);

            NormalResult result = _userManager.UpdateUser(user);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveUser(int id)
        {
            _userManager.RemoveUser(id);
            return SuccessfulResult();
        }

        public ActionResult GetUserList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<User> userList = _userManager.GetUserList(args);

            GetListDataResult<UserDto> result = new GetListDataResult<UserDto>();
            result.PagingInfo = userList.PagingInfo;
            result.Data = Mapper.Map<List<UserDto>>(userList.Data);

            return DataResult(result);
        }

        public ActionResult UpdatePassword()
        {
            UpdatePasswordArgs args = RequestArgs<UpdatePasswordArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            args.UserId = this.UserContext.UserId;

            NormalResult result = _userManager.UpdatePassword(args);
            return ApiResult(result.Successful, result.Message);
        }

        #endregion
    }
}