/*
 
曹旭升（sheng.c）
E-mail: cao.silhouette@msn.com
QQ: 279060597
https://github.com/iccb1013
http://shengxunwei.com 
  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jade.Model;
using Sheng.Web.Infrastructure;

namespace Jade.Core
{
    public class AppBrowseManager
    {
        private static readonly AppBrowseManager _instance = new AppBrowseManager();
        public static AppBrowseManager Instance
        {
            get { return _instance; }
        }

        public NormalResult CreateBrowse(App_Browse appBrowse)
        {
            using (Entities db = new Entities())
            { 

                Member dbMember = db.Member.FirstOrDefault(t => t.id == appBrowse.member_id);

                if (dbMember != null)
                {
                    //新增访问APP次数
                    db.App_Browse.Add(appBrowse);

                    //更新会员总访问数
                    dbMember.app_browse_count += 1;

                    db.SaveChanges();
                } 
            }
            return new NormalResult();
        }
    }
}
