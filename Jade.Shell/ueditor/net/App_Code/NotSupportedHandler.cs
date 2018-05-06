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

/// <summary>
/// NotSupportedHandler 的摘要说明
/// </summary>
public class NotSupportedHandler : Handler
{
    public NotSupportedHandler(HttpContext context)
        : base(context)
    {
    }

    public override void Process()
    {
        WriteJson(new
        {
            state = "action 参数为空或者 action 不被支持。"
        });
    }
}