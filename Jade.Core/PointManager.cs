/*
 
曹旭升（sheng.c）
E-mail: cao.silhouette@msn.com
QQ: 279060597
https://github.com/iccb1013
http://shengxunwei.com 
  
 */

using Jade.Model;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Data.Entity;
using Jade.Model.Dto;

namespace Jade.Core
{
    //申请状态  1:申请中  2：审核通过  3：已付款  4：已拒绝
    //已付款状态不实现，只做申请中，审核通过，拒绝


    public class PointManager
    {
        private static readonly PointManager _instance = new PointManager();
        public static PointManager Instance
        {
            get { return _instance; }
        }

        private PointManager()
        {

        }

        public Point_Exchange GetPointExchange(int id)
        {
            using (Entities db = new Entities())
            {
                Point_Exchange pointExchange = db.Point_Exchange
                    .Include(c => c.Member)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return pointExchange;
            }
        }

        public NormalResult CreatePointExchange(Point_Exchange pointExchange)
        {
            pointExchange.exchange_time = DateTime.Now;
            pointExchange.status = 1;

            using (Entities db = new Entities())
            {
                db.Point_Exchange.Add(pointExchange);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdatePointExchange(Point_Exchange pointExchange)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Point_Exchange> queryable = db.Point_Exchange;

                Point_Exchange dbPointExchange = queryable.FirstOrDefault(e => e.id == pointExchange.id);
                if (dbPointExchange == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(pointExchange, dbPointExchange);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void UpdatePointExchangeStatus(UpdatePointExchangeStatusArgs args)
        {
            using (Entities db = new Entities())
            {
                Point_Exchange pointExchange = db.Point_Exchange.FirstOrDefault(e => e.id == args.id);
                if (pointExchange != null)
                {
                    pointExchange.status = args.status;
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Point_Exchange> GetPointExchangeList(GetListDataArgs args)
        {
            GetListDataResult<Point_Exchange> result = new GetListDataResult<Point_Exchange>();
            using (Entities db = new Entities())
            {
                IQueryable<Point_Exchange> queryable = db.Point_Exchange
                    .Include(c => c.Member)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("member_id") == false)
                {
                    int memberId = args.Parameters.GetIntValue("member_id");
                    queryable = queryable.Where(c => c.member_id == memberId);
                }

                result.PagingInfo = args.PagingInfo;
                int totalCount = queryable.Count();
                result.PagingInfo.UpdateTotalCount(totalCount);

                result.Data = queryable.OrderBy(args.OrderBy)
                    .Skip((result.PagingInfo.CurrentPage - 1) * result.PagingInfo.PageSize)
                    .Take(result.PagingInfo.PageSize).ToList();
            }

            return result;
        }
        
        /// <summary>
        /// 根据用户编号 获取最后（最近）的一条记录
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool CheckExistNotFinishPointExchange(int memberId)
        {

            using (Entities db = new Entities())
            { 
                return db.Point_Exchange 
                     .Any(e => e.member_id == memberId && e.status !=3 && e.status !=4); 
            } 
        } 

    }
}
