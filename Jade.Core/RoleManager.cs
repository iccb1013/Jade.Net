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
    public class RoleManager
    {
        private static readonly RoleManager _instance = new RoleManager();
        public static RoleManager Instance
        {
            get { return _instance; }
        }

        private RoleManager()
        {

        }

        #region Role

        public Role GetRole(int id)
        {
            using (Entities db = new Entities())
            {
                Role role = db.Role.Include(c => c.RoleAuthorization)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return role;
            }
        }

        public NormalResult CreateRole(Role role, List<string> roleAuthorization)
        {
            if (role == null)
                return new NormalResult("参数错误。");

            //role.Id = Guid.NewGuid();

            using (Entities db = new Entities())
            {
                if (db.Role.Any(s => s.name == role.name))
                {
                    return new NormalResult("指定的角色名称重复，已被其它角色占用。");
                }

                role.RoleAuthorization.Clear();

                foreach (var item in roleAuthorization)
                {
                    role.RoleAuthorization.Add(new RoleAuthorization()
                    {
                        roleId = role.id,
                        authorizationKey = item
                    });
                }

                db.Role.Add(role);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateRole(Role role, List<string> roleAuthorization)
        {
            if (role == null)
                return new NormalResult("参数错误。");

            using (Entities db = new Entities())
            {
                if (db.Role.Any(s => s.name == role.name && s.id != role.id))
                {
                    return new NormalResult("指定的角色名称重复，已被其它角色占用。");
                }

                Role dbRole = db.Role.FirstOrDefault(s => s.id == role.id);
                if (dbRole == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(role, dbRole);

                dbRole.RoleAuthorization.Clear();

                foreach (var item in roleAuthorization)
                {
                    dbRole.RoleAuthorization.Add(new RoleAuthorization()
                    {
                        roleId = dbRole.id,
                        authorizationKey = item
                    });
                }

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveRole(int id)
        {
            using (Entities db = new Entities())
            {
                Role role = db.Role.FirstOrDefault(e => e.id == id);
                if (role != null)
                {
                    db.Role.Remove(role);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Role> GetRoleList(GetListDataArgs args)
        {
            GetListDataResult<Role> result = new GetListDataResult<Role>();
            using (Entities db = new Entities())
            {
                IQueryable<Role> queryable = db.Role.AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string name = args.Parameters.GetValue("Keyword");
                    queryable = queryable.Where(c => c.name.Contains(name));
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

        #endregion

        #region RoleUser

        public NormalResult AddUserListToRole(AddUserListToRoleArgs args)
        {
            using (Entities db = new Entities())
            {
                Role role = db.Role.FirstOrDefault(r => r.id == args.RoleId);

                if (role == null)
                    return new NormalResult("指定的角色不存在。");

                foreach (var userId in args.UserIdArray)
                {
                    User user = role.User.FirstOrDefault(u => u.id == userId);
                    if (user != null)
                        continue;

                    user = db.User.FirstOrDefault(u => u.id == userId);
                    if (user == null)
                        return new NormalResult("指定的用户不存在。");

                    role.User.Add(user);
                }

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveUserFormRole(int roleId, int userId)
        {
            using (Entities db = new Entities())
            {
                Role role = db.Role.FirstOrDefault(r => r.id == roleId);
                User user = role.User.FirstOrDefault(u => u.id == userId);
                if (user != null)
                {
                    role.User.Remove(user);
                }
                db.SaveChanges();
            }
        }

        public GetListDataResult<User> GetRoleUserList(GetListDataArgs args)
        {
            GetListDataResult<User> result = new GetListDataResult<User>();
            using (Entities db = new Entities())
            {

                int roleId = args.Parameters.GetIntValue("roleId");
                Role role = db.Role.FirstOrDefault(r => r.id == roleId);

                IQueryable<User> userQueryable = db.Entry(role).Collection(r => r.User).Query();

                result.PagingInfo = args.PagingInfo;
                int totalCount = userQueryable.Count();
                result.PagingInfo.UpdateTotalCount(totalCount);

                result.Data = userQueryable.Include(u => u.Role).AsNoTracking().OrderBy(args.OrderBy)
                    .Skip((result.PagingInfo.CurrentPage - 1) * result.PagingInfo.PageSize)
                    .Take(result.PagingInfo.PageSize).ToList();
            }

            return result;
        }

        #endregion
    }
}
