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


using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jade.Shell
{
    public static class AuthorizationHelper
    {
        public static bool Verify(string key, HttpContext httpContext)
        {
            ShellUserContext userContext = (ShellUserContext)SessionContainer.GetUserContext(httpContext);

            return userContext.AuthorizationKeyVerify(key);
        }
    }
}