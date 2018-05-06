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
using System.Web.Mvc;

namespace Sheng.Web.Infrastructure
{
    /// <summary>
    /// 不验证用户或会员是否登录
    /// </summary>
    public class AllowedAnonymous : ActionFilterAttribute
    {

    }
}