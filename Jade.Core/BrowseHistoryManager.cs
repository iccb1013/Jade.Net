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
    public class BrowseHistoryManager
    {
        private static readonly BrowseHistoryManager _instance = new BrowseHistoryManager();
        public static BrowseHistoryManager Instance
        {
            get { return _instance; }
        }

        public NormalResult CreateBrowseHistory(Browse_History browseHistory)
        {
            using (Entities db = new Entities())
            {

                Member dbMember = db.Member.FirstOrDefault(t => t.id == browseHistory.member_id);

                if (dbMember != null)
                {
                    Product_Info dbProductInfo = db.Product_Info.FirstOrDefault(t => t.id == browseHistory.product_id);
                    if (dbProductInfo != null)
                    {
                        //更新商品访问次数+1
                        dbProductInfo.browse_count += 1;

                        //新增一条访问记录
                        db.Browse_History.Add(browseHistory);

                        #region 新增或更新当天商品浏览记录或次数
                        Product_Browse_Statistics dbProductBrowseStatistics = db.Product_Browse_Statistics.FirstOrDefault(
                            t =>
                                System.Data.Entity.DbFunctions.DiffDays(t.create_time, DateTime.Now) == 0 &&
                                t.product_id == browseHistory.product_id);

                        if (dbProductBrowseStatistics == null)
                        {
                            dbProductBrowseStatistics = new Product_Browse_Statistics();
                            dbProductBrowseStatistics.product_id = browseHistory.product_id;
                            dbProductBrowseStatistics.create_time = DateTime.Now;
                            dbProductBrowseStatistics.total_count = 1;

                            db.Product_Browse_Statistics.Add(dbProductBrowseStatistics);
                        }
                        else
                        {
                            dbProductBrowseStatistics.total_count += 1;
                        }
                        #endregion

                        #region 新增或更新当天用户浏览记录或次数
                        Member_Browse_Statistics dbMemberBrowseStatistics = db.Member_Browse_Statistics.FirstOrDefault(
                            t =>
                                System.Data.Entity.DbFunctions.DiffDays(t.create_time, DateTime.Now) == 0 &&
                                t.member_id == browseHistory.member_id);

                        if (dbMemberBrowseStatistics == null)
                        {
                            dbMemberBrowseStatistics = new Member_Browse_Statistics();
                            dbMemberBrowseStatistics.member_id = browseHistory.member_id;
                            dbMemberBrowseStatistics.create_time = DateTime.Now;
                            dbMemberBrowseStatistics.total_count = 1;

                            db.Member_Browse_Statistics.Add(dbMemberBrowseStatistics);
                        }
                        else
                        {
                            dbProductBrowseStatistics.total_count += 1;
                        }
                        #endregion
                    }

                    //更新会员总访问数
                    dbMember.product_browse_count += 1;

                    db.SaveChanges();
                }
            }
            return new NormalResult();
        }
    }
}
