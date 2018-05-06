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

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class ProductGroupController : AppApiBaseController
    {
        ProductManager _productManager = ProductManager.Instance;
        // GET: AppApi/ProductGroup
        public ActionResult QueryList()
        {
            List<Product_Group> list = _productManager.GetProductGroupList();

            List<ProductGroupAppDto> result = Mapper.Map<List<Product_Group>, List<ProductGroupAppDto>>(list);

            return DataResult(result);
        }
    }
}