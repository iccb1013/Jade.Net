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
    /// 默认false
    /// </summary>
    public class ApiResult
    {
        public bool Successful
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 失败时返回一个原因代码
        /// 0成功
        /// 7001：Session过期，主要用于API请求
        /// </summary>
        public int Hint
        {
            get;
            set;
        }

        public object Data
        {
            get;set;
        }

        public ApiResult()
        {

        }

        public ApiResult(bool success)
        {
            Successful = success;
        }

        public ApiResult(string message)
        {
            Successful = false;
            Message = message;
        }
    }

    //public class ApiResult<T> : ApiResult
    //{
    //    public T Data
    //    {
    //        get;
    //        set;
    //    }
    //}
}
