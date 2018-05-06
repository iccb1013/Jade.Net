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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Kernal
{
    public class HttpRequestResult
    {
        public bool Success
        {
            get
            {
                return Exception == null;
            }
        }

        public Exception Exception
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public HttpRequestArgs HttpRequestArgs
        {
            get;
            set;
        }

        /// <summary>
        /// 获取详细信息，用来写日志
        /// </summary>
        /// <returns></returns>
        public string GetDetail()
        {
            StringBuilder str = new StringBuilder();
            if (HttpRequestArgs != null)
            {
                str.Append("请求的URL：");
                str.AppendLine();
                str.Append(HttpRequestArgs.Url);
                str.AppendLine();
                str.Append("发送的内容：");
                str.AppendLine();
                str.Append(HttpRequestArgs.Content);
                str.AppendLine();
                if (String.IsNullOrEmpty(HttpRequestArgs.File) == false)
                {
                    str.Append("文件：");
                    str.AppendLine();
                    str.Append(HttpRequestArgs.File);
                    str.AppendLine();
                }
            }
            if (this.Exception != null)
            {
                str.Append("异常：");
                str.AppendLine();
                str.Append(this.Exception.Message);
                str.AppendLine();
            }
            else
            {
                str.Append("返回的内容：");
                str.AppendLine();
                str.Append(this.Content);
                str.AppendLine();
            }

            return str.ToString();
        }

    }
}
