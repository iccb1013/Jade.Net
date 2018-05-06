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


using AutoMapper;
using Jade.Core;
using Jade.Model;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Jade.Shell
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private LogService _logService = LogService.Instance;
        private ExceptionHandlingService _exceptionHandling = ExceptionHandlingService.Instance;


        protected void Application_Start()
        {
         //   Yuntongxun.SendTemplateSMS("18120125118", "230242", new string [] { "测试" });


            //确保第一时间调用  SetLogWriter 方法，否则如果 ExceptionHandlingService 先于它初始化
            //则会报错
            _logService.Write("Application_Start");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(AutoMapperConfiguration);
        }

        private static void AutoMapperConfiguration(IMapperConfigurationExpression x)
        {
            AutoMapperConfig.Configuration(x);
            AppAutoMapperConfig.Configuration(x);
        }

        //ASP.NET MVC3 异常处理 摘抄
        //http://blog.csdn.net/kufeiyun/article/details/8191673
        //为全局提供未捕获的异常处理，和用于 Controller 的过滤器是互补的
        //由MVC控制器调用所引发的异常将由其 Filter 处理，handled以后不会抛到这里
        //而由程序运行过程中自身产生的异常，在没有处理的情况下会一路抛到此处
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            DbEntityValidationException dbEntityException = exception as DbEntityValidationException;

            Server.ClearError();

            _exceptionHandling.HandleException(exception);

        }
    }
}
