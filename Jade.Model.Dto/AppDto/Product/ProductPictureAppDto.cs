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
    public class ProductPictureAppDto
    {
        public string id{ get; set; }
        public string productId{ get; set; }
        public string picUrl{ get; set; }
        public string videoUrl{ get; set; }

        public string serialNo{ get; set; }
    }
}
