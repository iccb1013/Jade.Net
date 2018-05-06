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
    public class MemberLoginArgs
    {
        public string mobilephone
        {
            get; set;
        }

        /// <summary>
        /// MD5加密后传输
        /// </summary>
        public string password
        {
            get; set;
        }
    }
}
