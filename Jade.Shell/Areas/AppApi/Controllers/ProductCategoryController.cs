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
    public class ProductCategoryController : AppApiBaseController
    {
        private static readonly ProductManager _productManager = ProductManager.Instance;
        // GET: AppApi/AppProductCategory
        public ActionResult QueryList()
        {
            List<Product_Category> list = _productManager.GetProductCatalogTree();

            List<ProductCategoryAppDto> result = Mapper.Map<List<Product_Category>, List<ProductCategoryAppDto>>(list);

            return DataResult(result); 
        }
    }
}