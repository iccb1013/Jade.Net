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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jade.Model.Dto.AppDto
{
    public class ProductOrderMasterAppDto
    {
        public int memberId { get; set; }
        public string id{ get; set; }
        public string ids { get; set; }
        public string personInfo { get; set; }
        public double sumPrice;
        public int totalRebateAmount { get; set; } //总返点
        public int superiorRebateAmount { get; set; } //上级返点
        public int superiorParentRebateAmount { get; set; } //上上级返点
        private string _receivingRealPrice;
        public string receivingRealPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_receivingRealPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_receivingRealPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _receivingRealPrice;
            }
            set { _receivingRealPrice = value; }
        }
        private string _realTotalPrice;
        public string realTotalPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_realTotalPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_realTotalPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _realTotalPrice;
            }
            set { _realTotalPrice = value; }
        }
        public int personIdNo{ get; set; }
        public string consignee{ get; set; }
        public string consigneeAddress{ get; set; }
        public string consigneePhone{ get; set; }
        private string _orderPrice;
        public string orderPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(_orderPrice))
                {
                    decimal nowPrice = Convert.ToDecimal(_orderPrice);
                    return nowPrice.ToString("#0.00");
                }
                return _orderPrice;
            }
            set { _orderPrice = value; }
        }
        public int productNum{ get; set; }
        public string currentStatus{ get; set; }
        public string transportNo{ get; set; }
        public string transportCompany{ get; set; }
        private string _orderDateTime;
        public string orderDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_orderDateTime) == false)
                {

                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_orderDateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _orderDateTime;
            }
            set { _orderDateTime = value; }
        }
        private string _paymentDateTime;
        public string paymentDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_paymentDateTime) == false)
                {

                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_paymentDateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _paymentDateTime;
            }
            set { _paymentDateTime = value; }
        }
        private string _refundDateTime;
        public string refundDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_refundDateTime) == false)
                { 
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_refundDateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _refundDateTime;
            }
            set { _refundDateTime = value; }
        }
        public object recordIds{ get; set; }
        public List<ProductOrderRecordAppDto> productOrderRecordList{ get; set; }
        public List<ProductOrderDetailAppDto> productOrderDetailList{ get; set; }
    }
}
