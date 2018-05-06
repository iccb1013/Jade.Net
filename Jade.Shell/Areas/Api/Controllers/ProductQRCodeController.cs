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
    public class ProductQRCodeController : ApiBaseController
    {
        private static readonly ProductQRCodeManager _productQRCodeManager = ProductQRCodeManager.Instance;

        #region ProductQRCode

        public ActionResult GetProductQRCode(int id)
        {
            Product_Anti_Fake productQRCode = _productQRCodeManager.GetProductQRCode(id);
            if (productQRCode == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Product_Anti_FakeDto productDto = Mapper.Map<Product_Anti_FakeDto>(productQRCode);
            return DataResult(productDto);
        }

        public ActionResult CreateProductQRCode()
        {
            Product_Anti_FakeDto args = RequestArgs<Product_Anti_FakeDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Anti_Fake productQRCode = Mapper.Map<Product_Anti_Fake>(args);

            NormalResult result = _productQRCodeManager.CreateProductQRCode(productQRCode);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateProductQRCode()
        {
            Product_Anti_FakeDto args = RequestArgs<Product_Anti_FakeDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Product_Anti_Fake productQRCode = Mapper.Map<Product_Anti_Fake>(args);

            NormalResult result = _productQRCodeManager.UpdateProductQRCode(productQRCode);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveProductQRCode(int id)
        {
            _productQRCodeManager.RemoveProductQRCode(id);
            return SuccessfulResult();
        }

        public ActionResult GetProductQRCodeList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Product_Anti_Fake> productList = _productQRCodeManager.GetProductQRCodeList(args);

            GetListDataResult<Product_Anti_FakeDto> result = new GetListDataResult<Product_Anti_FakeDto>();
            result.PagingInfo = productList.PagingInfo;
            result.Data = Mapper.Map<List<Product_Anti_Fake>, List<Product_Anti_FakeDto>>(productList.Data);

            return DataResult(result);
        }


        #endregion
    }
}