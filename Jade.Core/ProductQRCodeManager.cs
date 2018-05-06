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
    public class ProductQRCodeManager
    {
        private static readonly ProductQRCodeManager _instance = new ProductQRCodeManager();
        public static ProductQRCodeManager Instance
        {
            get { return _instance; }
        }

        private ProductQRCodeManager()
        {

        }

        public Product_Anti_Fake GetProductQRCode(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Anti_Fake product = db.Product_Anti_Fake
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return product;
            }
        }

        public NormalResult CreateProductQRCode(Product_Anti_Fake product)
        {
            //product.Id = Guid.NewGuid();

            product.status = 0;
            product.create_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                if (db.Product_Anti_Fake.Any(s => s.product_code == product.product_code))
                {
                    return new NormalResult("商品编码重复，已被其他商品占用。");
                }

                db.Product_Anti_Fake.Add(product);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateProductQRCode(Product_Anti_Fake product)
        {

            using (Entities db = new Entities())
            {
                if (db.Product_Anti_Fake.Any(s => s.product_code == product.product_code && s.id != product.id))
                {
                    return new NormalResult("商品编码重复，已被其他商品占用。");
                }

                IQueryable<Product_Anti_Fake> queryable = db.Product_Anti_Fake;

                Product_Anti_Fake dbProduct_Anti_Fake = queryable.FirstOrDefault(e => e.id == product.id);
                if (dbProduct_Anti_Fake == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(product, dbProduct_Anti_Fake);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        public void RemoveProductQRCode(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Anti_Fake product = db.Product_Anti_Fake.FirstOrDefault(e => e.id == id);
                if (product != null)
                {
                    db.Product_Anti_Fake.Remove(product);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Product_Anti_Fake> GetProductQRCodeList(GetListDataArgs args)
        {
            GetListDataResult<Product_Anti_Fake> result = new GetListDataResult<Product_Anti_Fake>();
            using (Entities db = new Entities())
            {
                IQueryable<Product_Anti_Fake> queryable = db.Product_Anti_Fake
                    .AsNoTracking();


                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.product_name.Contains(keyword) || c.product_code.Contains(keyword));
                }

                if (args.Parameters.IsNullOrEmpty("status") == false)
                {
                    int status = args.Parameters.GetIntValue("status");
                    queryable = queryable.Where(c => c.status == status);
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

    }
}
