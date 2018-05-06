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
    public class MemberCouponAccountAppDto
    {
        public int id { get; set; }
        public int personIdNo { get; set; }
        public int isUse { get; set; }
        private string _userTime;
        public string useTime
        {
            get
            {
                if (string.IsNullOrEmpty(_userTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); 
                    DateTime thisDate = Convert.ToDateTime(_userTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                     
                }
                return _userTime;
            }
            set { _userTime = value; }
        }
        public int couponId { get; set; }
        private string _createTime;
        public string createTime
        {
            get
            {
                if (string.IsNullOrEmpty(_createTime) == false)
                { 
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_createTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _createTime;
            }
            set { _createTime = value; }
        }
        private string _serverTime;
        public string serverTime
        {
            get
            {
                if (string.IsNullOrEmpty(_serverTime) == false)
                {

                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_serverTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _serverTime;
            }
            set { _serverTime = value; }
        }
        public string couponName{ get; set; }
        public string personName{ get; set; }
        public string discount{ get; set; }
        private string _startTime;
        public string startTime
        {
            get
            {
                if (string.IsNullOrEmpty(_startTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_startTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _startTime;
            }
            set { _startTime = value; }
        }
        private string _endTime;
        public string endTime
        {
            get
            {
                if (string.IsNullOrEmpty(_endTime) == false)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime thisDate = Convert.ToDateTime(_endTime);

                    return (thisDate - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                }
                return _endTime;
            }
            set { _endTime = value; }
        }
        public int isLoseValid{ get; set; }
    }
}
