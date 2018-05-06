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


using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public class AliOSS
    {
        private OssClient _client;
        private string _bucketName;
        private string _endpoint;

        public AliOSS()
        {
            AliyunConfigurationSection config =ConfigurationService.Instance.AliyunConfiguration;
            _endpoint = config.OSS.EndPoint;
            _bucketName = config.OSS.BucketName;
            _client = new OssClient(_endpoint, config.RTM.AccessKeyId, config.RTM.AccessKeySecret);
        }

        public AliOSS(string endpoint, string accessKeyId, string accessKeySecret,string bucketName)
        {
            _endpoint = endpoint;
            _bucketName = bucketName;
            _client = new OssClient(_endpoint, accessKeyId, accessKeySecret);
        }

        //https://help.aliyun.com/document_detail/31978.html?spm=5176.doc32086.6.868.WloID9
        //https://www.alibabacloud.com/help/zh/faq-detail/39607.htm
        //https://help.aliyun.com/document_detail/31837.html?spm=5176.doc32087.2.2.DGnG6C
        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间
        /// </summary>
        /// <param name="bucketName">指定的存储空间名称</param>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="fileToUpload">指定上传文件的本地路径</param>
        public string PutObject(string key, string fileToUpload)
        {
            PutObjectResult putObjectResult = _client.PutObject(_bucketName, key, fileToUpload);
            return "http://" + _bucketName + "." + _endpoint + "/" + key;
        }
    }
}
