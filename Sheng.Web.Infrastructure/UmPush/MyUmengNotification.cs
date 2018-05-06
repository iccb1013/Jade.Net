/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*

*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public class MyUmengBaseNotification
    {
        public string production_mode { get; set; } 
        public string timestamp { get; set; }
        public string appkey { get; set; }
        public string type { get; set; }
        public string alias_type { get; set; }
        public string file_id { get; set; }
        public object payload { get; set; } 
    }
    public class MyUmengIOSNotification: MyUmengBaseNotification
    { 
        public string description { get; set; }
    }
}
