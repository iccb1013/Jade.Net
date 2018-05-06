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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public class ConfigurationService
    {
        private static readonly ConfigurationService _instance = new ConfigurationService();
        public static ConfigurationService Instance
        {
            get { return _instance; }
        }


        public AliyunConfigurationSection AliyunConfiguration
        {
            get;
            private set;
        }

        public YuntongxunConfigurationSection YuntongxunConfiguration
        {
            get;
            private set;
        }


        private ConfigurationService()
        {
            AliyunConfiguration =
                ConfigurationManager.GetSection("aliyunConfiguration") as AliyunConfigurationSection;

            YuntongxunConfiguration =
                ConfigurationManager.GetSection("yuntongxunConfiguration") as YuntongxunConfigurationSection;
        }
    }
}
