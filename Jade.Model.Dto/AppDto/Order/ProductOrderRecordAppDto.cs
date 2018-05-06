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
    public class ProductOrderRecordAppDto
    {
        public int id { get; set; }
        public string orderId { get; set; }
        public string modifyComment { get; set; }
        public int modifyUserId { get; set; }
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

        public string modifyUsername { get; set; }
    }
}
