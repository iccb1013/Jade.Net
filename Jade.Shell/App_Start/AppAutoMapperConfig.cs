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
    public static class AppAutoMapperConfig
    {
        public static void Initialize()
        {
            //http://www.cnblogs.com/1-2-3/p/AutoMapper-Best-Practice.html

            Mapper.Initialize(Configuration);

        }

        public static void Configuration(IMapperConfigurationExpression x)
        {
            #region Member

            x.CreateMap<Member, MemberAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.username, (map) => map.MapFrom(obj => obj.name))
                //.ForMember(dto => dto.password, (map) => map.MapFrom(obj => obj.password))
                .ForMember(dto => dto.personName, (map) => map.MapFrom(obj => obj.name))  
                .ForMember(dto => dto.picPath, (map) => map.MapFrom(obj => obj.pic_path)) 
                .ForMember(dto => dto.type, (map) => map.MapFrom(obj => obj.type))
                .ForMember(dto => dto.totalAmount, (map) => map.MapFrom(obj => obj.total_amount))
                .ForMember(dto => dto.superiorAgentId, (map) => map.MapFrom(obj => obj.superior_agent_id))
                .ForMember(dto => dto.cardHolder, (map) => map.MapFrom(obj => obj.card_holder))
                .ForMember(dto => dto.cardNo, (map) => map.MapFrom(obj => obj.card_no))
                .ForMember(dto => dto.depositBank, (map) => map.MapFrom(obj => obj.deposit_bank))
                .ForMember(dto => dto.depositBranchBank, (map) => map.MapFrom(obj => obj.deposit_branch_bank))
                .ForMember(dto => dto.regTime, (map) => map.MapFrom(obj => obj.reg_time))
                .ForMember(dto => dto.totalPoint, (map) => map.MapFrom(obj => obj.total_point))
                .ForMember(dto => dto.updateTime, (map) => map.MapFrom(obj => obj.update_time==null?null: obj.update_time.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.secretStatus, (map) => map.MapFrom(obj => obj.secret_status)) 
                .ForMember(dto => dto.status, (map) => map.MapFrom(obj => obj.status));

            x.CreateMap<MemberAppDto, Member>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.name, (map) => map.MapFrom(obj => obj.username))
                //.ForMember(dto => dto.password, (map) => map.MapFrom(obj => obj.password))
                .ForMember(dto => dto.name, (map) => map.MapFrom(obj => obj.personName))
                .ForMember(dto => dto.pic_path, (map) => map.MapFrom(obj => obj.picPath)) 
                .ForMember(dto => dto.type, (map) => map.MapFrom(obj => obj.type))
                .ForMember(dto => dto.total_amount, (map) => map.MapFrom(obj => obj.totalAmount))
                .ForMember(dto => dto.superior_agent_id, (map) => map.MapFrom(obj => obj.superiorAgentId))
                .ForMember(dto => dto.card_holder, (map) => map.MapFrom(obj => obj.cardHolder))
                .ForMember(dto => dto.card_no, (map) => map.MapFrom(obj => obj.cardNo))
                .ForMember(dto => dto.deposit_bank, (map) => map.MapFrom(obj => obj.depositBank))
                .ForMember(dto => dto.deposit_branch_bank, (map) => map.MapFrom(obj => obj.depositBranchBank))
                .ForMember(dto => dto.reg_time, (map) => map.MapFrom(obj => obj.regTime))
                .ForMember(dto => dto.total_point, (map) => map.MapFrom(obj => obj.totalPoint))
                .ForMember(dto => dto.update_time, (map) => map.MapFrom(obj => obj.updateTime))
                .ForMember(dto => dto.secret_status, (map) => map.MapFrom(obj => obj.secretStatus))
                .ForMember(dto => dto.status, (map) => map.MapFrom(obj => obj.status));

            #endregion

            #region Point

            x.CreateMap<Point_Exchange, PointExchangeAppDto>()
                .ForMember(dto => dto.Id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.exchangeId, (map) => map.MapFrom(obj => obj.member_id))
                .ForMember(dto => dto.exchangeName, (map) => map.MapFrom(obj => obj.Member.name))
                .ForMember(dto => dto.amount, (map) => map.MapFrom(obj => obj.amount))
                .ForMember(dto => dto.status, (map) => map.MapFrom(obj => obj.status))
                .ForMember(dto => dto.exchangeTime, (map) => map.MapFrom(obj => obj.exchange_time));

            #endregion

            #region MemberCouponAccount


            x.CreateMap<Member_Coupon_Account, MemberCouponAccountAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.personIdNo, (map) => map.MapFrom(obj => obj.member_id))
                .ForMember(dto => dto.isUse, (map) => map.MapFrom(obj => obj.is_use))
                .ForMember(dto => dto.useTime, (map) => map.MapFrom(obj => obj.use_time))
                .ForMember(dto => dto.couponId, (map) => map.MapFrom(obj => obj.coupon_id))
                .ForMember(dto => dto.createTime, (map) => map.MapFrom(obj => obj.create_time.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.serverTime, (map) => map.MapFrom(obj => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.couponName, (map) => map.MapFrom(obj => obj.Coupon_Info.name))
                .ForMember(dto => dto.personName, (map) => map.MapFrom(obj => obj.Member.name))
                .ForMember(dto => dto.discount, (map) => map.MapFrom(obj => obj.Coupon_Info.discount))
                .ForMember(dto => dto.startTime, (map) => map.MapFrom(obj => obj.Coupon_Info.start_time==null?null:obj.Coupon_Info.start_time.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.endTime, (map) => map.MapFrom(obj => obj.Coupon_Info.end_time == null ? null : obj.Coupon_Info.end_time.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.isLoseValid, (map) => map.MapFrom(obj => obj.Coupon_Info.is_lose_valid));

            #endregion

            #region MicroClassInfo

            x.CreateMap<Micro_Class_Info, MicroClassInfoAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.groupId, (map) => map.MapFrom(obj => obj.group_id))
                .ForMember(dto => dto.topicInfoTitle, (map) => map.MapFrom(obj => obj.topic_info_title))
                .ForMember(dto => dto.lecturerText, (map) => map.MapFrom(obj => obj.lecturer_text))
                .ForMember(dto => dto.mp3Url, (map) => map.MapFrom(obj => obj.mp3_url))
                .ForMember(dto => dto.mp3Duration, (map) =>map.MapFrom(obj=>obj.mp3_duration))
                .ForMember(dto => dto.imgUrl, (map) => map.MapFrom(obj => obj.img_url ?? ""))
                .ForMember(dto => dto.createTime, (map) => map.MapFrom(obj => obj.create_time.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.updateTime, (map) => map.MapFrom(obj => obj.update_time == null ? null : obj.update_time.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.adminSys, (map) => map.MapFrom(obj => true));


            x.CreateMap<Micro_Class_Group, MicroClassGroupAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.status, (map) => map.MapFrom(obj => obj.status))
                .ForMember(dto => dto.name, (map) => map.MapFrom(obj => obj.name))
                .ForMember(dto => dto.imgUrl, (map) => map.MapFrom(obj => obj.img_url))
                .ForMember(dto => dto.createTime, (map) => map.MapFrom(obj => obj.create_time.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.updateTime, (map) => map.MapFrom(obj => obj.update_time == null ? null : obj.update_time.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            #endregion

            #region Product
            x.CreateMap<Product_Info, ProductInfoAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.productCode, (map) => map.MapFrom(obj => obj.product_code))
                .ForMember(dto => dto.productName, (map) => map.MapFrom(obj => obj.product_name))
                //.ForMember(dto => dto.CategoryId, (map) => map.MapFrom(obj => obj.CategoryId))
                .ForMember(dto => dto.thumbPic, (map) => map.MapFrom(obj => obj.thumb_pic))
                .ForMember(dto => dto.memberPrice, (map) => map.MapFrom(obj => obj.member_price))
                .ForMember(dto => dto.summary, (map) => map.MapFrom(obj => obj.summary))
                .ForMember(dto => dto.descUrl, (map) => map.MapFrom(obj => obj.desc_url))
                .ForMember(dto => dto.costPrice, (map) => map.MapFrom(obj => obj.cost_price))
                .ForMember(dto => dto.realPrice, (map) => map.MapFrom(obj => obj.real_price))
                .ForMember(dto => dto.firstDistributionPrice, (map) => map.MapFrom(obj => obj.first_distribution_price))
                .ForMember(dto => dto.secondDistributionPrice, (map) => map.MapFrom(obj => obj.second_distribution_price))
                .ForMember(dto => dto.salesNum, (map) => map.MapFrom(obj => obj.sales_num))
                .ForMember(dto => dto.stock, (map) => map.MapFrom(obj => obj.stock))
                .ForMember(dto => dto.status, (map) => map.MapFrom(obj => obj.status))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no))
                .ForMember(dto => dto.picUrl, (map) => map.MapFrom(obj => obj.pic_url))
                .ForMember(dto => dto.videoUrl, (map) => map.MapFrom(obj => obj.video_url))
                .ForMember(dto => dto.createDateTime, (map) => map.MapFrom(obj => obj.create_date_time))
                .ForMember(dto => dto.modifyDateTime, (map) => map.MapFrom(obj => obj.modify_date_time))
                //.ForMember(dto => dto.modifyPerson, (map) => map.MapFrom(obj => obj.Modify_User.name))
                .ForMember(dto => dto.attributeList, (map) => map.MapFrom(obj => obj.Product_Attribute))
                .ForMember(dto => dto.productCategoryList, (map) => map.MapFrom(obj => obj.Product_Category))
                .ForMember(dto => dto.white, (map) => map.MapFrom(obj => obj.star_white))
                .ForMember(dto => dto.granularity, (map) => map.MapFrom(obj => obj.star_granularity))
                .ForMember(dto => dto.defect, (map) => map.MapFrom(obj => obj.star_defect))
                .ForMember(dto => dto.oily, (map) => map.MapFrom(obj => obj.star_oily));

            x.CreateMap<Product_Pictures, ProductPictureAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.picUrl, (map) => map.MapFrom(obj => obj.pic_url))
                .ForMember(dto => dto.productId, (map) => map.MapFrom(obj => obj.product_id))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no))
                .ForMember(dto => dto.videoUrl, (map) => map.MapFrom(obj => obj.video_url));

            x.CreateMap<Product_Category, ProductCategoryAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.categoryName, (map) => map.MapFrom(obj => obj.category_name))
                .ForMember(dto => dto.imgUrl, (map) => map.MapFrom(obj => obj.img_url))
                .ForMember(dto => dto.parentId, (map) => map.MapFrom(obj => obj.parent_id))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no))
                .ForMember(dto => dto.childCategoryList, (map) => map.MapFrom(obj => obj.Product_CategoryChild));

            x.CreateMap<Product_Category, ProductCategoryInAttributeAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.categoryName, (map) => map.MapFrom(obj => obj.category_name))
                .ForMember(dto => dto.imgUrl, (map) => map.MapFrom(obj => obj.img_url))
                .ForMember(dto => dto.parentId, (map) => map.MapFrom(obj => obj.parent_id))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no));

            x.CreateMap<Product_Category, ProductCategorySimpleAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.categoryName, (map) => map.MapFrom(obj => obj.category_name))
                .ForMember(dto => dto.imgUrl, (map) => map.MapFrom(obj => obj.img_url))
                .ForMember(dto => dto.parentId, (map) => map.MapFrom(obj => obj.parent_id))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no));

            x.CreateMap<Product_Group, ProductGroupAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.groupName, (map) => map.MapFrom(obj => obj.group_name))
                .ForMember(dto => dto.groupDesc, (map) => map.MapFrom(obj => obj.group_desc)) 
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no));


            x.CreateMap<Product_Attribute, ProductAttributeAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.attributeName, (map) => map.MapFrom(obj => obj.attribute_name))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no))
                .ForMember(dto => dto.parentId, (map) => map.MapFrom(obj => obj.parent_id))
                .ForMember(dto => dto.inputType, (map) => map.MapFrom(obj => obj.input_type))
                .ForMember(dto => dto.categoryId, (map) => map.MapFrom(obj => obj.category_id))
                .ForMember(dto => dto.childAttributeList, (map) => map.MapFrom(obj => obj.Product_AttributeChild))
                .ForMember(dto => dto.productCategory, (map) => map.MapFrom(obj => obj.Product_Category));

            x.CreateMap<Product_Attribute, ProductAttributeSimpleAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.attributeName, (map) => map.MapFrom(obj => obj.attribute_name))
                .ForMember(dto => dto.serialNo, (map) => map.MapFrom(obj => obj.serial_no))
                .ForMember(dto => dto.parentId, (map) => map.MapFrom(obj => obj.parent_id))
                .ForMember(dto => dto.inputType, (map) => map.MapFrom(obj => obj.input_type))
                .ForMember(dto => dto.categoryId, (map) => map.MapFrom(obj => obj.category_id));

            #endregion

            #region Order

            x.CreateMap<Product_Order_Record, ProductOrderRecordAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.orderId, (map) => map.MapFrom(obj => obj.order_id))
                .ForMember(dto => dto.modifyComment, (map) => map.MapFrom(obj => obj.modify_comment))
                .ForMember(dto => dto.modifyUserId, (map) => map.MapFrom(obj => obj.modify_user_id))
                .ForMember(dto => dto.modifyDateTime, (map) => map.MapFrom(obj => obj.modify_date_time.ToString("yyyy-MM-dd HH:mm:ss")));

            x.CreateMap<Product_Order_Detail, ProductOrderDetailAppDto>()
                .ForMember(dto => dto.Id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.orderIdNo, (map) => map.MapFrom(obj => obj.order_id))
                //.ForMember(dto => dto.CategoryId, (map) => map.MapFrom(obj => obj.))
                .ForMember(dto => dto.productCode, (map) => map.MapFrom(obj => obj.product_code))
                .ForMember(dto => dto.productName, (map) => map.MapFrom(obj => obj.product_name))
                .ForMember(dto => dto.thumbPic, (map) => map.MapFrom(obj => obj.thumb_pic))
                .ForMember(dto => dto.buyNum, (map) => map.MapFrom(obj => obj.buy_num))
                .ForMember(dto => dto.summary, (map) => map.MapFrom(obj => obj.summary))
                .ForMember(dto => dto.descUrl, (map) => map.MapFrom(obj => obj.desc_url))
                .ForMember(dto => dto.costPrice, (map) => map.MapFrom(obj => obj.cost_price))
                .ForMember(dto => dto.currentPrice, (map) => map.MapFrom(obj => obj.current_price))
                .ForMember(dto => dto.firstDistributionPrice, (map) => map.MapFrom(obj => obj.first_distribution_price))
                .ForMember(dto => dto.secondDistributionPrice, (map) => map.MapFrom(obj => obj.second_distribution_price))
                .ForMember(dto => dto.memberPrice, (map) => map.MapFrom(obj => obj.member_price)) 
                .ForMember(dto => dto.isReceiving, (map) => map.MapFrom(obj => obj.is_receiving))
                .ForMember(dto => dto.couponAccountId, (map) => map.MapFrom(obj => obj.coupon_id))
                .ForMember(dto => dto.couponName, (map) => map.MapFrom(obj => obj.coupon_name))
                .ForMember(dto => dto.couponSalePrice, (map) => map.MapFrom(obj => obj.coupon_sale_price))
                .ForMember(dto => dto.couponId, (map) => map.MapFrom(obj => obj.Member_Coupon_Account.coupon_id));

            x.CreateMap<ProductOrderDetailAppDto, Product_Order_Detail>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.Id))
                .ForMember(dto => dto.order_id, (map) => map.MapFrom(obj => obj.orderIdNo))
                .ForMember(dto => dto.product_id, (map) => map.MapFrom(obj => obj.productId))
                .ForMember(dto => dto.product_code, (map) => map.MapFrom(obj => obj.productCode))
                .ForMember(dto => dto.product_name, (map) => map.MapFrom(obj => obj.productName))
                .ForMember(dto => dto.thumb_pic, (map) => map.MapFrom(obj => obj.thumbPic))
                .ForMember(dto => dto.buy_num, (map) => map.MapFrom(obj => obj.buyNum))
                .ForMember(dto => dto.summary, (map) => map.MapFrom(obj => obj.summary))
                .ForMember(dto => dto.desc_url, (map) => map.MapFrom(obj => obj.descUrl))
                .ForMember(dto => dto.cost_price, (map) => map.MapFrom(obj => obj.costPrice))
                .ForMember(dto => dto.current_price, (map) => map.MapFrom(obj => obj.currentPrice))
                .ForMember(dto => dto.first_distribution_price, (map) => map.MapFrom(obj => obj.firstDistributionPrice))
                .ForMember(dto => dto.second_distribution_price, (map) => map.MapFrom(obj => obj.secondDistributionPrice))
                .ForMember(dto => dto.member_price, (map) => map.MapFrom(obj => obj.memberPrice))
                .ForMember(dto => dto.coupon_id, (map) => map.MapFrom(obj => obj.couponAccountId))
                .ForMember(dto => dto.coupon_name, (map) => map.MapFrom(obj => obj.couponName))
                .ForMember(dto => dto.coupon_sale_price, (map) => map.MapFrom(obj => obj.couponSalePrice));

            x.CreateMap<Product_Order_Master, ProductOrderMasterAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.personIdNo, (map) => map.MapFrom(obj => obj.member_id))
                .ForMember(dto => dto.memberId, (map) => map.MapFrom(obj => obj.member_id))
                .ForMember(dto => dto.consignee, (map) => map.MapFrom(obj => obj.consignee))
                .ForMember(dto => dto.consigneeAddress, (map) => map.MapFrom(obj => obj.consignee_address))
                .ForMember(dto => dto.consigneePhone, (map) => map.MapFrom(obj => obj.consignee_phone))
                .ForMember(dto => dto.orderPrice, (map) => map.MapFrom(obj => obj.order_price))
                .ForMember(dto => dto.productNum, (map) => map.MapFrom(obj => obj.product_num))
                .ForMember(dto => dto.currentStatus, (map) => map.MapFrom(obj => obj.current_status))
                .ForMember(dto => dto.transportNo, (map) => map.MapFrom(obj => obj.transport_no ?? ""))
                .ForMember(dto => dto.transportCompany, (map) => map.MapFrom(obj => obj.transport_company ?? ""))
                .ForMember(dto => dto.orderDateTime, (map) => map.MapFrom(obj => obj.order_date_time))
                .ForMember(dto => dto.paymentDateTime, (map) => map.MapFrom(obj => obj.payment_date_time))
                .ForMember(dto => dto.refundDateTime, (map) => map.MapFrom(obj => obj.refund_date_time))  
                .ForMember(dto => dto.totalRebateAmount, (map) => map.MapFrom(obj => obj.total_Rebate_Amount))
                .ForMember(dto => dto.superiorRebateAmount, (map) => map.MapFrom(obj => obj.superior_Rebate_Amount)) 
                .ForMember(dto => dto.superiorParentRebateAmount, (map) => map.MapFrom(obj => obj.superior_Parent_Rebate_Amount))  
                .ForMember(dto => dto.receivingRealPrice, (map) => map.MapFrom(obj => obj.receiving_real_price))   
                .ForMember(dto => dto.realTotalPrice, (map) => map.MapFrom(obj => obj.real_total_price))  
                .ForMember(dto => dto.productOrderRecordList, (map) => map.MapFrom(obj => obj.Product_Order_Record))
                .ForMember(dto => dto.productOrderDetailList, (map) => map.MapFrom(obj => obj.Product_Order_Detail));


            x.CreateMap<ProductOrderMasterAppDto, Product_Order_Master>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.member_id, (map) => map.MapFrom(obj => obj.memberId))
                .ForMember(dto => dto.consignee, (map) => map.MapFrom(obj => obj.consignee))
                .ForMember(dto => dto.consignee_address, (map) => map.MapFrom(obj => obj.consigneeAddress))
                .ForMember(dto => dto.consignee_phone, (map) => map.MapFrom(obj => obj.consigneePhone))
                .ForMember(dto => dto.order_price, (map) => map.MapFrom(obj => obj.orderPrice))
                .ForMember(dto => dto.product_num, (map) => map.MapFrom(obj => obj.productNum))
                .ForMember(dto => dto.current_status, (map) => map.MapFrom(obj => obj.currentStatus))
                .ForMember(dto => dto.transport_no, (map) => map.MapFrom(obj => obj.transportNo))
                .ForMember(dto => dto.transport_company, (map) => map.MapFrom(obj => obj.transportCompany))
                .ForMember(dto => dto.order_date_time, (map) => map.MapFrom(obj => obj.orderDateTime))
                .ForMember(dto => dto.payment_date_time, (map) => map.MapFrom(obj => obj.paymentDateTime))
                .ForMember(dto => dto.refund_date_time, (map) => map.MapFrom(obj => obj.refundDateTime))
                .ForMember(dto => dto.Product_Order_Detail, (map) => map.MapFrom(obj => obj.productOrderDetailList));


            #endregion

            #region OrderRebate
            x.CreateMap<Order_Rebate_New, OrderRebateAppDto>()
                .ForMember(dto => dto.id, (map) => map.MapFrom(obj => obj.id))
                .ForMember(dto => dto.reciverId, (map) => map.MapFrom(obj => obj.reciver_id)) 
                .ForMember(dto => dto.reciverRebate, (map) => map.MapFrom(obj => obj.reciver_rebate))
                .ForMember(dto => dto.reciverLevel, (map) => map.MapFrom(obj => obj.reciver_level))
                .ForMember(dto => dto.giverId, (map) => map.MapFrom(obj => obj.giver_id))
                .ForMember(dto => dto.giverName, (map) => map.MapFrom(obj => obj.Giver.name))
                .ForMember(dto => dto.status, (map) => map.MapFrom(obj => obj.status))
                .ForMember(dto => dto.reason, (map) => map.MapFrom(obj => obj.reason))
                .ForMember(dto => dto.rebateTime, (map) => map.MapFrom(obj => obj.rebate_time.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dto => dto.updateTime, (map) => map.MapFrom(obj => obj.update_time == null?null: obj.update_time.Value.ToString("yyyy-MM-dd HH:mm:ss"))) ;
            #endregion

 
        }

    }
}