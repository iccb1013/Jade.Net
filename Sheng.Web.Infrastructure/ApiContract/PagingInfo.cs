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
    public class PagingInfo
    {
        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (value < 1)
                    value = 1;
                _currentPage = value;
            }
        }

        private int _totalPage = 1;
        public int TotalPage
        {
            get { return _totalPage; }
            set
            {
                if (value < 1)
                    value = 1;
                _totalPage = value;
            }
        }

        private int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value < 1)
                    value = 1;
                _pageSize = value;
            }
        }

        public int TotalCount
        {
            get; set;
        }


        public void UpdateTotalCount(int totalCount)
        {
            TotalCount = totalCount;

            int totalPage = totalCount / _pageSize;
            if (totalCount % _pageSize > 0)
            {
                totalPage++;
            }
            if (totalPage == 0)
                totalPage = 1;

            TotalPage = totalPage;

            if (_currentPage > totalPage)
            {
                _currentPage = totalPage;
            }
        }
    }
}
