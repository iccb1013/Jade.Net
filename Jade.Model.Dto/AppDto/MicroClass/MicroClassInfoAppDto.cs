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
    public class MicroClassInfoAppDto
    {
        public int id { get; set; }
        public int groupId { get; set; }
        public string topicInfoTitle { get; set; }
        public string lecturerText { get; set; }
        public string mp3Url { get; set; }
        public int mp3Duration { get; set; } = 0;
        public string imgUrl { get; set; } 
        public string createTime { get; set; } 
        public string updateTime { get; set; }
        public bool adminSys { get; set; } = true;
        public object objList { get; set; } = null;
    }
}
