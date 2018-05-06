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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jade.Model.Dto.AppDto
{
    public class MicroClassGroupAppDto
    {
        public int id { get; set; }
        public int status { get; set; }
        public string name { get; set; } 
        public string imgUrl { get; set; } 
        public string createTime{ get; set; } 
        public string updateTime { get; set; }
        public bool AdminSys { get; set; } = true;
        public object objList { get; set; } = null;
    }
}
