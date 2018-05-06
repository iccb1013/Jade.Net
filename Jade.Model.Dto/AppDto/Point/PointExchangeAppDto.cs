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
using Sheng.Web.Infrastructure;

namespace Jade.Model.Dto.AppDto
{
    public class PointExchangeAppDto
    {
        public int Id { get; set; }

        [TransferToDictionaryKey("exchange_id")]
        public string exchangeId { get; set; }

        [TransferToDictionaryKey("exchange_name")]
        public string exchangeName { get; set; }

        public string amount { get; set; }

        public string status { get; set; }

        private string _exchangeTime;
        [TransferToDictionaryKey("exchange_time")]
        public string exchangeTime
        {
            get
            {
                if (string.IsNullOrEmpty(_exchangeTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_exchangeTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _exchangeTime;
            }
            set { _exchangeTime = value; }
        }

        public string remark { get; set; }
    }
}
