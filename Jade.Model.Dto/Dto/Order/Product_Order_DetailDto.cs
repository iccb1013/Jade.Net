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
    public class Product_Order_DetailDto
    {
        public int id { get; set; }
        public string order_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string thumb_pic { get; set; }
        public int buy_num { get; set; }
        public string summary { get; set; }
        public string desc_url { get; set; }
        public string cert_sign { get; set; }
        public decimal current_price { get; set; }
        public decimal cost_price { get; set; }
        public decimal real_price { get; set; }
        public decimal first_distribution_price { get; set; }
        public decimal second_distribution_price { get; set; }
        public decimal member_price { get; set; }
        public int is_receiving { get; set; }
        public int product_id { get; set; }
        public Nullable<int> coupon_id { get; set; }
        public string coupon_name { get; set; }
        public decimal coupon_sale_price { get; set; }


        /////////////////////


    }
}
