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


using Jade.Core;
using Jade.Model.Dto.AppDto;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class AppApiBaseController : Controller
    {
        private CachingService _cachingService = CachingService.Instance;

        public MemberUserContext UserContext
        {
            get;
            set;
        }

        protected override void OnException(ExceptionContext filterContext)
        {  
            filterContext.ExceptionHandled = true;//组织web.config配置customerror处理 
            filterContext.Result = FailedResult("系统异常。"); 
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            object[] objAllowedAnonymousArray =
               filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowedAnonymous), false);
            if (objAllowedAnonymousArray.Length > 0)
                return;

            //从请求中提取token
            string token = Request.Headers["token"];
            if (String.IsNullOrEmpty(token))
            {
                AppApiResult apiResult = new AppApiResult()
                {
                    result = "error",
                    message = "会话失效，请重新登录"
                };

                filterContext.Result = ApiResult(apiResult);
                return;
            }

            UserContext = _cachingService.Get<MemberUserContext>(token);

            if (UserContext == null)
            {
                AppApiResult apiResult = new AppApiResult()
                {
                    result = "error",
                    message = "会话失效，请重新登录"
                };

                filterContext.Result = ApiResult(apiResult);
                return;
            }
        }
        protected TObj RequestArgs<TObj>() where TObj : class
        {
            StreamReader reader = new StreamReader(HttpContext.Request.InputStream);
            //这里不能用HttpUtility.UrlDecode，会把 + 号忽略掉！
            //例如日期时间序列化后会变为：
            //Date(1412957280374+0800)
            string json = reader.ReadToEnd();//HttpUtility.UrlDecode(reader.ReadToEnd());

            TObj requestObj = JsonHelper.NewtonsoftDeserialize<TObj>(json);
            return requestObj;
        }

        #region RespondResult

        protected virtual ContentResult SuccessfulResult()
        {
            return ApiResult(true, null, null);
        }

        protected virtual ContentResult FailedResult(string message)
        {
            AppApiResult apiResult = new AppApiResult()
            {
                result = "error",
                message = message,
                token = UserContext?.Token
            };
            return ApiResult(apiResult);
        }

        protected virtual ContentResult DataResult(object data)
        {
            return ApiResult(true, "查询信息成功!", data);
        }

        protected virtual ContentResult ApiResult(bool successful,string message)
        {
            AppApiResult apiResult = new AppApiResult()
            {
                result = successful ? "success" : "error",
             //   errorcode = errorcode,
                message = message,
                token = UserContext?.Token
            };
            return ApiResult(apiResult);
        }

        protected virtual ContentResult ApiResult(bool successful, string message, object data)
        {
            AppApiResult apiResult = new AppApiResult()
            {
                result = successful? "success" : "error",
              //  errorcode = errorcode,
                message = message,
                data = data,
                token = UserContext?.Token
            };
            return ApiResult(apiResult);
        }

        protected virtual ContentResult ApiResult(AppApiResult apiResult)
        {
            ContentResult result = new ContentResult();
            result.ContentEncoding = Encoding.UTF8;
            result.Content = JsonHelper.NewtonsoftSerializer(apiResult);
            return result;
        }

       

        #endregion
    }
}