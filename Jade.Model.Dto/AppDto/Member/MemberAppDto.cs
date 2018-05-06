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
    public class MemberAppDto
    {
        public int id { get; set; }
        public string username { get; set; } 
        public string password { get; set; }
        public string personName { get; set; }
        public string sex { get; set; }
        public string dateOfBirth { get; set; }
        public string picPath { get; set; }
        public string name { get; set; }
        public string phoneNum { get; set; }
        public string wechatNo { get; set; }
        public int type { get; set; }
        public string totalAmount { get; set; }
        public string superiorAgentId { get; set; }
        public string cardHolder { get; set; }
        public string cardNo { get; set; }
        public string depositBank { get; set; }
        public string depositBranchBank { get; set; }
        public string regTime { get; set; }
        public string totalPoint { get; set; }
        private string _updateTime;
        public string updateTime {
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
            set
            {
                _updateTime = value;
            }
        }
        public string secretStatus { get; set; }
        public string status { get; set; }
        public int primaryDistributionCount { get; set; }
        public int secondDistributionCount { get; set; }
        public bool adminSys { get; set; } = true;
    }
}
