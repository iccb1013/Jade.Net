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
    public class CreateOrderRecordArgs
    {
        public string order_id
        {
            get;set;
        }

        public string modify_comment
        {
            get;set;
        }

        public int modify_user_id
        {
            get;set;
        }

        /// <summary>
        /// 新的订单状态
        /// </summary>
        public int newOrder_status
        {
            get;set;
        }

        //如果新的订单状态是已发货的话，填写物流信息，也可不填

        public string transport_no
        {
            get;set;
        }

        public string transport_company
        {
            get;set;
        }

        /// <summary>
        /// 已完成时，确认收货的明细表id
        /// </summary>
        public int [] receiving_detail_id
        {
            get;set;
        }
    }
}
