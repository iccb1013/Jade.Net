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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public class MyPushService
    {
        protected static readonly string APP_ANDROID_KEY    = "";
        protected static readonly string APP_ANDROID_SECRET = ""; 

        protected static readonly string APP_IOS_KEY    = "";
        protected static readonly string APP_IOS_SECRET = ""; 
        public bool SendAndroidCustomizedcastFile(string title, string msg, string fileIds)
        {
            MyPushClient pushClient = new MyPushClient();

            string fileId = pushClient.uploadContents(APP_ANDROID_KEY, APP_ANDROID_SECRET, fileIds + "alias");

            MyUmengBaseNotification umengNotification = new MyUmengBaseNotification();
            umengNotification.appkey = APP_ANDROID_KEY;
            umengNotification.production_mode = "true";
            umengNotification.alias_type = "jadeMember";
            umengNotification.type = "customizedcast";
            umengNotification.file_id = fileId;

            var body = new
            {
                text = msg,
                title = title,
                ticker = "收到一条张寿宴玉雕消息通知",
                after_open = "go_app",
            };
            
            var payload = new { body = body, display_type = "notification"};

            umengNotification.payload = payload;

            return pushClient.send(umengNotification, APP_ANDROID_SECRET);
        }
        public bool SendAndroidCustomizedcastFile(string title, string msg, string fileIds, string activity, string url)
        {
            MyPushClient pushClient = new MyPushClient();

            string fileId = pushClient.uploadContents(APP_ANDROID_KEY, APP_ANDROID_SECRET, fileIds + "alias");

            MyUmengBaseNotification umengNotification = new MyUmengBaseNotification();
            umengNotification.appkey = APP_ANDROID_KEY;
            umengNotification.production_mode = "true";
            umengNotification.alias_type = "jadeMember";
            umengNotification.type = "customizedcast"; 
            umengNotification.file_id = fileId;

            var body = new
            {
                text = msg,
                title = title,
                ticker = "收到一条张寿宴玉雕消息通知",
                after_open = "go_activity",
                activity = "com.android.zhangsy.H5urlActivity"
            };
             
            if (String.IsNullOrEmpty(url) == false)
            {
                var extra = new { theme_url = url }; 
                var payload = new { body = body, display_type = "notification", extra = extra };
                umengNotification.payload = payload;
            }
            else
            {
                var payload = new { body = body, display_type = "notification", theme_url = url };
                umengNotification.payload = payload;
            }

            return pushClient.send(umengNotification, APP_ANDROID_SECRET);
        }

        public bool SendIOSCustomizedcast(string title, string msg, string fileIds, string goActivity, string url)
        {
            MyPushClient pushClient = new MyPushClient();
            
            string fileId = pushClient.uploadContents(APP_IOS_KEY, APP_IOS_SECRET, fileIds + "alias");

            MyUmengIOSNotification umengNotification = new MyUmengIOSNotification();
            umengNotification.appkey = APP_IOS_KEY;
            umengNotification.production_mode = "true";
            umengNotification.alias_type = "jadeMemberIOS";
            umengNotification.type = "customizedcast";
            umengNotification.description = msg;
            umengNotification.file_id = fileId;

            var aps = new
            {
                sound= "default",
                alert =  msg,
                badge= 0
            };

            var payload = new { aps = aps, actionType = goActivity,theme_url = url };

            umengNotification.payload = payload;
            
            return pushClient.send(umengNotification, APP_IOS_SECRET);
        }

    }
}
