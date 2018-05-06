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
    public class YuntongxunConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("info", IsRequired = false)]
        public YuntongxunConfigurationElement Config
        {
            get { return base["info"] as YuntongxunConfigurationElement; }
            set { base["info"] = value; }
        }
    }

    #region RTM

    public class YuntongxunConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("restAddress", IsRequired = true)]
        public string RestAddress
        {
            get { return base["restAddress"].ToString(); }
            set { base["restAddress"] = value; }
        }

        [ConfigurationProperty("restPort", IsRequired = true)]
        public string RestPort
        {
            get { return base["restPort"].ToString(); }
            set { base["restPort"] = value; }
        }

        [ConfigurationProperty("restVersion", IsRequired = true)]
        public string RestVersion
        {
            get { return base["restVersion"].ToString(); }
            set { base["restVersion"] = value; }
        }

        [ConfigurationProperty("account", IsRequired = true)]
        public string Account
        {
            get { return base["account"].ToString(); }
            set { base["account"] = value; }
        }

        [ConfigurationProperty("token", IsRequired = true)]
        public string Token
        {
            get { return base["token"].ToString(); }
            set { base["token"] = value; }
        }

        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get { return base["appId"].ToString(); }
            set { base["appId"] = value; }
        }
    }

    #endregion

}
