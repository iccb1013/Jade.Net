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
    public class ProductAttributeAppDto
    {
        public int id { get; set; }
        public Nullable<int> parentId { get; set; }
        public Nullable<int> categoryId { get; set; }
        public int serialNo { get; set; }
        public string attributeName { get; set; }
        public int inputType { get; set; } 
        public List<ProductAttributeAppDto> childAttributeList { get; set; } 
        public ProductCategoryInAttributeAppDto productCategory { get; set; } 
    }
}
