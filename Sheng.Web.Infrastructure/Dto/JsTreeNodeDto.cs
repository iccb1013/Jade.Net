/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*

*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    [DataContract]
    public class JsTreeNodeDto
    {
        [DataMember(Name = "id")]
        public string Id
        {
            get; set;
        }

        [DataMember(Name = "text")]
        public string Text
        {
            get; set;
        }

        [DataMember(Name = "children")]
        public List<JsTreeNodeDto> Children
        {
            get;set;
        }

        [DataMember(Name = "state")]
        public JsTreeNodeStateDto State
        {
            get; set;
        }

        [DataMember(Name = "li_attr")]
        //[DataMember(Name = "a_attr")]
        public Dictionary<string, object> Attributes
        {
            get; set;
        }

        public JsTreeNodeDto()
        {
            State = new JsTreeNodeStateDto()
            {
                Opened = true
            };

            Children = new List<JsTreeNodeDto>();

            Attributes = new Dictionary<string, object>();
        }
    }
}
