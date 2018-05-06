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
using System.Data.Entity.Validation;
using Jade.Model.Dto;

namespace Jade.Core
{
    public class MemberManager
    {
        private static readonly MemberManager _instance = new MemberManager();
        public static MemberManager Instance
        {
            get { return _instance; }
        }

        private object _lockObj = new object();

        private MemberManager()
        {

        }

        public Member GetMember(int id)
        {
            using (Entities db = new Entities())
            {
                Member member = db.Member
                    .Include(c=>c.Superior_Agent)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return member;
            }
        }

        public NormalResult CreateMember(Member member)
        {
            lock (_lockObj)
            {
                //member.Id = Guid.NewGuid();
                member.total_point = 0;
                member.reg_time = DateTime.Now;
                member.pic_path = "";
                if (String.IsNullOrEmpty(member.password))
                {
                    member.password = IOHelper.GetMD5HashFromString(member.phone_num);
                }

                using (Entities db = new Entities())
                {
                    if (db.Member.Any(s => s.phone_num == member.phone_num))
                    {
                        return new NormalResult("手机号码重复，已被其他会员占用。");
                    }

                    db.Member.Add(member);
                    db.SaveChanges();
                }
            }

            return new NormalResult();
        }

        public int GetMemberDistributionCount(int? memberId,int? superiorAgentId)
        {

            using (Entities db = new Entities())
            {
                if (superiorAgentId　!= null && memberId != null && superiorAgentId != 0)
                { 
                    return db.Member.Count(t => t.superior_agent_id == superiorAgentId && t.id == memberId.Value && t.status == 2);
                }
                if (superiorAgentId != null && superiorAgentId!=0)
                {
                    return db.Member.Count(t => t.superior_agent_id == superiorAgentId && t.status == 2);
                }
                if (memberId != null)
                {
                    return db.Member.Count(t => t.status == 2 && t.superior_agent_id == memberId);
                }
                return 0;
            }
        }

        public NormalResult UpdateMember(Member member)
        {
            if (member.pic_path == null)
                member.pic_path = "";

            using (Entities db = new Entities())
            {
                if (db.Member.Any(s => s.phone_num == member.phone_num && s.id != member.id))
                {
                    return new NormalResult("手机号码重复，已被其他会员占用。");
                }

                IQueryable<Member> queryable = db.Member;

                Member dbMember = queryable.FirstOrDefault(e => e.id == member.id);
                if (dbMember == null)
                    return new NormalResult("指定的数据不存在。");

                //不更新密码
                ShengMapper.SetValuesWithoutProperties(member, dbMember, new string[] { "password", "total_point", "total_amount" }, true);

                dbMember.update_time = DateTime.Now;

                db.SaveChanges();
            } 

            return new NormalResult();
        }

        public List<Member> GetDirectMemberList(int superiorAgentId)
        {
            using (Entities db = new Entities())
            {

                IQueryable<Member> queryable = db.Member 
                    .AsNoTracking();

                return queryable.Where(e => e.status == 2 && e.superior_agent_id == superiorAgentId).ToList(); 
            }
        }
        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        
        public List<Member> GetAllMemberList()
        {
            using (Entities db = new Entities())
            {

                IQueryable<Member> queryable = db.Member
                    .AsNoTracking();

                return queryable.ToList();
            }
        }

        public Member GetMemberByMobilePhone(string phoneNum)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Member> queryable = db.Member
                    .Include(c => c.Superior_Agent)
                    .AsNoTracking();
                Member dbMember = null;

                if (string.IsNullOrEmpty(phoneNum) == false)
                {
                    dbMember = queryable.FirstOrDefault(e => e.phone_num == phoneNum);
                }
                return dbMember;
            }
        }

        private Member GetMember(int id,string phoneNum,string password)
        {
            using (Entities db = new Entities())
            {

                IQueryable<Member> queryable = db.Member
                    .Include(c => c.Superior_Agent)
                    .AsNoTracking();
                Member dbMember = null;

                if (id != 0)
                {
                    dbMember = queryable.FirstOrDefault(e => e.id == id);
                }
                if (string.IsNullOrEmpty(phoneNum) == false && string.IsNullOrEmpty(password) == false)
                {
                    dbMember = queryable.FirstOrDefault(e => e.phone_num == phoneNum && e.password == password);
                } 
                return dbMember;
            }
        }

        public Member GetMemberByMobilePhoneAndPassword(string phoneNum,string password)
        { 
            return GetMember(0, phoneNum, password);
        }
        public NormalResult UpdateMemberPassword(int id, string newPassword)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Member> queryable = db.Member;

                Member dbMember = queryable.FirstOrDefault(e => e.id == id);
                if (dbMember == null)
                    return new NormalResult("指定的数据不存在。");

                dbMember.update_time = DateTime.Now;
                dbMember.password = newPassword;

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveMember(int id)
        {
            using (Entities db = new Entities())
            {
                Member member = db.Member.FirstOrDefault(e => e.id == id);
                if (member != null)
                {
                    //db.Member.Remove(member);

                    member.status = 5;
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Member> GetMemberList(GetListDataArgs args)
        {
            GetListDataResult<Member> result = new GetListDataResult<Member>();
            using (Entities db = new Entities())
            {
                IQueryable<Member> queryable = db.Member
                    .Include(c => c.Superior_Agent)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.name.Contains(keyword) || c.phone_num.Contains(keyword));
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

        public NormalResult ChangeStatus(ChangeMemberStatusArgs args)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Member> queryable = db.Member;

                Member dbMember = queryable.FirstOrDefault(e => e.id == args.MemberId);
                if (dbMember == null)
                    return new NormalResult("指定的数据不存在。");

                if (dbMember.status == args.Status)
                    return new NormalResult();

                dbMember.status = args.Status;


                switch (dbMember.status)
                {
                    case 2: //审核通过
                            //发短信
                        Yuntongxun.SendTemplateSMS(dbMember.phone_num, "202414", new string[] { });
                        break;
                    case 3: //审核不通过
                        //发短信
                        break;
                    default:
                        break;
                }

                dbMember.update_time = DateTime.Now;

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult ResetPassword(int id)
        {
            using (Entities db = new Entities())
            {
                Member member = db.Member.FirstOrDefault(e => e.id == id);
                if (member == null)
                {
                    return new NormalResult("指定的会员不存在。");
                }

                member.password = IOHelper.GetMD5HashFromString(member.phone_num);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public int GetMemberCount(int status)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Member> queryable = db.Member
                    .Where(c => c.status == status)
                    .AsNoTracking();

                return queryable.Count();
            }
        }

    }
}
