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
    public class AppGetMicroClassInfoListArgs : AppGetListDataArgs
    {
        public int GroupId { get; set; }
        public string TopicInfoTittle { get; set; }
    }
}
