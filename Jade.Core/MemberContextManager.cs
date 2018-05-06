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
using Jade.Model.Dto.AppDto;

namespace Jade.Core
{
    public class MemberContextManager
    {
        private static readonly MemberContextManager _instance = new MemberContextManager();
        public static MemberContextManager Instance
        {
            get { return _instance; }
        }


        private CachingService _cachingService = CachingService.Instance;

        private MemberContextManager()
        {

        }

        public NormalResult<MemberUserContext> Login(MemberLoginArgs args)
        {
            if (String.IsNullOrEmpty(args.mobilephone) || String.IsNullOrEmpty(args.password))
            {
                return new NormalResult<MemberUserContext>("手机号或密码无效。");
            }

            Member member = null;
            using (Entities db = new Entities())
            {
                member = db.Member.FirstOrDefault(e => e.phone_num == args.mobilephone && e.password == args.password);

                if (member == null)
                {
                    return new NormalResult<MemberUserContext>("手机号或密码无效。");
                }
            }

            MemberUserContext userContext = new MemberUserContext()
            {
                LoginTime = DateTime.Now,
                Token = Guid.NewGuid().ToString(),
                UserId = member.id,
                //Member = member
            };
            NormalResult<MemberUserContext> result = new NormalResult<MemberUserContext>();
            result.Data = userContext;

            _cachingService.Set<MemberUserContext>(userContext.Token, userContext);

            return result;
        }
    }
}
