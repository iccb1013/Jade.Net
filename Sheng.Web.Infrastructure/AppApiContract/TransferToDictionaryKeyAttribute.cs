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
    public class TransferToDictionaryKeyAttribute: Attribute
    {
        public string Key
        {
            get;set;
        }

        public TransferToDictionaryKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
