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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    /// <summary>
    /// 将App用的传参方式由Json转成指定的字典类型
    /// <remarks>指定的字典对象：ParametersContainer</remarks>
    /// </summary>
    public class TransferAppApiJsonArgsToDictionary
    {

        /**
         * 将Dto传进来，对象属性都转成字典输出
         */
        public static ParametersContainer ToParametersContainer(Object obj)
        {
            ParametersContainer parametersContainer = new ParametersContainer();
            Type t = obj.GetType();

            PropertyInfo[] propertyArray = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in propertyArray)
            {
                //MethodInfo method = p.GetGetMethod();

                //if (method != null && method.IsPublic)
                //{
                //    parametersContainer.Add(p.Name, method.Invoke(obj, new Object[] {}));
                //}

                if (p.CanRead)
                {
                    TransferToDictionaryKeyAttribute attribute = 
                        p.GetCustomAttribute(typeof(TransferToDictionaryKeyAttribute)) as TransferToDictionaryKeyAttribute;

                    if (attribute == null)
                    {
                        parametersContainer.Add(p.Name, p.GetValue(obj));
                    }
                    else
                    {
                        parametersContainer.Add(attribute.Key, p.GetValue(obj));
                    }
                }


            }
            return parametersContainer;
        }
    }

    /// <summary>
    /// 此类用于将APP传的Json转换成Manager 查询方法统一使用的GetListDataArgs
    /// </summary>
    public class TransferAppGetListJsonArgsToGetListArgs
    {
           
        /**
         * 为保证此类只用于App端传递参数使用，故加上了AppGetListDataArgs的约束
         */
        public static GetListDataArgs ToGetListDataArgs<T>(T t) where T : AppGetListDataArgs
        {
            GetListDataArgs contractArgs = new GetListDataArgs();
            contractArgs.Parameters = TransferAppApiJsonArgsToDictionary.ToParametersContainer(t);

            PagingInfo pageInfo = new PagingInfo();
            pageInfo.CurrentPage = t.PageNo;
            pageInfo.PageSize = t.PageCount;

            contractArgs.OrderBy = t.OrderBy;
            contractArgs.PagingInfo = pageInfo;
             
            return contractArgs;
        }
    }
}
