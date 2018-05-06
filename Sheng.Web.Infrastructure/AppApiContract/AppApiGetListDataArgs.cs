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
    public class AppGetListDataArgs
    {


        public const int DEFAULT_PAGE_COUNT = 10;
        public const int DEFAULT_PAGE_NO = 1;

        /**
         * 当前页
         */
        public int PageNo { get; set; }

        /**
         * 每页数据量
         */
        public int PageCount { get; set; }


        /**
         * 起始时间
         */
        public DateTime StartTime { get; set; }

        /**
         * 结束时间
         */
        public DateTime EndTime { get; set; }

        /**
         * 查询关键字
         */
        [TransferToDictionaryKeyAttribute("Keyword")]
        public string Search { get; set; }

        /**
         * 是否分页
         */
        public bool IsPagination { get; set; }

        public string Token { get; set; }

        public string OrderBy { get; set; }
    }
}
