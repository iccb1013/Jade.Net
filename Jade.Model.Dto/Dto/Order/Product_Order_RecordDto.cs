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
    public class Product_Order_RecordDto
    {
        public int id { get; set; }
        public string order_id { get; set; }
        public string modify_comment { get; set; }
        public int modify_user_id { get; set; }
        public System.DateTime modify_date_time { get; set; }


        ///////////////////

        public string modify_user_name { get; set; }
    }
}
