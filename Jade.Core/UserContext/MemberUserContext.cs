/*
 
曹旭升（sheng.c）
E-mail: cao.silhouette@msn.com
QQ: 279060597
https://github.com/iccb1013
http://shengxunwei.com 
  
 */

using Jade.Model;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jade.Model.Dto.AppDto;

namespace Jade.Core
{
    public class MemberUserContext: UserContext
    {
        public MemberAppDto Member
        {
            get;set;
        }
    }
}
