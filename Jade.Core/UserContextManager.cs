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
using Sheng.Web.Infrastructure;
using Sheng.Kernal;
using Jade.Model;

namespace Jade.Core
{
    public class UserContextManager
    {
        private static readonly UserContextManager _instance = new UserContextManager();
        public static UserContextManager Instance
        {
            get { return _instance; }
        }

        private CachingService _cachingService = CachingService.Instance;

        private UserContextManager()
        {

        }

        public NormalResult<UserContext> Login(string account, string password)
        {
            if (String.IsNullOrEmpty(account) || String.IsNullOrEmpty(password))
            {
                return new NormalResult<UserContext>("用户名或密码无效。");
            }

            User userObj = null;
            using (Entities db = new Entities())
            {
                userObj = db.User.FirstOrDefault(e => e.account == account && e.password == password);

                if (userObj == null)
                {
                    return new NormalResult<UserContext>("用户名或密码无效。");
                }

                if(userObj.available == false)
                {
                    return new NormalResult<UserContext>("该用户账户已被删除。");
                }

                userObj.lastLoginTime = DateTime.Now;
                db.SaveChanges();
            }

            UserContext userContext = new UserContext()
            {
                LoginTime = DateTime.Now,
                Token = Guid.NewGuid().ToString(),
                UserId = userObj.id
            };
            NormalResult<UserContext> result = new NormalResult<UserContext>();
            result.Data = userContext;

            _cachingService.Set<UserContext>(userContext.Token, userContext);

            return result;
        }

        public void Logout(string token)
        {
            _cachingService.Remove(token);
        }

        public UserContext GetUserContext(string token)
        {
            UserContext userContext = _cachingService.Get<UserContext>(token);
            return userContext;
        }


    }
}
