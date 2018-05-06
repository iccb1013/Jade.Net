/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*
*    © Copyright 2017
*
********************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Kernal
{
    /// <summary>
    /// 通过键值对导出数据
    /// 用于源数据对象的列存在不确定性的情况，比如按产品的销售渠道 分别 统计产品的销量
    /// </summary>
    public class ExcelExportByDictionaryArgs
    {
        public List<Dictionary<string, string>> DataList
        {
            get;set;
        }

        /// <summary>
        /// 是否导出序号
        /// </summary>
        public bool RowNumber
        {
            get;set;
        }

        public List<ExcelExportColumnDefine> ColumnDefine
        {
            get;set;
        }

        public string FilePath
        {
            get;set;
        }

        public ExcelExportByDictionaryArgs()
        {
            ColumnDefine = new List<ExcelExportColumnDefine>();
        }
    }

   
}
