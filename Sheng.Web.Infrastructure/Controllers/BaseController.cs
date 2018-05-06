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


using Newtonsoft.Json;
using Sheng.Web.Infrastructure;
using Sheng.Kernal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sheng.Web.Infrastructure
{
    public class BaseController : Controller
    {
        //public UserContext UserContext
        //{
        //    get;
        //    set;
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
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
            return ApiResult(true, null);
        }

        protected virtual ContentResult FailedResult(string message)
        {
            ApiResult apiResult = new ApiResult()
            {
                Successful = false,
                Message = message
            };
            return ApiResult(apiResult);
        }

        protected virtual ContentResult DataResult(object data)
        {
            return ApiResult(true, null, data);
        }

        protected virtual ContentResult ApiResult(bool successful, string message)
        {
            ApiResult apiResult = new ApiResult()
            {
                Successful = successful,
                Message = message
            };
            return ApiResult(apiResult);
        }

        protected virtual ContentResult ApiResult(bool successful, string message, object data)
        {
            ApiResult apiResult = new ApiResult()
            {
                Successful = successful,
                Message = message,
                Data = data
            };
            return ApiResult(apiResult);
        }

        protected virtual ContentResult ApiResult(ApiResult apiResult)
        {
            //JsonResult 无法序列化DataTable
            //JsonResult result = new JsonResult();
            //result.Data = apiResult;
            //result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return result;

            ContentResult result = new ContentResult();
            result.ContentEncoding = Encoding.UTF8;
            //result.Content = Newtonsoft.Json.JsonConvert.SerializeObject(apiResult);
            result.Content = JsonHelper.NewtonsoftSerializer(apiResult);
            return result;
        }

        #endregion
    }
}