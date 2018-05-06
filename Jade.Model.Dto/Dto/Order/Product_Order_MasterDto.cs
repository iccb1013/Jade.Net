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
    public class Product_Order_MasterDto
    {
        public string id { get; set; }
        public int member_id { get; set; }
        public string consignee { get; set; }
        public string consignee_address { get; set; }
        public string consignee_phone { get; set; }
        public decimal real_total_price { get; set; }
        public decimal order_price { get; set; }
        public int product_num { get; set; }
        public int current_status { get; set; }
        public string transport_no { get; set; }
        public string transport_company { get; set; }
        public System.DateTime order_date_time { get; set; }
        public Nullable<System.DateTime> order_complete_time { get; set; }
        public Nullable<System.DateTime> payment_date_time { get; set; }
        public Nullable<System.DateTime> rebate_date_time { get; set; }
        public Nullable<System.DateTime> refund_date_time { get; set; }
        public int rebate_flag { get; set; }
        public int del_flag { get; set; }
        public int total_Rebate_Amount { get; set; }
        public int superior_Rebate_Amount { get; set; }
        public int superior_Parent_Rebate_Amount { get; set; }
        public decimal receiving_real_price { get; set; }


        ////////////////////////////

        public string member_name { get; set; }

        public string member_phone_num { get; set; }

        public List<Product_Order_DetailDto> Product_Order_Detail { get; set; }

        public List<Product_Order_RecordDto> Product_Order_Record { get; set; }


        public string current_status_text
        {
            get
            {
                switch (current_status)
                {
                    case 1:
                        return "待付费";
                    case 2:
                        return "待发货";
                    case 3:
                        return "已发货";
                    case 4:
                        return "已收货";
                    case 5:
                        return "退货订单";
                    case 6:  //作废
                        return "退货订单";
                    case 9:
                        return "已完成";
                    default:
                        return "未知:" + current_status;
                }
            }
        }



    }
}
