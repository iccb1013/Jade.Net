/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com r
*

*
********************************************************************/


using Jade.Model;
using Sheng.Web.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jade.Shell
{
    public class ShellUserContext : UserContext
    {
        private Hashtable _authorizationHashtable = new Hashtable();
        private List<string> _authorizationKeyList = new List<string>();
        
        public bool NotBelongToAnyRole
        {
            get;set;
        }

        private User _user;
        /// <summary>
        /// 本地用户模型
        /// </summary>
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;

                if(_user == null || _user.Role == null || _user.Role.Count == 0)
                {
                    NotBelongToAnyRole = true;
                }
                else
                {
                    NotBelongToAnyRole = false;
                }

                _authorizationHashtable.Clear();

                if (_user == null || _user.Role == null || _user.Role.Count == 0)
                    return;

                foreach (var Role in _user.Role)
                {
                    if (Role.RoleAuthorization == null || Role.RoleAuthorization.Count == 0)
                        continue;

                    foreach (var authorization in Role.RoleAuthorization)
                    {
                        if (_authorizationHashtable.ContainsKey(authorization.authorizationKey) == false)
                        {
                            _authorizationHashtable.Add(authorization.authorizationKey, null);
                        }

                        if (_authorizationKeyList.Contains(authorization.authorizationKey) == false)
                        {
                            _authorizationKeyList.Add(authorization.authorizationKey);
                        }
                    }
                }
            }
        }

        public List<string> AuthorizationKeyList
        {
            get
            {
                return _authorizationKeyList;
            }
        }

        /// <summary>
        /// 验证用户是否有指定的权限
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool AuthorizationKeyVerify(string key)
        {
            if (String.IsNullOrEmpty(key))
                return true;

            if (this.User.account == "admin")
                return true;

            //如果不属于任何角色，默认为 没有 任何限
            if (User.Role == null || User.Role.Count == 0)
                return false;

            return _authorizationHashtable.ContainsKey(key);
        }

     
        

    }
}