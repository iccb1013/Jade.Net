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
using Jade.Model;
using Jade.Model.Dto;
using Newtonsoft.Json;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Jade.Model.Dto.AppDto;

namespace Jade.Shell
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            //http://www.cnblogs.com/1-2-3/p/AutoMapper-Best-Practice.html

            Mapper.Initialize(Configuration);

        }

        public static void Configuration(IMapperConfigurationExpression x)
        {
            x.CreateMap<UserContext, ShellUserContext>();

            #region User

            x.CreateMap<User, UserInListDto>()
                .ForMember(dto => dto.roleNames,
                (map) => map.MapFrom(user => user.Role != null ? String.Join("，", (from c in user.Role select c.name).ToArray()) : ""));

            x.CreateMap<User, UserDto>()
                .ForMember(dto => dto.roleNames,
                (map) => map.MapFrom(user => user.Role != null ? String.Join("，", (from c in user.Role select c.name).ToArray()) : ""))
                .ForMember(dto => dto.roles,
                (map) => map.MapFrom(user => user.Role != null ? (from c in user.Role select c.id).ToArray() : null))
                .ForMember(dto => dto.password,
                (map) => map.Ignore());

            x.CreateMap<UserDto, User>()
               .ForMember(user => user.Role, (map) => map.Ignore());

            #endregion

            #region Role

            x.CreateMap<Role, RoleDto>()
                 .ForMember(dto => dto.roleAuthorization,
                (map) => map.MapFrom(role => role.RoleAuthorization != null ?
                (from c in role.RoleAuthorization select c.authorizationKey).ToArray() : null));

            x.CreateMap<Role, RoleInListDto>();

            x.CreateMap<RoleDto, Role>()
                .ForMember(role => role.RoleAuthorization, (map) => map.Ignore());

            #endregion

            #region Member

            x.CreateMap<Member, MemberDto>()
                .ForMember(dto => dto.superior_agent_name, (map) => map.MapFrom(obj =>
                 obj.Superior_Agent != null ? obj.Superior_Agent.name : String.Empty))
                  .ForMember(dto => dto.superior_agent_phone_num, (map) => map.MapFrom(obj =>
                 obj.Superior_Agent != null ? obj.Superior_Agent.phone_num : String.Empty));

            x.CreateMap<MemberDto, Member>();

            #endregion

            #region Point_ExchangeDto

            x.CreateMap<Point_Exchange, Point_ExchangeDto>()
                .ForMember(dto => dto.member_name, (map) => map.MapFrom(obj =>
                 obj.Member != null ? obj.Member.name : String.Empty))
                  .ForMember(dto => dto.member_phone_num, (map) => map.MapFrom(obj =>
                 obj.Member != null ? obj.Member.phone_num : String.Empty));

            x.CreateMap<Point_ExchangeDto, Point_Exchange>();

            #endregion

            #region Product

            x.CreateMap<Product_Info, Product_InfoDto>()
            .ForMember(dto => dto.summary, (map) => map.MapFrom(obj => 
            String.IsNullOrEmpty(obj.summary)?String.Empty:obj.summary.Replace(@"\r\n","\n")))
            .ForMember(dto => dto.pic_url,
                 (map) => map.MapFrom(obj => String.IsNullOrEmpty(obj.pic_url) == false
                 ? JsonConvert.DeserializeObject<List<string>>(obj.pic_url) : null))
                 .ForMember(dto => dto.catalog_idList, (map) => map.MapFrom(obj =>
                 obj.Product_Category != null ? obj.Product_Category.Select(c => c.id).ToArray() : null))
                 .ForMember(dto => dto.catalog_nameList, (map) => map.MapFrom(obj =>
                 obj.Product_Category != null ? obj.Product_Category.Select(c => c.category_name).ToArray() : null))
                  .ForMember(dto => dto.attribute_idList, (map) => map.MapFrom(obj =>
                 obj.Product_Attribute != null ? obj.Product_Attribute.Select(c => c.id).ToArray() : null))
                 .ForMember(dto => dto.attribute_nameList, (map) => map.MapFrom(obj =>
                 obj.Product_Attribute != null ? obj.Product_Attribute.Select(c => c.attribute_name).ToArray() : null));

            x.CreateMap<Product_InfoDto, Product_Info>()
                .ForMember(obj => obj.pic_url,
                (map) => map.MapFrom(dto => dto.pic_url != null && dto.pic_url.Count > 0
                ? JsonConvert.SerializeObject(dto.pic_url) : ""));

            x.CreateMap<Product_Category, Product_CategoryDto>();

            x.CreateMap<Product_CategoryDto, Product_Category>();

            x.CreateMap<Product_Category, JsTreeNodeDto>()
                .ForMember(dto => dto.Id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.Text, (map) => map.MapFrom(obj => obj.category_name))
                .ForMember(dto => dto.Children, (map) => map.MapFrom(obj => obj.Product_CategoryChild));

            x.CreateMap<Product_Attribute, Product_AttributeDto>();

            x.CreateMap<Product_AttributeDto, Product_Attribute>();

            x.CreateMap<Product_Attribute, JsTreeNodeDto>()
                .ForMember(dto => dto.Id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.Text, (map) => map.MapFrom(obj => obj.attribute_name))
                .ForMember(dto => dto.Children, (map) => map.MapFrom(obj => obj.Product_AttributeChild));


            #endregion

            #region ProductQRCode

            x.CreateMap<Product_Anti_Fake, Product_Anti_FakeDto>();

            x.CreateMap<Product_Anti_FakeDto, Product_Anti_Fake>();


            #endregion

            #region Order

            x.CreateMap<Product_Order_Master, Product_Order_MasterDto>()
                 .ForMember(dto => dto.member_name, (map) => map.MapFrom(obj => obj.Member.name))
                 .ForMember(dto => dto.member_phone_num, (map) => map.MapFrom(obj => obj.Member.phone_num));

            x.CreateMap<Product_Order_Master, Product_Order_MasterInListDto>();

            x.CreateMap<Product_Order_MasterDto, Product_Order_Master>();

            x.CreateMap<Product_Order_Detail, Product_Order_DetailDto>();

            x.CreateMap<Product_Order_DetailDto, Product_Order_Detail>();

            x.CreateMap<Product_Order_Record, Product_Order_RecordDto>()
                .ForMember(dto => dto.modify_user_name, (map) => map.MapFrom(obj => obj.Modify_User.name));

            x.CreateMap<Product_Order_RecordDto, Product_Order_Record>();

            #endregion

            #region Coupon

            x.CreateMap<Coupon_Info, Coupon_InfoDto>();

            x.CreateMap<Coupon_InfoDto, Coupon_Info>();

            #endregion

            #region MicroClass

            x.CreateMap<Micro_Class_Group, Micro_Class_GroupDto>();

            x.CreateMap<Micro_Class_GroupDto, Micro_Class_Group>();

            x.CreateMap<Micro_Class_Info, Micro_Class_InfoDto>();

            x.CreateMap<Micro_Class_InfoDto, Micro_Class_Info>();


            #endregion

            #region PushMessage

            x.CreateMap<Push_Message, Push_MessageDto>();

            x.CreateMap<Push_Message, Push_MessageInListDto>();

            x.CreateMap<Push_MessageDto, Push_Message>();

            x.CreateMap<Push_History, Push_HistoryDto>()
                .ForMember(dto => dto.user_name, (map) => map.MapFrom(obj =>
                 obj.User != null ? obj.User.name : String.Empty));

            x.CreateMap<Push_History, Push_HistoryInListDto>()
                .ForMember(dto => dto.user_name, (map) => map.MapFrom(obj =>
                 obj.User != null ? obj.User.name : String.Empty));

            x.CreateMap<Push_HistoryDto, Push_History>();

            #endregion

            //App 用的 AutoMapper 配置在 AppAutoMapperConfig.cs
        }
    }
}