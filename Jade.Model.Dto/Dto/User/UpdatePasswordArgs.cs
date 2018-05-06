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

namespace Jade.Model.Dto
{
    public class UpdatePasswordArgs
    {
        public int UserId
        {
            get;set;
        }

        public string OldPassword
        {
            get;set;
        }

        public string NewPassword
        {
            get;set;
        }
    }
}
