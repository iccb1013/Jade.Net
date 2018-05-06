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
    public class CouponInfoAppDto
    {
        public int id { get; set; }
        public decimal discount { get; set; }
        public int is_lose_valid { get; set; }
        public System.DateTime create_time { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }

    }
}
