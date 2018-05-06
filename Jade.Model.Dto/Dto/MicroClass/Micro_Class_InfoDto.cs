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
    public class Micro_Class_InfoDto
    {
        public int id { get; set; }
        public int group_id { get; set; }
        public string topic_info_title { get; set; }
        public string lecturer_text { get; set; }
        public string mp3_url { get; set; }
        public Nullable<int> mp3_duration { get; set; }
        public string img_url { get; set; }
        public System.DateTime create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
    }
}
