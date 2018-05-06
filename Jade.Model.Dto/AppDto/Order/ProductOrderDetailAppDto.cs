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
using System.Text;
using System.Threading.Tasks;
using Sheng.Web.Infrastructure;

namespace Jade.Model.Dto.AppDto
{
    public class ProductOrderDetailAppDto
    {
        public string Id{ get; set; }
        public string orderIdNo{ get; set; }
        public string certSign { get; set; }
        public int categoryId{ get; set; } 
        public int productId { get; set; }
        public string productCode{ get; set; }
        public string productName{ get; set; } 
        public string thumbPic{ get; set; }
        public int buyNum{ get; set; }
        public string summary{ get; set; }
        public string descUrl{ get; set; }
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
        private string _firstDistributionPrice;
        public string firstDistributionPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_firstDistributionPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_firstDistributionPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _firstDistributionPrice;
            }
            set { _firstDistributionPrice = value; }
        }
        private string _secondDistributionPrice;
        public string secondDistributionPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_secondDistributionPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_secondDistributionPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _secondDistributionPrice;
            }
            set { _secondDistributionPrice = value; }
        }
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
        public int isReceiving { get; set; }
        public string couponAccountId{ get; set; }    
        public string couponName{ get; set; }
        private string _couponSalePrice;
        public string couponSalePrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_couponSalePrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_couponSalePrice);
                    return nowPrice.ToString("#0.00");
                }
                return _couponSalePrice;
            }
            set { _couponSalePrice = value; }
        }
        public string couponId{ get; set; }

    }
}
