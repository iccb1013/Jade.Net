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

namespace Jade.Model.Dto
{
    public class UserInListDto
    {
        public int id { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string cellphone { get; set; }
        public bool available { get; set; }
        public System.DateTime createdTime { get; set; }
        public Nullable<System.DateTime> lastLoginTime { get; set; }

        //////////////////


        public string roleNames { get; set; }
    }
}
