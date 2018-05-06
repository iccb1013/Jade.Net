/*
 *  Copyright (c) 2014 The CCP project authors. All Rights Reserved.
 *
 *  Use of this source code is governed by a Beijing Speedtong Information Technology Co.,Ltd license
 *  that can be found in the LICENSE file in the root of the web site.
 *
 *   http://www.yuntongxun.com
 *
 *  An additional intellectual property rights grant can be found
 *  in the file PATENTS.  All contributing project authors may
 *  be found in the AUTHORS file in the root of the source tree.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace CCPRestSDK
{
    enum EBodyType:uint
    {
        EType_XML = 0,
        EType_JSON
    };

    public class CCPRestSDK
    {
        private string m_restAddress = null;
        private string m_restPort = null;
        private string m_mainAccount = null;
        private string m_mainToken = null;
        private string m_subAccount = null;
        private string m_subToken = null;
        private string m_voipAccount = null;
        private string m_voipPwd = null;
        private string m_appId = null;
        private bool m_isWriteLog = false;

        private EBodyType m_bodyType = EBodyType.EType_XML;

        /// <summary>
        /// 服务器api版本
        /// </summary>
        const string softVer = "2013-12-26";

        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="serverIP">服务器地址</param>
        /// <param name="serverPort">服务器端口</param>
        /// <returns></returns>
        public bool init(string restAddress, string restPort)
        {
            this.m_restAddress = restAddress;
            this.m_restPort = restPort;

            if (m_restAddress == null || m_restAddress.Length < 0 || m_restPort == null || m_restPort.Length < 0 || Convert.ToInt32(m_restPort) < 0)
                return false;

            return true;
        }

        /// <summary>
        /// 设置主帐号信息
        /// </summary>
        /// <param name="accountSid">主帐号</param>
        /// <param name="accountToken">主帐号令牌</param>
        public void setAccount(string accountSid, string accountToken)
        {
            this.m_mainAccount = accountSid;
            this.m_mainToken = accountToken;
        }

        /// <summary>
        /// 设置子帐号信息
        /// </summary>
        /// <param name="subAccountSid">子帐号</param>
        /// <param name="subAccountToken">子帐号令牌</param>
        /// <param name="voipAccount">VoIP帐号</param>
        /// <param name="voipPassword">VoIP密码</param>
        public void setSubAccount(string subAccountSid, string subAccountToken, string voipAccount, string voipPassword)
        {
            this.m_subAccount = subAccountSid;
            this.m_subToken = subAccountToken;
            this.m_voipAccount = voipAccount;
            this.m_voipPwd = voipPassword;
        }

        /// <summary>
        /// 设置应用ID
        /// </summary>
        /// <param name="appId">应用ID</param>
        public void setAppId(string appId)
        {
            this.m_appId = appId;
        }

        /// <summary>
        /// 日志开关
        /// </summary>
        /// <param name="enable">日志开关</param>
        public void enabeLog(bool enable)
        {
            this.m_isWriteLog = enable;
        }

        /// <summary>
        /// 获取日志路径
        /// </summary>
        /// <returns>日志路径</returns>
        public string GetLogPath()
        {
            string dllpath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            dllpath = dllpath.Substring(8, dllpath.Length - 8);    // 8是 file:// 的长度
            return System.IO.Path.GetDirectoryName(dllpath)+"\\log.txt";
        }

        /// <summary>
        /// 主帐号信息查询
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <returns>包体内容</returns>
        public Dictionary<string,object> QueryAccountInfo()
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/AccountInfo?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("QueryAccountInfo url = "+uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "GET";
                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);

                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream  
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string responseStr = reader.ReadToEnd();

                        WriteLog("QueryAccountInfo responseBody = " + responseStr);

                        if (responseStr != null && responseStr.Length > 0)
                        {
                            Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "Account")
                                {
                                    Dictionary<string, object> data = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        data.Add(subItem.Name, subItem.InnerText);
                                    }
                                    responseResult["data"] = new Dictionary<string, object> {{item.Name,data}};
                                }
                            }
                            return responseResult;
                        }
                        return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                    }
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string responseStr = reader.ReadToEnd();

                        WriteLog("QueryAccountInfo responseBody = " + responseStr);

                        if (responseStr != null && responseStr.Length > 0)
                        {
                            Dictionary<string, object> responseResult = new Dictionary<string, object> ();
                            responseResult["resposeBody"] = responseStr;
                            return responseResult;
                        }
                        return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                    }
                }


                // 获取请求
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 创建子帐号
        /// </summary>
        /// <param name="friendlyName">子帐号名称。可由英文字母和阿拉伯数字组成子帐号唯一名称，推荐使用电子邮箱地址</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns>包体内容</returns>
        public Dictionary<string, object> CreateSubAccount(string friendlyName) 
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if(friendlyName == null)
                throw new ArgumentNullException("friendlyName");

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/SubAccounts?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("CreateSubAccount url = " +uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();

                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><SubAccount>");
                    data.Append("<appId>").Append(m_appId).Append("</appId>");
                    data.Append("<friendlyName>").Append(friendlyName).Append("</friendlyName>");
                    data.Append("</SubAccount>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"appId\":\"").Append(m_appId).Append("\"");
                    data.Append(",\"friendlyName\":\"").Append(friendlyName).Append("\"");
                    data.Append("}");
                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("CreateSubAccount requestBody = " +data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("CreateSubAccount responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "SubAccount")
                                {
                                    Dictionary<string, object> retData = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        retData.Add(subItem.Name, subItem.InnerText);
                                    }
                                    responseResult["data"] = new Dictionary<string, object> { { item.Name, retData } };
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取应用下子帐号
        /// </summary>
        /// <param name="startNo">开始的序号，默认从0开始</param>
        /// <param name="offset">一次查询的最大条数，最小是1条，最大是100条</param>
        /// <exception cref="ArgumentOutOfRangeException">参数超出范围</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public Dictionary<string, object> GetSubAccounts(uint startNo, uint offset)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (offset < 1 || offset > 100)
            {
                throw new ArgumentOutOfRangeException("offset超出范围");
            }
            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/GetSubAccounts?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("GetSubAccounts url = "+uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();

                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><SubAccount>");
                    data.Append("<appId>").Append(m_appId).Append("</appId>");
                    data.Append("<startNo>").Append(startNo).Append("</startNo>");
                    data.Append("<offset>").Append(offset).Append("</offset>");
                    data.Append("</SubAccount>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"appId\":\"").Append(m_appId).Append("\"");
                    data.Append(",\"startNo\":\"").Append(startNo).Append("\"");
                    data.Append(",\"offset\":\"").Append(offset).Append("\"");
                    data.Append("}");
                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("GetSubAccounts requestBody = " + data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("GetSubAccounts responseBody = "+responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };
                        Dictionary<string, object> retData = new Dictionary<string, object>();
                        List<object> subAccountList = new List<object>();

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "totalCount")
                                {
                                    retData.Add(item.Name, item.InnerText);
                                }
                                else if (item.Name == "SubAccount")
                                {
                                    Dictionary<string, object> SubAccount = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        SubAccount.Add(subItem.Name, subItem.InnerText);
                                    }
                                    subAccountList.Add(SubAccount);
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }

                        if (retData.Count > 0)
                        {
                            if (subAccountList.Count > 0)
                            {
                                retData.Add("SubAccount", subAccountList);
                            }
                            responseResult["data"] = retData;
                        }
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 查询子帐号信息
        /// </summary>
        /// <param name="friendlyName">子帐号名称</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns>包体内容</returns>
        public Dictionary<string, object> QuerySubAccount(string friendlyName)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (friendlyName == null)
                throw new ArgumentNullException("friendlyName");

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/QuerySubAccountByName?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("QuerySubAccount url = " + uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();

                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><SubAccount>");
                    data.Append("<appId>").Append(m_appId).Append("</appId>");
                    data.Append("<friendlyName>").Append(friendlyName).Append("</friendlyName>");
                    data.Append("</SubAccount>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"appId\":\"").Append(m_appId).Append("\"");
                    data.Append(",\"friendlyName\":\"").Append(friendlyName).Append("\"");
                    data.Append("}");
                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("QuerySubAccount requestBody = "+data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("QuerySubAccount responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "SubAccount")
                                {
                                    Dictionary<string, object> retData = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        retData.Add(subItem.Name, subItem.InnerText);
                                    }
                                    responseResult["data"] = new Dictionary<string, object> { { item.Name, retData } };
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// 模板短信
        /// </summary>
        /// <param name="to">短信接收端手机号码集合，用英文逗号分开，每批发送的手机号数量不得超过100个</param>
        /// <param name="templateId">模板Id</param>
        /// <param name="data">可选字段 内容数据，用于替换模板中{序号}</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public Dictionary<string, object> SendTemplateSMS(string to, string templateId, string[] data)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            if (templateId == null)
            {
                throw new ArgumentNullException("templateId");
            }

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/SMS/TemplateSMS?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("SendTemplateSMS url = " + uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder bodyData = new StringBuilder();

                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    bodyData.Append("<?xml version='1.0' encoding='utf-8'?><TemplateSMS>");
                    bodyData.Append("<to>").Append(to).Append("</to>");
                    bodyData.Append("<appId>").Append(m_appId).Append("</appId>");
                    bodyData.Append("<templateId>").Append(templateId).Append("</templateId>");
                    if (data != null && data.Length>0)
                    {
                        bodyData.Append("<datas>");
                        foreach (string item in data)
                        {
                            bodyData.Append("<data>").Append(item).Append("</data>");
                        }
                        bodyData.Append("</datas>");
                    }
                    bodyData.Append("</TemplateSMS>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    bodyData.Append("{");
                    bodyData.Append("\"to\":\"").Append(to).Append("\"");
                    bodyData.Append(",\"appId\":\"").Append(m_appId).Append("\"");
                    bodyData.Append(",\"templateId\":\"").Append(templateId).Append("\"");
                    if (data != null && data.Length > 0)
                    {
                        bodyData.Append(",\"datas\":[");
                        int index = 0;
                        foreach (string item in data)
                        {
                            if (index == 0)
                            {
                                bodyData.Append("\""+item+"\"");
                            }
                            else
                            {
                                bodyData.Append(",\"" + item + "\"");
                            }
                            index++;
                        }
                        bodyData.Append("]");
                    }
                    bodyData.Append("}");

                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(bodyData.ToString());

                WriteLog("SendTemplateSMS requestBody = " + bodyData.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("SendTemplateSMS responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "TemplateSMS")
                                {
                                    Dictionary<string, object> retData = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        retData.Add(subItem.Name, subItem.InnerText);
                                    }
                                    responseResult["data"] = new Dictionary<string, object> { { item.Name, retData } };
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        /// <summary>
        /// 外呼通知
        /// </summary>
        /// <param name="to">被叫号码</param>
        /// <param name="mediaName">可选字段 语音文件名称，格式 wav。与mediaTxt不能同时为空，不为空时mediaTxt属性失效</param>
        /// <param name="mediaTxt">可选字段 文本内容，默认值为空</param>
        /// <param name="displayNum">可选字段 显示的主叫号码，显示权限由服务侧控制</param>
        /// <param name="playTimes">可选字段 循环播放次数，1－3次，默认播放1次</param>
        /// <param name="type">可选字段 语音文件名的类型，默认值为0，表示用户语音文件；值为1表示平台通用文件</param>
        /// <param name="respUrl">可选字段 外呼通知状态通知回调地址，云通讯平台将向该Url地址发送呼叫结果通知</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public Dictionary<string, object> LandingCall(string to, string mediaName, string mediaTxt, string displayNum, string playTimes, int type, string respUrl)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            if (mediaName==null && mediaTxt==null)
            {
                throw new ArgumentNullException("mediaName和mediaTxt同时为null");
            }

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/Calls/LandingCalls?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("LandingCall url = " + uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();
                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><LandingCall>");
                    data.Append("<appId>").Append(m_appId).Append("</appId>");
                    data.Append("<to>").Append(to).Append("</to>");

                    if (mediaName != null)
                    {
                        data.Append("<mediaName type=\""+type+"\">").Append(mediaName).Append("</mediaName>");
                    }

                    if (mediaTxt != null)
                    {
                        data.Append("<mediaTxt>").Append(mediaTxt).Append("</mediaTxt>");
                    }

                    if (displayNum!=null)
                    {
                        data.Append("<displayNum>").Append(displayNum).Append("</displayNum>");
                    }

                    if (playTimes!=null)
                    {
                        data.Append("<playTimes>").Append(playTimes).Append("</playTimes>");
                    }

                    if (respUrl!=null)
                    {
                        data.Append("<respUrl>").Append(respUrl).Append("</respUrl>");
                    }
                    
                    data.Append("</LandingCall>");

                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"to\":\"").Append(to).Append("\"");
                    data.Append(",\"appId\":\"").Append(m_appId).Append("\"");
                    if (mediaName != null)
                    {
                        data.Append(",\"mediaName\":\"").Append(mediaName).Append("\"");
                        data.Append(",\"mediaNameType\":\"").Append(type).Append("\"");
                    }

                    if (mediaTxt != null)
                    {
                        data.Append(",\"mediaTxt\":\"").Append(mediaTxt).Append("\"");
                    }

                    if (displayNum != null)
                    {
                        data.Append(",\"displayNum\":\"").Append(displayNum).Append("\"");
                    }

                    if (playTimes != null)
                    {
                        data.Append(",\"playTimes\":\"").Append(playTimes).Append("\"");
                    }

                    if (respUrl != null)
                    {
                        data.Append(",\"respUrl\":\"").Append(respUrl).Append("\"");
                    }

                    data.Append("}");
                }
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("LandingCall requestBody = " + data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("LandingCall responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "LandingCall")
                                {
                                    Dictionary<string, object> retData = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        retData.Add(subItem.Name, subItem.InnerText);
                                    }
                                    responseResult["data"] = new Dictionary<string, object> { { item.Name, retData } };
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 语音验证码
        /// </summary>
        /// <param name="to">接收号码</param>
        /// <param name="verifyCode">验证码内容，为数字和英文字母，不区分大小写，长度4-8位</param>
        /// <param name="displayNum">可选字段 显示主叫号码，显示权限由服务侧控制</param>
        /// <param name="playTimes">可选字段 循环播放次数，1－3次，默认播放1次</param>
        /// <param name="respUrl">可选字段 语音验证码状态通知回调地址，云通讯平台将向该Url地址发送呼叫结果通知</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public Dictionary<string, object> VoiceVerify(string to, string verifyCode, string displayNum, string playTimes, string respUrl)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            if (verifyCode == null)
            {
                throw new ArgumentNullException("verifyCode");
            }

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/Calls/VoiceVerify?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("VoiceVerify url = " + uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();
                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><VoiceVerify>");
                    data.Append("<appId>").Append(m_appId).Append("</appId>");
                    data.Append("<verifyCode>").Append(verifyCode).Append("</verifyCode>");
                    data.Append("<to>").Append(to).Append("</to>");

                    if (displayNum != null)
                    {
                        data.Append("<displayNum>").Append(displayNum).Append("</displayNum>");
                    }

                    if (playTimes != null)
                    {
                        data.Append("<playTimes>").Append(playTimes).Append("</playTimes>");
                    }

                    if (respUrl != null)
                    {
                        data.Append("<respUrl>").Append(respUrl).Append("</respUrl>");
                    }

                    data.Append("</VoiceVerify>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"to\":\"").Append(to).Append("\"");
                    data.Append(",\"appId\":\"").Append(m_appId).Append("\"");
                    data.Append(",\"verifyCode\":\"").Append(verifyCode).Append("\"");

                    if (displayNum != null)
                    {
                        data.Append(",\"displayNum\":\"").Append(displayNum).Append("\"");
                    }

                    if (playTimes != null)
                    {
                        data.Append(",\"playTimes\":\"").Append(playTimes).Append("\"");
                    }

                    if (respUrl != null)
                    {
                        data.Append(",\"respUrl\":\"").Append(respUrl).Append("\"");
                    }

                    data.Append("}");

                }
                
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("VoiceVerify requestBody = " + data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("VoiceVerify responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else if (item.Name == "VoiceVerify")
                                {
                                    Dictionary<string, object> retData = new Dictionary<string, object>();
                                    foreach (XmlNode subItem in item.ChildNodes)
                                    {
                                        retData.Add(subItem.Name, subItem.InnerText);
                                    }
                                    responseResult["data"] = new Dictionary<string, object> { { item.Name, retData } };
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// IVR外呼 暂不支持json格式
        /// </summary>
        /// <param name="number">待呼叫号码</param>
        /// <param name="userdata">可选字段 用户数据，在<startservice>通知中返回，只允许填写数字字符</param>
        /// <param name="record">可选字段 是否录音，可填项为true和false，默认值为false不录音</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public Dictionary<string, object> IvrDial(string number, string userdata, string record)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (number == null)
            {
                throw new ArgumentNullException("number");
            }

            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + date);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/ivr/dial?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("IvrDial url = " + uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";
                request.Accept = "application/xml";
                request.ContentType = "application/xml;charset=utf-8";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();

                data.Append("<?xml version='1.0' encoding='utf-8'?><Request>");
                data.Append("<Appid>").Append(m_appId).Append("</Appid>");
                data.Append("<Dial ");
                data.Append("number=\"" + number + "\"");
                if (userdata != null)
                {
                    data.Append(" userdata=\"" + userdata + "\"");
                }

                if (record != null && record=="true")
                {
                    data.Append(" record=\"true\"");
                }

                data.Append("></Dial>");
                data.Append("</Request>");

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("IvrDial requestBody = " + data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("IvrDial responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 话单下载
        /// </summary>
        /// <param name="date">day 代表前一天的数据（从00:00 – 23:59）;week代表前一周的数据(周一 到周日)；month表示上一个月的数据（上个月表示当前月减1，如果今天是4月10号，则查询结果是3月份的数据）</param>
        /// <param name="keywords">客户的查询条件，由客户自行定义并提供给云通讯平台。默认不填忽略此参数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public Dictionary<string, object> BillRecords(string date, string keywords)
        {
            Dictionary<string, object> initError = paramCheckRest();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckMainAccount();
            if (initError != null)
            {
                return initError;
            }
            initError = paramCheckAppId();
            if (initError != null)
            {
                return initError;
            }

            if (date == null)
            {
                throw new ArgumentNullException("date");
            }

            try
            {
                string mydate = DateTime.Now.ToString("yyyyMMddhhmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(m_mainAccount + m_mainToken + mydate);
                string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/BillRecords?sig={4}", m_restAddress, m_restPort, softVer, m_mainAccount, sigstr);
                Uri address = new Uri(uriStr);

                WriteLog("BillRecords url = " + uriStr);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(m_mainAccount + ":" + mydate);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();
                if (m_bodyType == EBodyType.EType_XML)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><BillRecords>");
                    data.Append("<appId>").Append(m_appId).Append("</appId>");
                    data.Append("<date>").Append(date).Append("</date>");

                    if (keywords != null)
                    {
                        data.Append("<keywords>").Append(keywords).Append("</keywords>");
                    }

                    data.Append("</BillRecords>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"appId\":\"").Append(m_appId).Append("\"");
                    data.Append(",\"date\":\"").Append(date).Append("\"");

                    if (keywords != null)
                    {
                        data.Append(",\"keywords\":\"").Append(keywords).Append("\"");
                    }
                    data.Append("}");

                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                WriteLog("BillRecords requestBody = " + data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();

                    WriteLog("BillRecords responseBody = " + responseStr);

                    if (responseStr != null && responseStr.Length > 0)
                    {
                        Dictionary<string, object> responseResult = new Dictionary<string, object> { { "statusCode", "0" }, { "statusMsg", "成功" }, { "data", null } };
                        Dictionary<string, object> retData = new Dictionary<string, object>();

                        if (m_bodyType == EBodyType.EType_XML)
                        {
                            XmlDocument resultXml = new XmlDocument();
                            resultXml.LoadXml(responseStr);
                            XmlNodeList nodeList = resultXml.SelectSingleNode("Response").ChildNodes;
                            foreach (XmlNode item in nodeList)
                            {
                                if (item.Name == "statusCode")
                                {
                                    responseResult["statusCode"] = item.InnerText;
                                }
                                else if (item.Name == "statusMsg")
                                {
                                    responseResult["statusMsg"] = item.InnerText;
                                }
                                else
                                {
                                    retData.Add(item.Name, item.InnerText);
                                }
                            }
                        }
                        else
                        {
                            responseResult.Clear();
                            responseResult["resposeBody"] = responseStr;
                        }
                        
                        if (retData.Count > 0)
                        {
                            responseResult["data"] = retData;
                        }
                        return responseResult;
                    }
                    return new Dictionary<string, object> { { "statusCode", 172002 }, { "statusMsg", "无返回" }, { "data", null } };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #region MD5 和 https交互函数定义

        private void WriteLog(string log)
        {
            if (m_isWriteLog)
            {
                string strFilePath = GetLogPath();
                System.IO.FileStream fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Append);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(DateTime.Now.ToString() + "\t" + log);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 检查服务器地址信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> paramCheckRest()
        {
            int statusCode = 0;
            string statusMsg = null;

            if (m_restAddress == null)
            {
                statusCode = 172004;
                statusMsg = "IP空";
            }
            else if (m_restPort == null)
            {
                statusCode = 172005;
                statusMsg = "端口错误";
            }

            if (statusCode != 0)
            {
                return new Dictionary<string, object> { { "statusCode", statusCode + "" }, { "statusMsg", statusMsg }, { "data", null } };
            }

            return null;
        }

        /// <summary>
        /// 检查主帐号信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> paramCheckMainAccount()
        {
            int statusCode = 0;
            string statusMsg = null;

            if (m_mainAccount == null)
            {
                statusCode = 172006;
                statusMsg = "主帐号空";
            }
            else if (m_mainToken == null)
            {
                statusCode = 172007;
                statusMsg = "主帐号令牌空";
            }

            if (statusCode != 0)
            {
                return new Dictionary<string, object> { { "statusCode", statusCode + "" }, { "statusMsg", statusMsg }, { "data", null } };
            }

            return null;
        }

        /// <summary>
        /// 检查子帐号信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> paramCheckSunAccount()
        {
            int statusCode = 0;
            string statusMsg = null;

            if (m_subAccount == null)
            {
                statusCode = 172008;
                statusMsg = "子帐号空";
            }
            else if (m_subToken == null)
            {
                statusCode = 172009;
                statusMsg = "子帐号令牌空";
            }
            else if (m_voipAccount == null)
            {
                statusCode = 1720010;
                statusMsg = "VoIP帐号空";
            }
            else if (m_voipPwd == null)
            {
                statusCode = 172011;
                statusMsg = "VoIP密码空";
            }

            if (statusCode != 0)
            {
                return new Dictionary<string, object> { { "statusCode", statusCode+"" }, { "statusMsg", statusMsg }, { "data", null } };
            }

            return null;
        }

        /// <summary>
        /// 检查应用ID
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> paramCheckAppId()
        {
            if (m_appId == null)
            {
                return new Dictionary<string, object> { { "statusCode", 172012 + "" }, { "statusMsg", "应用ID为空" }, { "data", null } };
            }

            return null;
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">原内容</param>
        /// <returns>加密后内容</returns>
        public static string MD5Encrypt(string source)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();
            
            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        /// <summary>
        /// 设置服务器证书验证回调
        /// </summary>
        public void setCertificateValidationCallBack()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = CertificateValidationResult;
        }

        /// <summary>
        ///  证书验证回调函数  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cer"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool CertificateValidationResult(object obj, System.Security.Cryptography.X509Certificates.X509Certificate cer, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }
        #endregion
    }
}
