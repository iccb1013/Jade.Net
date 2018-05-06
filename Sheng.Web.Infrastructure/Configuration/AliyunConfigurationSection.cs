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
    public class AliyunConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 作为第三方平台运行时的参数配置
        /// </summary>
        [ConfigurationProperty("rtm", IsRequired = false)]
        public RTMConfigurationElement RTM
        {
            get { return base["rtm"] as RTMConfigurationElement; }
            set { base["rtm"] = value; }
        }

        /// <summary>
        /// 作为第三方平台运行时的参数配置
        /// </summary>
        [ConfigurationProperty("oss", IsRequired = false)]
        public OSSConfigurationElement OSS
        {
            get { return base["oss"] as OSSConfigurationElement; }
            set { base["oss"] = value; }
        }
    }

    #region RTM

    /// <summary>
    /// 系统拓扑结构设置
    /// </summary>
    public class RTMConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("accessKeyId", IsRequired = true)]
        public string AccessKeyId
        {
            get { return base["accessKeyId"].ToString(); }
            set { base["accessKeyId"] = value; }
        }

        [ConfigurationProperty("accessKeySecret", IsRequired = true)]
        public string AccessKeySecret
        {
            get { return base["accessKeySecret"].ToString(); }
            set { base["accessKeySecret"] = value; }
        }

    }

    #endregion

    #region OSS

    public class OSSConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("bucketName", IsRequired = true)]
        public string BucketName
        {
            get { return base["bucketName"].ToString(); }
            set { base["bucketName"] = value; }
        }

        [ConfigurationProperty("endPoint", IsRequired = true)]
        public string EndPoint
        {
            get { return base["endPoint"].ToString(); }
            set { base["endPoint"] = value; }
        }

      

    }

    #endregion

}
