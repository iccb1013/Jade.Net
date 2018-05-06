/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*
*    ? Copyright 2017
*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Sheng.Kernal;

namespace Sheng.Web.Infrastructure
{
    public class MyPushClient
    {
        protected const String USER_AGENT = "Mozilla/5.0";

	    // This object is used for sending the post request to Umeng
	    protected HttpClient client = new HttpClient();
	
	    // The host
	    protected const String host = "http://msg.umeng.com";
	
	    // The upload path
	    protected const String uploadPath = "/upload";
	
	    // The post path
	    protected const String postPath = "/api/send";

        public bool send(MyUmengBaseNotification msg,string appMasterSecret)
        {
            HttpWebResponse response = null;
            Stream receiveStream = null;
            StreamReader readStream = null;

            MemoryStream postStream = new MemoryStream();
            try
            {
                // Construct the result
                HttpRequestResult result = new HttpRequestResult();

                string timestamp = GetTimeStamp().ToString();

                msg.timestamp = timestamp;

                // Construct the json string
                string postBody = JsonConvert.SerializeObject(msg);
                string url = host + postPath;
                string sign = IOHelper.GetMD5HashFromString("POST" + url + postBody + appMasterSecret);
                url = url + "?sign=" + sign;

                // Construct the request
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.UserAgent = USER_AGENT;

                byte[] bs = Encoding.UTF8.GetBytes(postBody);
                request.ContentLength = bs.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }

                response = (HttpWebResponse)request.GetResponse();

                receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                readStream = new StreamReader(receiveStream, encode);

                result.Content = readStream.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                { 
                    return true;
                }
                else
                { 
                    return false;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse res = (HttpWebResponse)ex.Response;
                Stream myResponseStream = res.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                Debug.Assert(false, retString);
                throw;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw;
            }
            finally
            {
                if (response != null)
                    response.Close();
                if (receiveStream != null)
                    receiveStream.Close();
                if (readStream != null)
                    readStream.Close();
                if (postStream != null)
                    postStream.Close();
            }
        }

	    // Upload file with device_tokens to Umeng
	    public string uploadContents(string appkey, string appMasterSecret, string contents)
        {
            HttpWebResponse response = null;
            Stream receiveStream = null;
            StreamReader readStream = null;

            MemoryStream postStream = new MemoryStream();
	        try
	        {  

                // Construct the result
                HttpRequestResult result = new HttpRequestResult(); 
	            string timestamp = GetTimeStamp().ToString();

	            var jsonObject = new
	            {
	                appkey = appkey,
	                timestamp = timestamp,
	                content = contents
	            };

	            // Construct the json string
	            string postBody = JsonConvert.SerializeObject(jsonObject);
	            string url = host + uploadPath;
	            string sign = IOHelper.GetMD5HashFromString("POST" + url + postBody + appMasterSecret);
	            url = url + "?sign=" + sign;

                // Construct the request

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; 
                request.Method = "POST";
	            request.UserAgent = USER_AGENT;

                byte[] bs = Encoding.ASCII.GetBytes(postBody);
                request.ContentLength = bs.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                } 

                response = (HttpWebResponse) request.GetResponse();

	            receiveStream = response.GetResponseStream();
	            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
	            readStream = new StreamReader(receiveStream, encode);

	            result.Content = readStream.ReadToEnd();

	            MyUmResult umReturnResult = JsonConvert.DeserializeObject<MyUmResult>(result.Content);

	            if (!umReturnResult.ret.Equals("SUCCESS"))
	            {
	                throw new Exception("Failed to upload file");
	            }
	            string fileId = umReturnResult.data.file_id;

	            return fileId;
            }
            catch (WebException ex)
            {
                HttpWebResponse res = (HttpWebResponse)ex.Response;
                Stream myResponseStream = res.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                Debug.Assert(false, retString); 
                throw;
            }
            catch (Exception ex)
	        {
	            Debug.Assert(false, ex.Message);
	            throw;
	        }
            finally
            {
                if (response != null)
                    response.Close();
                if (receiveStream != null)
                    receiveStream.Close();
                if (readStream != null)
                    readStream.Close();
                if (postStream != null)
                    postStream.Close();
            }
      }
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        private uint GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Convert.ToUInt32(ts.TotalSeconds);
        }
    } 
 }

 
