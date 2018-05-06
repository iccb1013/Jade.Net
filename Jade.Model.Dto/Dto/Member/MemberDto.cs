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
    public class MemberDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone_num { get; set; }
        public string wechat_no { get; set; }
        public string password { get; set; }
        public int type { get; set; }
        public Nullable<decimal> total_amount { get; set; }
        public Nullable<int> superior_agent_id { get; set; }
        public string pic_path { get; set; }
        public string card_holder { get; set; }
        public string card_no { get; set; }
        public string deposit_bank { get; set; }
        public string deposit_branch_bank { get; set; }
        public System.DateTime reg_time { get; set; }
        public Nullable<System.DateTime> settle_time { get; set; }
        public Nullable<int> total_point { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public int secret_status { get; set; }
        public int status { get; set; }
        public Nullable<System.DateTime> month_rebate_time { get; set; }
        public int app_browse_count { get; set; }
        public int product_browse_count { get; set; }



        /////////////////////////////

        public string superior_agent_name { get; set; }

        public string superior_agent_phone_num { get; set; }


        public string status_text
        {
            get
            {
                switch (status)
                {
                    case 1:
                        return "新建待审核";
                    case 2:
                        return "审核通过";
                    case 3:
                        return "审核不通过";
                    case 5:
                        return "注销";
                    default:
                        return "未知:" + status;
                }
            }
        }

        public string type_text
        {
            get
            {
                switch (type)
                {
                    case 1:
                        return "一级代理";
                    case 2:
                        return "二级代理";
                    case 3:
                        return "会员";
                    default:
                        return "未知:" + status;
                }
            }
        }

        

    }
}
