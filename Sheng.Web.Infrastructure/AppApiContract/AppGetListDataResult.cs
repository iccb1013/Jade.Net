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
    public class AppGetListDataResult<T>
    {
        public const string SUCCESS = "success";

        public const string ERROR = "error";
        public int totalPage { get; set; }

        public int pageNo { get; set; }

        public int totalRecord { get; set; }

        public int pageCount { get; set; }

        public List<T> objectList { get; set; }
    }
}
