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
    public class UserManager
    {
        private static readonly UserManager _instance = new UserManager();
        public static UserManager Instance
        {
            get { return _instance; }
        }

        private UserManager()
        {

        }

        public User GetUser(int id)
        {
            using (Entities db = new Entities())
            {
                User user = db.User
                    .Include(c => c.Role.Select(r => r.RoleAuthorization))
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return user;
            }
        }

        public NormalResult CreateUser(User user)
        {
            //user.Id = Guid.NewGuid();
            user.available = true;
            user.createdTime = DateTime.Now;
            user.password = IOHelper.GetMD5HashFromString(user.cellphone);

            using (Entities db = new Entities())
            {
                if (db.User.Any(s => s.account == user.account))
                {
                    return new NormalResult("指定的登录账户重复，已被其它用户占用。");
                }

                db.User.Add(user);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateUser(User user)
        {
            using (Entities db = new Entities())
            {
                if (db.User.Any(s => s.account == user.account && s.id != user.id))
                {
                    return new NormalResult("指定的登录账户重复，已被其它用户占用。");
                }

                IQueryable<User> queryable = db.User;

                User dbUser = queryable.FirstOrDefault(e => e.id == user.id);
                if (dbUser == null)
                    return new NormalResult("指定的数据不存在。");



                ShengMapper.SetValuesWithoutProperties(user, dbUser, new string[] { "password" }, true);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveUser(int id)
        {
            using (Entities db = new Entities())
            {
                User user = db.User.FirstOrDefault(e => e.id == id);
                if (user != null)
                {
                    user.available = false;
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<User> GetUserList(GetListDataArgs args)
        {
            GetListDataResult<User> result = new GetListDataResult<User>();
            using (Entities db = new Entities())
            {
                IQueryable<User> queryable = db.User
                    .Include(c=>c.Role)
                    .Where(c => c.available == true)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.account.Contains(keyword) || c.name.Contains(keyword) || c.cellphone.Contains(keyword));
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

        public NormalResult UpdatePassword(UpdatePasswordArgs args)
        {
            using (Entities db = new Entities())
            {
                IQueryable<User> queryable = db.User;

                User dbUser = queryable.FirstOrDefault(e => e.id == args.UserId && e.password == args.OldPassword);
                if (dbUser == null)
                    return new NormalResult("旧密码不正确。");

                dbUser.password = args.NewPassword;

                db.SaveChanges();
            }

            return new NormalResult();
        }

    }
}
