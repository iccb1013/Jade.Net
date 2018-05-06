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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto; 
using Sheng.Web.Infrastructure;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class ProductAttributeController : AppApiBaseController
    {   
        private static readonly ProductManager _productManager = ProductManager.Instance;
        // GET: AppApi/ProductAttribute
        public ActionResult QueryList()
        {
            AppGetProductAttributeListArgs args = RequestArgs<AppGetProductAttributeListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }
              
            List<Product_Attribute> productAttributes = _productManager.GetProductAttributeTreeByCategoryId(args.CategoryId);

            List<ProductAttributeAppDto> productAttributeList = Mapper.Map<List<ProductAttributeAppDto>>(productAttributes);
               
            return DataResult(productAttributeList);
        }
    }
}