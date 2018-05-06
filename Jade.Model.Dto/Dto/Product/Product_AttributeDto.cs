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
    public class Product_AttributeDto
    {
        public int id { get; set; }
        public Nullable<int> parent_id { get; set; }
        public Nullable<int> category_id { get; set; }
        public int serial_no { get; set; }
        public string attribute_name { get; set; }
        public int input_type { get; set; }

    }
}
