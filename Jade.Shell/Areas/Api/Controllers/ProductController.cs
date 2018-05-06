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
using Jade.Model.Dto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class ProductController : ApiBaseController
    {
        private static readonly ProductManager _productManager = ProductManager.Instance;

        #region Product

        public ActionResult GetProduct(int id)
        {
            Product_Info product = _productManager.GetProduct(id);
            if (product == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Product_InfoDto productDto = Mapper.Map<Product_InfoDto>(product);
            return DataResult(productDto);
        }

        public ActionResult CreateProduct()
        {
            Product_InfoDto args = RequestArgs<Product_InfoDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Info product = Mapper.Map<Product_Info>(args);

            NormalResult result = _productManager.CreateProduct(product, args.catalog_idList, args.attribute_idList);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateProduct()
        {
            Product_InfoDto args = RequestArgs<Product_InfoDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            args.summary = args.summary.Replace("\n", @"\r\n");

            Product_Info product = Mapper.Map<Product_Info>(args);

            NormalResult result = _productManager.UpdateProduct(product, args.catalog_idList, args.attribute_idList);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveProduct(int id)
        {
            _productManager.RemoveProduct(id);
            return SuccessfulResult();
        }

        public ActionResult GetProductList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Product_Info> productList = _productManager.GetProductList(args);

            GetListDataResult<Product_InfoDto> result = new GetListDataResult<Product_InfoDto>();
            result.PagingInfo = productList.PagingInfo;
            result.Data = Mapper.Map<List<Product_Info>, List<Product_InfoDto>>(productList.Data);

            return DataResult(result);
        }

        #endregion

        #region ProductCatalog

        public ActionResult GetProductCatalog(int id)
        {
            Product_Category productCatalog = _productManager.GetProductCatalog(id);
            if (productCatalog == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Product_CategoryDto productCatalogDto = Mapper.Map<Product_CategoryDto>(productCatalog);
            return DataResult(productCatalogDto);
        }

        public ActionResult CreateProductCatalog()
        {
            Product_CategoryDto args = RequestArgs<Product_CategoryDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Category productCatalog = Mapper.Map<Product_Category>(args);

            NormalResult result = _productManager.CreateProductCatalog(productCatalog);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateProductCatalog()
        {
            Product_CategoryDto args = RequestArgs<Product_CategoryDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Category productCatalog = Mapper.Map<Product_Category>(args);

            NormalResult result = _productManager.UpdateProductCatalog(productCatalog);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveProductCatalog(int id)
        {
            _productManager.RemoveProductCatalog(id);
            return SuccessfulResult();
        }

        public ActionResult GetProductCatalogList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Product_Category> productCatalogList = _productManager.GetProductCatalogList(args);

            GetListDataResult<Product_CategoryDto> result = new GetListDataResult<Product_CategoryDto>();
            result.PagingInfo = productCatalogList.PagingInfo;
            result.Data = Mapper.Map<List<Product_Category>, List<Product_CategoryDto>>(productCatalogList.Data);

            return DataResult(result);
        }

        public ActionResult GetProductCatalogTree()
        {
            //商品固定的最多三层，品类 Category、类型 Type 、品种 Kind
            List<Product_Category> list = _productManager.GetProductCatalogTree();
            List<JsTreeNodeDto> result = Mapper.Map<List<Product_Category>, List<JsTreeNodeDto>>(list);

            foreach (var categoryItem in result)
            {
                categoryItem.State.Opened = false;
                categoryItem.Attributes.Add("level", "1");
                categoryItem.Attributes.Add("text", categoryItem.Text);

                foreach (var typeItem in categoryItem.Children)
                {
                    typeItem.Attributes.Add("level", "2");
                    typeItem.Attributes.Add("text", typeItem.Text);
                }
            }

            return DataResult(result);
        }

        #endregion

        #region ProductAttribute

        public ActionResult GetProductAttribute(int id)
        {
            Product_Attribute productAttribute = _productManager.GetProductAttribute(id);
            if (productAttribute == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Product_AttributeDto productAttributeDto = Mapper.Map<Product_AttributeDto>(productAttribute);
            return DataResult(productAttributeDto);
        }

        public ActionResult CreateProductAttribute()
        {
            Product_AttributeDto args = RequestArgs<Product_AttributeDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Attribute productAttribute = Mapper.Map<Product_Attribute>(args);

            NormalResult result = _productManager.CreateProductAttribute(productAttribute);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateProductAttribute()
        {
            Product_AttributeDto args = RequestArgs<Product_AttributeDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Attribute productAttribute = Mapper.Map<Product_Attribute>(args);

            NormalResult result = _productManager.UpdateProductAttribute(productAttribute);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveProductAttribute(int id)
        {
            _productManager.RemoveProductAttribute(id);
            return SuccessfulResult();
        }

        public ActionResult GetProductAttributeList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Product_Attribute> productAttributeList = _productManager.GetProductAttributeList(args);

            GetListDataResult<Product_AttributeDto> result = new GetListDataResult<Product_AttributeDto>();
            result.PagingInfo = productAttributeList.PagingInfo;
            result.Data = Mapper.Map<List<Product_Attribute>, List<Product_AttributeDto>>(productAttributeList.Data);

            return DataResult(result);
        }

        public ActionResult GetProductAttributeTree()
        {
            //商品固定的最多三层，品类 Attribute、类型 Type 、品种 Kind
            List<Product_Attribute> list = _productManager.GetProductAttributeTree();
            List<JsTreeNodeDto> result = Mapper.Map<List<Product_Attribute>, List<JsTreeNodeDto>>(list);

            foreach (var categoryItem in result)
            {
                categoryItem.State.Opened = false;
                categoryItem.Attributes.Add("level", "1");
                categoryItem.Attributes.Add("text", categoryItem.Text);

                foreach (var typeItem in categoryItem.Children)
                {
                    typeItem.Attributes.Add("level", "2");
                    typeItem.Attributes.Add("text", typeItem.Text);
                }
            }

            return DataResult(result);
        }

        #endregion
    }
}