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
    public class Product_InfoDto
    {
        public int id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string summary { get; set; }
        public string thumb_pic { get; set; }
        public string desc_url { get; set; }
        public decimal cost_price { get; set; }
        public decimal real_price { get; set; }
        public decimal first_distribution_price { get; set; }
        public decimal second_distribution_price { get; set; }
        public decimal member_price { get; set; }
        public int sales_num { get; set; }
        public int stock { get; set; }
        public int status { get; set; }
        public int serial_no { get; set; }
        public System.DateTime create_date_time { get; set; }
        public Nullable<System.DateTime> modify_date_time { get; set; }
        public Nullable<int> modify_user_id { get; set; }
        public int browse_count { get; set; }
        public int star_white { get; set; }
        public int star_granularity { get; set; }
        public int star_defect { get; set; }
        public int star_oily { get; set; }
        //public string pic_url { get; set; }
        public string video_url { get; set; }

        //在 automapper 里自动映射
        public List<string> pic_url { get; set; }


        public int [] catalog_idList { get; set; }

        public string[] catalog_nameList { get; set; }

        public int[] attribute_idList { get; set; }

        public string[] attribute_nameList { get; set; }


        public string statusText
        {
            get
            {
                if (status == 0)
                    return "下架";
                else if (status == 1)
                    return "上架";
                else
                    return "未知";
            }
        }

    }
}
