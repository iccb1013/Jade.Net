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
    public class MyUmResult
    {
        public string ret { get; set; }
        public MyUmResultData data { get; set; }
    }
    public class MyUmResultData
    {
        public string file_id { get; set; }
        public string error_code { get; set; }
        public string error_msg { get; set; }
    }
}
