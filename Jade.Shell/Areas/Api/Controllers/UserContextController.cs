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
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class UserContextController : ApiBaseController
    {
        private static readonly UserContextManager _userContextManager = UserContextManager.Instance;

        public ActionResult Login()
        {
            ParametersContainer args = RequestArgs<ParametersContainer>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            NormalResult<UserContext> result = _userContextManager.Login(args.GetValue("Account"), args.GetValue("Password"));
            if (result.Successful)
            {
                ShellUserContext userContext = Mapper.Map<ShellUserContext>(result.Data);
                userContext.User = UserManager.Instance.GetUser(userContext.UserId);
                SessionContainer.SetUserContext(HttpContext, userContext);

                return DataResult(result.Data);
            }
            else
            {
                return FailedResult(result.Message);
            }

        }

        public ActionResult Logout()
        {
            string token = Request.QueryString["token"];
            _userContextManager.Logout(token);
            return SuccessfulResult();
        }

        public ActionResult GetUserContext()
        {
            string token = Request.QueryString["token"];
            if (String.IsNullOrEmpty(token))
                return FailedResult("未指定 token");

            UserContext userContext = _userContextManager.GetUserContext(token);
            if (userContext != null)
                return DataResult(userContext);
            else
                return FailedResult(String.Empty);
        }
    }
}