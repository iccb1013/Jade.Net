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
    public class AliOSSHelper
    {
        private static readonly AliOSSHelper _instance = new AliOSSHelper();
        public static AliOSSHelper Instance
        {
            get { return _instance; }
        }

        private AliOSS _oss = new AliOSS();
        public AliOSS OSS
        {
            get { return _oss; }
            set { _oss = value; }
        }

        private AliOSSHelper()
        {

        }


    }
}
