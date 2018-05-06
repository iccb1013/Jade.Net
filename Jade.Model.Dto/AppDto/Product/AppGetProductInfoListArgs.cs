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
using Sheng.Web.Infrastructure;

namespace Jade.Model.Dto.AppDto
{
    public class AppGetProductInfoListArgs: AppGetListDataArgs
    {
        public int orderByType { get; set; } 
        public int? status { get; set; }
        public decimal priceFrom { get; set; }
        public decimal priceTo { get; set; }
        public int memberType { get; set; }

        public int categoryId { get; set; }
        public int groupId { get; set; }
        public int[] attributeIdList { get; set; }
    }
}
