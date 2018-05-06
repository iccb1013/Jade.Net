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
    public class OrderManager
    {
        private static readonly OrderManager _instance = new OrderManager();
        public static OrderManager Instance
        {
            get { return _instance; }
        }


        private Random _random = new Random((int)DateTime.Now.Ticks);

        private OrderManager()
        {

        }

        public Product_Order_Master GetOrder(string id)
        {
            using (Entities db = new Entities())
            {
                Product_Order_Master order = db.Product_Order_Master
                    .Include(c => c.Product_Order_Detail)
                    .Include(c => c.Product_Order_Record.Select(r => r.Modify_User))
                    .Include(c => c.Member)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return order;
            }
        }

        public NormalResult CreateOrder(Product_Order_Master order)
        {
            if (order.Product_Order_Detail == null || order.Product_Order_Detail.Count == 0)
                return new NormalResult("订单明细数据无效。");

            //product.Id = Guid.NewGuid();

            using (Entities db = new Entities())
            {
                Member member = db.Member.FirstOrDefault(e => e.id == order.member_id);
                if (member == null)
                    return new NormalResult("订单明细中的 member_id 无效。");

                if (member.status != 2)
                    return new NormalResult("指定会员的状态不是审核通过状态。");

                if (String.IsNullOrEmpty(member.phone_num) || member.phone_num.Length < 11)
                    return new NormalResult("指定会员的手机号码无效。");

                order.id = DateTime.Now.ToString("yyyyMMddHHmmss") + member.phone_num.Substring(7, 4) + _random.Next(100, 999).ToString();
                order.order_date_time = DateTime.Now;
                order.real_total_price = 0;
                order.order_price = 0;
                order.product_num = 0;
                order.current_status = 1;//待付费
                order.rebate_flag = 0;
                order.del_flag = 0;
                order.total_Rebate_Amount = 0;
                order.superior_Rebate_Amount = 0;
                order.superior_Parent_Rebate_Amount = 0;
                order.receiving_real_price = 0;

                foreach (var detailItem in order.Product_Order_Detail)
                {
                    #region 验证

                    //App 里不会传productId的 都是传product_code
                    detailItem.product_id = Convert.ToInt32(detailItem.product_code);
                    Product_Info product = db.Product_Info.FirstOrDefault(e => e.id == detailItem.product_id);
                    if (product == null)
                        return new NormalResult("订单明细中的 product_id 无效。");

                    if (detailItem.buy_num <= 0)
                        return new NormalResult("订单明细中的商品购买数量不正确：" + product.product_name);

                    if (product.status == 0)
                        return new NormalResult("订单明细中的商品已下架：" + product.product_name);

                    if (product.stock < detailItem.buy_num)
                        return new NormalResult("订单明细中的商品库存不足：" + product.product_name);

                    Member_Coupon_Account couponAccount = null;
                    Coupon_Info coupon = null;
                    if (detailItem.coupon_id.HasValue)
                    {
                        if (detailItem.buy_num > 1)
                            return new NormalResult("订单明细中的商品在使用券的情况下，购买数量仅限1件：" + product.product_name);

                        couponAccount = db.Member_Coupon_Account.FirstOrDefault(e => e.id == detailItem.coupon_id.Value);
                        if (couponAccount == null)
                            return new NormalResult("订单明细中的 coupon_id 无效：" + product.product_name);

                        if (couponAccount.is_use == 1)
                            return new NormalResult("订单明细中指定的券是已经使用过的券：" + product.product_name);

                        coupon = db.Coupon_Info.FirstOrDefault(e => e.id == couponAccount.coupon_id);
                        if (couponAccount == null)
                            return new NormalResult("订单明细中指定商品所使用的券不存在：" + product.product_name);

                        if (coupon.is_lose_valid == 1)
                            return new NormalResult("订单明细中指定商品所使用的券已经失效：" + product.product_name);

                        if (coupon.start_time.HasValue && coupon.start_time.Value > DateTime.Now)
                            return new NormalResult("订单明细中指定商品所使用的券还没有到开始使用时间：" + product.product_name);

                        if (coupon.end_time.HasValue && coupon.end_time.Value < DateTime.Now)
                            return new NormalResult("订单明细中指定商品所使用的券已经超过了使用截止使用时间：" + product.product_name);

                    }

                    #endregion

                    #region 给明细表赋值

                    detailItem.order_id = order.id;
                    detailItem.product_code = product.product_code;
                    detailItem.product_name = product.product_name;
                    detailItem.thumb_pic = product.thumb_pic;
                    detailItem.summary = product.summary;
                    detailItem.desc_url = product.desc_url;
                    detailItem.real_price = product.real_price;
                    detailItem.first_distribution_price = product.first_distribution_price;
                    detailItem.second_distribution_price = product.second_distribution_price;
                    detailItem.member_price = product.member_price;
                    detailItem.cost_price = product.cost_price;
                    if (coupon != null)
                    {
                        detailItem.coupon_name = coupon.name;

                        //更新为已使用状态
                        couponAccount.is_use = 1;
                        couponAccount.use_time = DateTime.Now;

                    }
                    detailItem.is_receiving = 0;


                    #endregion

                    #region 计算价格

                    //如果使用了券，则用会员价计算
                    //如果没使用券，则看会员的级别
                    if (coupon != null)
                    {
                        detailItem.current_price = coupon.discount * product.member_price;
                        detailItem.coupon_sale_price = product.member_price - detailItem.current_price;
                    }
                    else
                    {
                        switch (member.type)
                        {
                            case 1: //一级代理
                                detailItem.current_price = product.first_distribution_price;
                                break;
                            case 2: //二级代理
                                detailItem.current_price = product.second_distribution_price;
                                break;
                            case 3: //会员
                                detailItem.current_price = product.member_price;
                                break;
                            default:
                                return new NormalResult("指定会员的会员级别无效：" + member.type);
                        }
                    }

                    #endregion

                    #region 给订单主表字段赋值

                    order.real_total_price += product.real_price;
                    order.order_price += detailItem.cost_price;
                    order.product_num += detailItem.buy_num;

                    #endregion
                }

                db.Product_Order_Master.Add(order);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult RemoveOrder(string id)
        {
            using (Entities db = new Entities())
            {
                Product_Order_Master order = db.Product_Order_Master.FirstOrDefault(e => e.id == id);
                if (order != null)
                {
                    //db.Product_Order_Master.Remove(member);

                    order.del_flag = 1;
                    db.SaveChanges();
                }
            }

            return new NormalResult();
        }

        public GetListDataResult<Product_Order_Master> GetOrderList(GetListDataArgs args)
        {
            GetListDataResult<Product_Order_Master> result = new GetListDataResult<Product_Order_Master>();
            using (Entities db = new Entities())
            {
                IQueryable<Product_Order_Master> queryable = db.Product_Order_Master
                    .Include(c => c.Product_Order_Detail)
                    .Include(c => c.Product_Order_Detail.Select(t=>t.Member_Coupon_Account))
                    .Include(c=>c.Product_Order_Record)
                    .Include(c => c.Member)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue("Keyword");
                    queryable = queryable.Where(c => c.id.Contains(keyword) || c.Member.name.Contains(keyword) || c.Member.phone_num.Contains(keyword)
                    || c.consignee.Contains(keyword) || c.consignee_address.Contains(keyword) || c.consignee_phone.Contains(keyword));
                }

                if (args.Parameters.IsNullOrEmpty("member_id") == false)
                {
                    int memberId = args.Parameters.GetIntValue("member_id");
                    queryable = queryable.Where(c => c.member_id == memberId);
                }

                if (args.Parameters.IsNullOrEmpty("current_status") == false)
                {
                    int currentStatus = args.Parameters.GetIntValue("current_status");
                    if (currentStatus > 0)
                    {
                        queryable = queryable.Where(c => c.current_status == currentStatus);
                    }
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

        public NormalResult CreateOrderRecord(CreateOrderRecordArgs args)
        {
            using (Entities db = new Entities())
            {
                Product_Order_Master order = db.Product_Order_Master
                    .FirstOrDefault(e => e.id == args.order_id);
                if (order == null)
                    return new NormalResult("order_id 无效。");

                if (order.del_flag == 1)
                    return new NormalResult("指定的订单已被删除。");

                if (order.current_status == 9)
                    return new NormalResult("已完成的订单不允许再修改状态。");

                if (order.current_status == args.newOrder_status)
                    return new NormalResult("指定的订单新状态没有变化。");

                //更新订单状态
                order.current_status = args.newOrder_status;
                switch (args.newOrder_status)
                {
                    case 2: //待发货
                        order.payment_date_time = DateTime.Now;
                        break;
                    case 3: //已发货
                        order.transport_no = args.transport_no;
                        order.transport_company = args.transport_company;
                        break;
                    case 5://退货
                        order.refund_date_time = DateTime.Now;
                        break;
                    case 9: //已完成
                        order.order_complete_time = DateTime.Now;
                        break;
                }

                //如果是把订单状态修改为已完成，需要一些业务处理
                if (order.current_status == 9)
                {
                    //计算已收货总成本价，更新订单明细中是否已收货
                    order.receiving_real_price = 0;
                    foreach (var detailItem in order.Product_Order_Detail)
                    {
                        if (args.receiving_detail_id.Contains(detailItem.id) == false)
                            continue;

                        detailItem.is_receiving = 1;
                        order.receiving_real_price += detailItem.real_price;

                    }

                    #region 计算返点

                    #region 总返点

                    //总返点和上下级代码没有任何关系，就是自己卖的货，自己得该货原始价的百分之多少的返点，
                    //会员没有，二级代理得5%，一级代理得10%。
                    if (order.Member.type == 1 || order.Member.type == 2)
                    {
                        Order_Rebate_New orderRebateNew = new Order_Rebate_New();
                        orderRebateNew.order_id = order.id;
                        orderRebateNew.reciver_id = order.member_id;
                        orderRebateNew.reciver_level = order.Member.type;
                        orderRebateNew.giver_id = order.member_id;
                        orderRebateNew.giver_level = order.Member.type;
                        orderRebateNew.reason = "订单总返点，订单号：" + order.id;
                        orderRebateNew.status = 1;
                        orderRebateNew.rebate_type = 2;
                        orderRebateNew.rebate_time = DateTime.Now;

                        if (order.Member.type == 1)
                            orderRebateNew.reciver_rebate = (int)(order.receiving_real_price * 0.1m);
                        else
                            orderRebateNew.reciver_rebate = (int)(order.receiving_real_price * 0.05m);

                        db.Order_Rebate_New.Add(orderRebateNew);

                        order.total_Rebate_Amount = orderRebateNew.reciver_rebate;
                        order.Member.total_point += orderRebateNew.reciver_rebate;

                    }

                    #endregion

                    #region 分销返点

                    //按会员上下级关系确定，卖货的人上线得原始价的8%，上线的上线得原始价的4%。
                    if (order.Member.Superior_Agent != null)
                    {
                        Order_Rebate_New orderRebateNew = new Order_Rebate_New();
                        orderRebateNew.order_id = order.id;
                        orderRebateNew.reciver_id = order.Member.Superior_Agent.id;
                        orderRebateNew.reciver_level = order.Member.Superior_Agent.type;
                        orderRebateNew.giver_id = order.member_id;
                        orderRebateNew.giver_level = order.Member.type;
                        orderRebateNew.reason = "分销返点，订单号：" + order.id;
                        orderRebateNew.status = 1;
                        orderRebateNew.rebate_type = 1;
                        orderRebateNew.rebate_time = DateTime.Now;

                        orderRebateNew.reciver_rebate = (int)(order.receiving_real_price * 0.08m);

                        db.Order_Rebate_New.Add(orderRebateNew);

                        order.superior_Rebate_Amount = orderRebateNew.reciver_rebate;
                        order.Member.Superior_Agent.total_point += orderRebateNew.reciver_rebate;

                        if (order.Member.Superior_Agent.Superior_Agent != null)
                        {
                            Order_Rebate_New orderRebateNew2 = new Order_Rebate_New();
                            orderRebateNew2.order_id = order.id;
                            orderRebateNew2.reciver_id = order.Member.Superior_Agent.Superior_Agent.id;
                            orderRebateNew2.reciver_level = order.Member.Superior_Agent.Superior_Agent.type;
                            orderRebateNew2.giver_id = order.member_id;
                            orderRebateNew2.giver_level = order.Member.type;
                            orderRebateNew2.reason = "分销返点，订单号：" + order.id;
                            orderRebateNew2.status = 1;
                            orderRebateNew2.rebate_type = 1;
                            orderRebateNew2.rebate_time = DateTime.Now;

                            orderRebateNew2.reciver_rebate = (int)(order.receiving_real_price * 0.04m);

                            db.Order_Rebate_New.Add(orderRebateNew2);

                            order.superior_Parent_Rebate_Amount = orderRebateNew2.reciver_rebate;
                            order.Member.Superior_Agent.Superior_Agent.total_point += orderRebateNew2.reciver_rebate;
                        }
                    }

                    #endregion

                    #endregion

                }

                Product_Order_Record record = new Product_Order_Record();
                record.order_id = args.order_id;
                record.modify_comment = args.modify_comment;
                record.modify_user_id = args.modify_user_id;
                record.modify_date_time = DateTime.Now;

                db.Product_Order_Record.Add(record);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        #region OrderRebate

        public GetListDataResult<Order_Rebate_New> GetOrderRebateList(GetListDataArgs args)
        {
            GetListDataResult<Order_Rebate_New> result = new GetListDataResult<Order_Rebate_New>();
            using (Entities db = new Entities())
            {
                IQueryable<Order_Rebate_New> queryable = db.Order_Rebate_New 
                    .Include(e=>e.Giver)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("reciver_id") == false)
                {
                    int reciverId = args.Parameters.GetIntValue("reciver_id");
                    queryable = queryable.Where(c => c.reciver_id == reciverId);
                }

                if (args.Parameters.IsNullOrEmpty("status") == false)
                {
                    int status = args.Parameters.GetIntValue("status"); 
                    queryable = queryable.Where(c => c.status == status); 
                }

                if (args.Parameters.IsNullOrEmpty("rebate_type") == false)
                {
                    int rebateType = args.Parameters.GetIntValue("rebate_type");

                    if (rebateType > 0)
                    {
                        queryable = queryable.Where(c => c.rebate_type == rebateType);
                    }
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
    }
}
