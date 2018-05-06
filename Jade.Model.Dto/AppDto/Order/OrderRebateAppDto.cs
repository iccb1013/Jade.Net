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
    public class OrderRebateAppDto
    { 
        public int id { get;set ;} 
        public int reciverId { get; set; }
        public int reciverRebate { get; set; }
        public int reciverLevel { get; set; }
        public int giverId { get; set; }
        public int giverLevel { get; set; }
        public int status { get; set; }
        public string giverName { get; set; }
        public string reason { get; set; }
        private string _rebateTime;
        public string rebateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_rebateTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_rebateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _rebateTime;
            }
            set { _rebateTime = value; }
        }
        private string _updateTime;
        public string updateTime
        {
            get
            {
                if (string.IsNullOrEmpty(_updateTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_updateTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _updateTime;
            }
            set { _updateTime = value; }
        }
    }
}
