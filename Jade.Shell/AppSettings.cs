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
using System.Configuration;
using System.Linq;
using System.Web;

namespace Jade.Shell
{
    public static class AppSettings
    {
        private static string _rootPath;
        /// <summary>
        /// 站点所在物理目录
        /// 如 D:\WorkFloder\WeixinConstruction\trunk\SourceCode\Sheng.WeixinConstruction.FileService\
        /// </summary>
        public static string RootPath
        {
            get
            {
                if (String.IsNullOrEmpty(_rootPath))
                {
                    _rootPath = ConfigurationManager.AppSettings["RootPath"];
                }
                return _rootPath;
            }
        }
    }
}