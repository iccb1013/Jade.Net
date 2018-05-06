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


using Jade.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq ; 
using System.Text ; 
using System.Threading.Tasks ; 

namespace Jade.Model.Dto.AppDto
{
    public class ProductInfoAppDto
    {

        public string id { get; set; } 
        public int categoryId { get; set; } 
        public string productCode { get; set; } 
        public string productName { get; set; }  
        public string picUrl { get; set; }
        public string videoUrl { get; set; }
        public string thumbPic { get; set; }

        //public string realPrice { get; set; }
        private string _realPrice;
        public string realPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_realPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_realPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _realPrice;
            }
            set { _realPrice = value; }
        }

        //public string currentPrice { get; set; }
        private string _currentPrice;
        public string currentPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_currentPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _currentPrice;
            }
            set { _currentPrice = value; }
        }
        public string summary { get; set; } 
        public string productSummary { get; set; }
        public string descUrl { get; set; }

        //public string costPrice { get; set; }
        private string _costPrice;
        public string costPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_costPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_costPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _costPrice;
            }
            set { _costPrice = value; }
        }
        //public string firstDistributionPrice { get; set; }
        private string _firstDistributionPrice;
        public string firstDistributionPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_firstDistributionPrice))
                {
                    if (_firstDistributionPrice.Equals("*****"))
                    {
                        return _firstDistributionPrice;
                    }
                    decimal nowPrice = Convert.ToDecimal(_firstDistributionPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _firstDistributionPrice;
            }
            set { _firstDistributionPrice = value; }
        }
        //public string secondDistributionPrice { get; set; }
        private string _secondDistributionPrice;
        public string secondDistributionPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_secondDistributionPrice))
                {
                    if (_secondDistributionPrice.Equals("*****"))
                    {
                        return _secondDistributionPrice;
                    }
                    decimal nowPrice = Convert.ToDecimal(_secondDistributionPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _secondDistributionPrice;
            }
            set { _secondDistributionPrice = value; }
        }
        //public string memberPrice { get; set; }
        private string _memberPrice;
        public string memberPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_memberPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_memberPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _memberPrice;
            }
            set { _memberPrice = value; }
        }
        public string salesNum { get; set; } 
        public string stock { get; set; } 
        public string status { get; set; } 
        public string serialNo { get; set; }
        private string _createDateTime;
        public string createDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_createDateTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_createDateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _createDateTime;
            }
            set { _createDateTime = value; }
        }
        private string _modifyDateTime;
        public string modifyDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_modifyDateTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_modifyDateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _modifyDateTime;
            }
            set { _modifyDateTime = value; }
        }
        public string modifyPerson { get; set; } 
        public string productCategoryIds { get; set; }
        public List<string> currentProductImgList { get; set; }
        public List<long> selectArray { get; set; }
        public string mp4Url { get; set; }
        public  int browseCount { get; set; }
        public List<ProductAttributeSimpleAppDto> attributeList { get; set; }
        public ProductCategorySimpleAppDto productCategory { get; set; }
        public List<ProductCategorySimpleAppDto> productCategoryList { get; set; } 
        public List<ProductPictureAppDto> picturesList { get; set; } 
        //白度
        public string white { get; set; } 
        //粒度（细度）
        public string granularity { get; set; } 
        //瑕疵
        public string defect { get; set; } 
        //油性
        public string oily { get; set; } 
    }
}
