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
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    /// <summary>
    /// 存储在 redis 中作为用户上下文
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class UserContext //<TUser>
    {
        /// <summary>
        /// 登录成功后分配一个 Token
        /// </summary>
        public string Token
        {
            get;set;
        }

        /// <summary>
        /// 用户对象
        /// </summary>
        public int UserId
        {
            get;set;
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get;set;
        }
    }
}
