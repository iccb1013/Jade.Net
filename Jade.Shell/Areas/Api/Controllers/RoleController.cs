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


using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class RoleController : ApiBaseController
    {
        private static readonly RoleManager _roleManager = RoleManager.Instance;

        #region Role

        public ActionResult GetRole(int id)
        {
            Role role = _roleManager.GetRole(id);
            if (role == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            RoleDto roleDto = Mapper.Map<RoleDto>(role);
            return DataResult(roleDto);
        }

        public ActionResult CreateRole()
        {
            RoleDto args = RequestArgs<RoleDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Role role = Mapper.Map<Role>(args);

            NormalResult result = _roleManager.CreateRole(role, args.roleAuthorization);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateRole()
        {
            RoleDto args = RequestArgs<RoleDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Role role = Mapper.Map<Role>(args);

            NormalResult result = _roleManager.UpdateRole(role, args.roleAuthorization);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveRole(int id)
        {
            _roleManager.RemoveRole(id);
            return SuccessfulResult();
        }

        public ActionResult GetRoleList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Role> roleList = _roleManager.GetRoleList(args);

            GetListDataResult<RoleInListDto> result = new GetListDataResult<RoleInListDto>();
            result.PagingInfo = roleList.PagingInfo;
            result.Data = Mapper.Map<List<Role>, List<Model.Dto.RoleInListDto>>(roleList.Data);

            return DataResult(result);
        }

        #endregion

        #region RoleUser

        public ActionResult AddUserListToRole()
        {
            AddUserListToRoleArgs args = RequestArgs<AddUserListToRoleArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            NormalResult result = _roleManager.AddUserListToRole(args);

            return ApiResult(result.Successful, result.Message);
        }


        public ActionResult RemoveUserFormRole()
        {
            int roleId = int.Parse(Request.QueryString["roleId"]);
            int userId = int.Parse(Request.QueryString["userId"]);

            _roleManager.RemoveUserFormRole(roleId, userId);
            return SuccessfulResult();
        }

        public ActionResult GetRoleUserList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<User> roleList = _roleManager.GetRoleUserList(args);

            GetListDataResult<UserInListDto> result = new GetListDataResult<UserInListDto>();
            result.PagingInfo = roleList.PagingInfo;
            result.Data = Mapper.Map<List<User>, List<UserInListDto>>(roleList.Data);

            return DataResult(result);
        }


        #endregion

        public ActionResult GetAuthorizationTree()
        {
            List<JsTreeNodeDto> result = new List<JsTreeNodeDto>();

            #region 系统管理

            JsTreeNodeDto l1_Settings = new JsTreeNodeDto();
            l1_Settings.Text = "系统管理";
            l1_Settings.Id = "L1_Settings";

            l1_Settings.Children.Add(new JsTreeNodeDto()
            {
                Text = "用户",
                Id = "User"
            });
            l1_Settings.Children.Add(new JsTreeNodeDto()
            {
                Text = "角色",
                Id = "Role"
            });
            result.Add(l1_Settings);

            #endregion

            #region 会员管理

            JsTreeNodeDto l1_Member = new JsTreeNodeDto();
            l1_Member.Text = "会员管理";
            l1_Member.Id = "L1_Member";

            l1_Member.Children.Add(new JsTreeNodeDto()
            {
                Text = "会员管理",
                Id = "Member"
            });
            l1_Member.Children.Add(new JsTreeNodeDto()
            {
                Text = "积分提取",
                Id = "PointExchange"
            });
            l1_Member.Children.Add(new JsTreeNodeDto()
            {
                Text = "消息推送",
                Id = "PushMessage"
            });
            result.Add(l1_Member);

            #endregion

            #region 商品管理

            JsTreeNodeDto l1_Product = new JsTreeNodeDto();
            l1_Product.Text = "商品管理";
            l1_Product.Id = "L1_Product";

            l1_Product.Children.Add(new JsTreeNodeDto()
            {
                Text = "订单管理",
                Id = "Order"
            });
            l1_Product.Children.Add(new JsTreeNodeDto()
            {
                Text = "商品管理",
                Id = "Product"
            });
            l1_Product.Children.Add(new JsTreeNodeDto()
            {
                Text = "分类管理",
                Id = "ProductCatalog"
            });
            l1_Product.Children.Add(new JsTreeNodeDto()
            {
                Text = "属性管理",
                Id = "ProductAttribute"
            });
            //l1_Product.Children.Add(new JsTreeNodeDto()
            //{
            //    Text = "防伪二维码",
            //    Id = "ProductQRCode"
            //});
            l1_Product.Children.Add(new JsTreeNodeDto()
            {
                Text = "优惠券管理",
                Id = "Coupon"
            });

            result.Add(l1_Product);

            #endregion

            #region 课程管理

            JsTreeNodeDto l1_MicroClass = new JsTreeNodeDto();
            l1_MicroClass.Text = "课程管理";
            l1_MicroClass.Id = "L1_MicroClass";

            l1_MicroClass.Children.Add(new JsTreeNodeDto()
            {
                Text = "课程管理",
                Id = "MicroClass"
            });
            result.Add(l1_MicroClass);

            #endregion

            return DataResult(result);

        }
    }
}