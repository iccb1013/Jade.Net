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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto;
using Newtonsoft.Json;
using Sheng.Web.Infrastructure;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    public class ProductInfoController : AppApiBaseController
    {
        private static readonly ProductManager _productManager = ProductManager.Instance;

        MatchEvaluator replaceRNLine = delegate (Match m)
        {
            return "\r\n";
        };

        // GET: AppApi/ProductInfo
        public ActionResult QueryPage()
        {
            AppGetProductInfoListArgs args = RequestArgs<AppGetProductInfoListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            } 
            args.OrderBy = "serial_no ASC";
            if (args.orderByType == 1)
            {
                args.OrderBy = "cost_price ASC";
            }
            if (args.orderByType == 2)
            {
                args.OrderBy = "cost_price DESC";
            }
            if (args.orderByType == 3)
            {
                args.OrderBy = "sales_num ASC";
            }
            if (args.orderByType == 4)
            {
                args.OrderBy = "sales_num DESC";
            }
            if (args.orderByType == 5)
            {
                args.OrderBy = "create_date_time DESC";
            }

            args.status = 1;

            args.memberType = UserContext.Member.type;
            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);

            contractArgs.Parameters.Add("stock", true); //只查有库存的

            GetListDataResult<Product_Info> productList = _productManager.GetProductList(contractArgs);

            AppGetListDataResult<ProductInfoAppDto> result = new AppGetListDataResult<ProductInfoAppDto>();

            result.pageNo       = productList.PagingInfo.CurrentPage;
            result.pageCount    = productList.PagingInfo.PageSize;
            result.totalPage    = productList.PagingInfo.TotalPage;
            result.totalRecord  = productList.PagingInfo.TotalCount;

            result.objectList = Mapper.Map<List<Product_Info>, List<ProductInfoAppDto>>(productList.Data); 
            TranscationAppNeedProductInfo(result, args);
            return DataResult(result); 
        }

        //会用到两个API
        public ActionResult QueryList()
        {
            AppGetProductInfoListArgs args = RequestArgs<AppGetProductInfoListArgs>();

            args.status = 1;
            args.OrderBy = "create_date_time ASC";

            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);

            contractArgs.Parameters.Add("stock", true); //只查有库存的

            GetListDataResult<Product_Info> productList = _productManager.GetProductList(contractArgs);

            AppGetListDataResult<ProductInfoAppDto> result = new AppGetListDataResult<ProductInfoAppDto>();

            result.pageNo = productList.PagingInfo.CurrentPage;
            result.pageCount = productList.PagingInfo.PageSize;
            result.totalPage = productList.PagingInfo.TotalPage;
            result.totalRecord = productList.PagingInfo.TotalCount;

            result.objectList = Mapper.Map<List<Product_Info>, List<ProductInfoAppDto>>(productList.Data);

            TranscationAppNeedProductInfo(result, args);
            
            return DataResult(result);
        }

        /// <summary>
        /// 翻译成App需要的商品信息
        /// </summary>
        /// <param name="result"></param>
        /// <param name="args"></param>
        private void TranscationAppNeedProductInfo(AppGetListDataResult<ProductInfoAppDto> result, AppGetProductInfoListArgs args)
        {
   
            foreach (ProductInfoAppDto productInfo in result.objectList)
            {
                string strSummary = Regex.Replace(productInfo.summary, @"\\r\\n", replaceRNLine);
                productInfo.summary = strSummary;// ;
                productInfo.productSummary = strSummary + "\n白度 " + getStarInfo(productInfo.white) + "\n油性 " +
                                             getStarInfo(productInfo.oily) + "\n细度 " + getStarInfo(productInfo.granularity) +
                                             "\n有无瑕疵 " + getStarInfo(productInfo.defect);
                productInfo.picturesList = new List<ProductPictureAppDto>();
                if (string.IsNullOrEmpty(productInfo.picUrl) == false)
                {
                    int i = 1;
                    foreach (string picUrl in JsonConvert.DeserializeObject<List<string>>(productInfo.picUrl))
                    {
                        i++;
                        var picture = new ProductPictureAppDto()
                        {
                            id = i.ToString(),
                            productId = productInfo.id,
                            picUrl = picUrl,
                            videoUrl = null,
                            serialNo = i.ToString()
                        };
                        productInfo.picturesList.Add(picture);
                    }
                }
                //设置视频地址
                if (productInfo.picturesList != null && productInfo.picturesList.Count > 0)
                {
                    productInfo.picturesList[0].videoUrl = productInfo.videoUrl;
                }
                int[] categoryIds= productInfo.productCategoryList.Select(t => t.id).ToArray();
                foreach (var categoryId in categoryIds)
                {
                    productInfo.productCategoryIds += categoryId + ",";
                }
                if (productInfo.productCategoryIds != null && productInfo.productCategoryIds.Length > 1)
                {
                    productInfo.productCategoryIds = productInfo.productCategoryIds.Substring(0,
                        productInfo.productCategoryIds.Length - 1);
                }

                productInfo.currentPrice = productInfo.memberPrice;
                if (args.memberType == 1)
                {
                    productInfo.currentPrice = productInfo.firstDistributionPrice;
                }
                else if (args.memberType == 2)
                {
                    productInfo.currentPrice = productInfo.secondDistributionPrice;
                    productInfo.firstDistributionPrice = "*****";
                }
                else if (args.memberType == 3)
                {
                    productInfo.currentPrice = productInfo.memberPrice;
                    productInfo.firstDistributionPrice = "*****";
                    productInfo.secondDistributionPrice = "*****";
                }
            }
        }
        private string getStarInfo(string starCount)
        {
            int nStarCount = int.Parse(starCount);
            string strStar = "";
            switch (nStarCount)
            {
                case 1:
                    strStar = "★☆☆☆☆";
                    break;
                case 2:
                    strStar = "★★☆☆☆";
                    break;
                case 3:
                    strStar = "★★★☆☆";
                    break;
                case 4:
                    strStar = "★★★★☆";
                    break;
                case 5:
                    strStar = "★★★★★";
                    break;
                default:
                    strStar = "☆☆☆☆☆";
                    break;
            }
            return strStar;
        }
    }
}