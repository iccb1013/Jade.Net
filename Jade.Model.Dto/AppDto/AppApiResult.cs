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
using System.Text;
using System.Threading.Tasks;

namespace Jade.Model.Dto.AppDto
{
    public class AppApiResult
    {
        /// <summary>
        /// success
        /// </summary>
        public string result
        {
            get;set;
        }

        public string errorcode
        {
            get;set;
        }

        public string message
        {
            get;set;
        }

        public string token
        {
            get;set;
        }

        public object data
        {
            get;set;
        }
    }
}
