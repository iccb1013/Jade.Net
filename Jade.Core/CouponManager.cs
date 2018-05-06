/*
 
曹旭升（sheng.c）
E-mail: cao.silhouette@msn.com
QQ: 279060597
https://github.com/iccb1013
http://shengxunwei.com 
  
 */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Jade.Model;
using Jade.Model.Dto.AppDto;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using Jade.Model.Dto;

namespace Jade.Core
{
    public class CouponManager
    {
        private static readonly CouponManager _instance = new CouponManager(); 
        public static CouponManager Instance
        {
            get { return _instance; }
        }

        public Coupon_Info GetCoupon(int id)
        {
            using (Entities db = new Entities())
            {
                Coupon_Info coupon = db.Coupon_Info
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return coupon;
            }
        }

        public NormalResult CreateCoupon(Coupon_Info coupon)
        {
            //coupon.Id = Guid.NewGuid();

            coupon.create_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                if (db.Coupon_Info.Any(s => s.name == coupon.name))
                {
                    return new NormalResult("优惠券名称重复，已被其他优惠券占用。");
                }

                db.Coupon_Info.Add(coupon);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateCoupon(Coupon_Info coupon)
        {
            //coupon.modify_date_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                if (db.Coupon_Info.Any(s => s.name == coupon.name && s.id != coupon.id))
                {
                    return new NormalResult("优惠券名称重复，已被其他优惠券占用。");
                }

                IQueryable<Coupon_Info> queryable = db.Coupon_Info;

                Coupon_Info dbCoupon_Info = queryable.FirstOrDefault(e => e.id == coupon.id);
                if (dbCoupon_Info == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(coupon, dbCoupon_Info);
              
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveCoupon(int id)
        {
            using (Entities db = new Entities())
            {
                Coupon_Info coupon = db.Coupon_Info.FirstOrDefault(e => e.id == id);
                if (coupon != null)
                {
                    coupon.is_lose_valid = 1;

                    //db.Coupon_Info.Remove(coupon);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Coupon_Info> GetCouponList(GetListDataArgs args)
        {
            GetListDataResult<Coupon_Info> result = new GetListDataResult<Coupon_Info>();
            using (Entities db = new Entities())
            {
                IQueryable<Coupon_Info> queryable = db.Coupon_Info.Where(c=>c.is_lose_valid==0)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.name.Contains(keyword));
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
        /// 派发指定的卡券给所有会员
        /// </summary>
        /// <returns></returns>
        public NormalResult DistributeCouponToAllMember(DistributeCouponToAllMemberArgs args)
        {
            //临时方案，先把所有会员信息拉出来
            using (Entities db = new Entities())
            {
                Coupon_Info coupon = db.Coupon_Info.FirstOrDefault(c => c.id == args.CouponId);
                if (coupon == null)
                    return new NormalResult("指定的券不存在。");

                if (coupon.is_lose_valid == 1)
                    return new NormalResult("指定的券已失效。");

                if (coupon.end_time <= DateTime.Now)
                    return new NormalResult("指定的券已超过最后使用时限。");

                List<Member> memberList = db.Member.Where(c => c.status == 2).ToList();
                foreach (Member member in memberList)
                {
                    Member_Coupon_Account couponAccount = new Member_Coupon_Account();
                    couponAccount.member_id = member.id;
                    couponAccount.is_use = 0;
                    couponAccount.coupon_id = coupon.id;
                    couponAccount.create_time = DateTime.Now;

                    db.Member_Coupon_Account.Add(couponAccount);
                }

                db.SaveChanges();

                return new NormalResult();
            }
        }

        public NormalResult DistributeCouponToMember(DistributeCouponToMemberArgs args)
        {
            //临时方案，先把所有会员信息拉出来
            using (Entities db = new Entities())
            {
                Coupon_Info coupon = db.Coupon_Info.FirstOrDefault(c => c.id == args.CouponId);
                if (coupon == null)
                    return new NormalResult("指定的券不存在。");

                if (coupon.is_lose_valid == 1)
                    return new NormalResult("指定的券已失效。");

                if (coupon.end_time <= DateTime.Now)
                    return new NormalResult("指定的券已超过最后使用时限。");

                Member member = db.Member.FirstOrDefault(c => c.id == args.MemberId);

                if (member == null)
                    return new NormalResult("指定的会员不存在。");

                Member_Coupon_Account couponAccount = new Member_Coupon_Account();
                couponAccount.member_id = member.id;
                couponAccount.is_use = 0;
                couponAccount.coupon_id = coupon.id;
                couponAccount.create_time = DateTime.Now;

                db.Member_Coupon_Account.Add(couponAccount);

                db.SaveChanges();

                return new NormalResult();
            }
        }


        public GetListDataResult<Member_Coupon_Account> GetMemberCouponList(GetListDataArgs args)
        {
            GetListDataResult<Member_Coupon_Account> result = new GetListDataResult<Member_Coupon_Account>();

            using (Entities db = new Entities())
            {
                IQueryable<Member_Coupon_Account> queryable = db.Member_Coupon_Account
                    .Include(t=>t.Coupon_Info)
                    .Include(t=>t.Member)
                    .AsNoTracking(); 

                if (args.Parameters.IsNullOrEmpty("IsUse") == false)
                {
                    int isUse = args.Parameters.GetIntValue("IsUse");
                    queryable = queryable.Where(t => t.is_use == isUse);
                }

                if (args.Parameters.IsNullOrEmpty("member_id") == false)
                {
                    int member_id = args.Parameters.GetIntValue("member_id");
                    queryable = queryable.Where(t => t.member_id == member_id);
                } 
                result.PagingInfo = args.PagingInfo;
                int totalCount = queryable.Count();
                result.PagingInfo.UpdateTotalCount(totalCount);

                result.Data = queryable.OrderBy(args.OrderBy).Include(t=>t.Coupon_Info)
                    .Skip((result.PagingInfo.CurrentPage - 1) * result.PagingInfo.PageSize)
                    .Take(result.PagingInfo.PageSize).ToList();
            }

            return result;
        }
         
    }
}
