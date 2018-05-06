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
    public class GetListDataArgs
    {
        public PagingInfo PagingInfo
        {
            get; set;
        }

        public string OrderBy
        {
            get;set;
        }

        public ParametersContainer Parameters
        {
            get; set;
        }

        public GetListDataArgs()
        {
            PagingInfo = new PagingInfo();
            Parameters = new ParametersContainer();
        }
    }

    public class GetListDataArgs<T>
    {
        public PagingInfo PagingInfo
        {
            get; set;
        }

        public string OrderBy
        {
            get; set;
        }
        
        public T Parameters
        {
            get; set;
        }

        public GetListDataArgs()
        {
            PagingInfo = new PagingInfo();
        }
    }

    public class GetListDataResult<T>
    {
        public PagingInfo PagingInfo
        {
            get; set;
        }

        public List<T> Data
        {
            get;
            set;
        }

        public GetListDataResult()
        {
            PagingInfo = new PagingInfo();
        }
    }
}
