/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*

*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sheng.Web.Infrastructure
{
    public static class SessionContainer
    {
       // private static Dictionary<Guid, UserContext> _userContextList = new Dictionary<Guid, UserContext>();

        public static void SetUserContext(HttpContextBase httpContext, UserContext userContext)
        {
            httpContext.Session["UserContext"] = userContext;
        }

        public static UserContext GetUserContext(HttpContextBase httpContext)
        {
            return httpContext.Session["UserContext"] as UserContext;
        }

        public static UserContext GetUserContext(HttpContext httpContext)
        {
            return httpContext.Session["UserContext"] as UserContext;
        }

        public static void ClearUserContext(HttpContextBase httpContext)
        {
            httpContext.Session["UserContext"] = null;
        }


        //public static void SetUserContext(Guid userId, UserContext userContext)
        //{
        //    if (_userContextList.ContainsKey(userId))
        //    {
        //        _userContextList[userId] = userContext;
        //    }
        //    else
        //    {
        //        _userContextList.Add(userId, userContext);
        //    }
        //}

        //public static UserContext GetUserContext(Guid userId)
        //{
        //    if (_userContextList.ContainsKey(userId))
        //    {
        //        return _userContextList[userId];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static void ClearUserContext(Guid userId)
        //{
        //    if (_userContextList.ContainsKey(userId))
        //    {
        //        _userContextList.Remove(userId);
        //    }
        //}
    }
}