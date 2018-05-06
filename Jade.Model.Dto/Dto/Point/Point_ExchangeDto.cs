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
    public class Point_ExchangeDto
    {
        public int id { get; set; }

        public int member_id { get; set; }

        public int amount { get; set; }

        public int status { get; set; }

        public System.DateTime exchange_time { get; set; }



        /////////////////

        public string member_name { get; set; }

        public string member_phone_num { get; set; }

        public string status_text
        {
            get
            {
                switch (status)
                {
                    case 1:
                        return "申请中";
                    case 2:
                        return "审核通过";
                    case 3:
                        return "已付款";
                    case 4:
                        return "已拒绝";
                    default:
                        return "未知：" + status;
                }
            }
        }

    }
}
