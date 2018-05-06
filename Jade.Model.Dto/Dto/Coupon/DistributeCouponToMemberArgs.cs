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
    public class DistributeCouponToMemberArgs
    {
        public int CouponId
        {
            get;set;
        }

        public int MemberId
        {
            get;set;
        }

        public int UserId
        {
            get;set;
        }
    }
}
