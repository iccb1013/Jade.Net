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
    public class ProductManager
    {
        private static readonly ProductManager _instance = new ProductManager();
        public static ProductManager Instance
        {
            get { return _instance; }
        }

        private ProductManager()
        {

        }

        #region Product

        public Product_Info GetProduct(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Info product = db.Product_Info
                    .Include(c => c.Product_Category)
                    .Include(c => c.Product_Attribute)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return product;
            }
        }

        public NormalResult CreateProduct(Product_Info product, int[] catalogIdList, int[] attributeIdList)
        {
            //product.Id = Guid.NewGuid();

            product.status = 1;
            product.create_date_time = DateTime.Now;
            product.modify_date_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                if (db.Product_Info.Any(s => s.product_code == product.product_code))
                {
                    return new NormalResult("商品编码重复，已被其他商品占用。");
                }

                if (catalogIdList != null && catalogIdList.Length > 0)
                {
                    List<Product_Category> dictionaryItemList =
                         db.Product_Category.Where(c => catalogIdList.Contains(c.id)).ToList();
                    foreach (var item in dictionaryItemList)
                    {
                        product.Product_Category.Add(item);
                    }
                }

                if (attributeIdList != null && attributeIdList.Length > 0)
                {
                    List<Product_Attribute> dictionaryItemList =
                         db.Product_Attribute.Where(c => attributeIdList.Contains(c.id)).ToList();
                    foreach (var item in dictionaryItemList)
                    {
                        product.Product_Attribute.Add(item);
                    }
                }

                db.Product_Info.Add(product);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateProduct(Product_Info product,int [] catalogIdList, int[] attributeIdList)
        {
            product.modify_date_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                if (db.Product_Info.Any(s => s.product_code == product.product_code && s.id != product.id))
                {
                    return new NormalResult("商品编码重复，已被其他商品占用。");
                }

                IQueryable<Product_Info> queryable = db.Product_Info.Include(c=>c.Product_Category);

                Product_Info dbProduct_Info = queryable.FirstOrDefault(e => e.id == product.id);
                if (dbProduct_Info == null)
                    return new NormalResult("指定的数据不存在。");
                
                ShengMapper.SetValuesWithoutProperties(product, dbProduct_Info, 
                    new string[] { "sales_num", "browse_count" }, true);

                dbProduct_Info.Product_Category.Clear();

                if (catalogIdList != null && catalogIdList.Length > 0)
                {
                    List<Product_Category> dictionaryItemList =
                         db.Product_Category.Where(c => catalogIdList.Contains(c.id)).ToList();
                    foreach (var item in dictionaryItemList)
                    {
                        dbProduct_Info.Product_Category.Add(item);
                    }
                }


                dbProduct_Info.Product_Attribute.Clear();

                if (attributeIdList != null && attributeIdList.Length > 0)
                {
                    List<Product_Attribute> dictionaryItemList =
                         db.Product_Attribute.Where(c => attributeIdList.Contains(c.id)).ToList();
                    foreach (var item in dictionaryItemList)
                    {
                        dbProduct_Info.Product_Attribute.Add(item);
                    }
                }


                dbProduct_Info.modify_date_time = DateTime.Now;

                db.SaveChanges();
            }

            return new NormalResult();
        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        public void RemoveProduct(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Info product = db.Product_Info.FirstOrDefault(e => e.id == id);
                if (product != null)
                {
                    //db.Product_Info.Remove(product);

                    product.status = 0;
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Product_Info> GetProductList(GetListDataArgs args)
        {
            GetListDataResult<Product_Info> result = new GetListDataResult<Product_Info>();
            using (Entities db = new Entities())
            {
                IQueryable<Product_Info> queryable = db.Product_Info
                    .Include(c => c.Product_Category)
                    .Include(c=>c.Product_Attribute)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("status") == false)
                {
                    int status = args.Parameters.GetIntValue("status");
                    queryable = queryable.Where(c => c.status == status);
                }

                if (args.Parameters.IsNullOrEmpty("stock") == false)
                {
                    bool stock = args.Parameters.GetBoolValue("stock");
                    if (stock)
                    {
                        queryable = queryable.Where(c => c.stock > 0);
                    }
                }

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.product_name.Contains(keyword) || c.product_code.Contains(keyword));
                }
              
                if (args.Parameters.IsNullOrEmpty("categoryId") == false)
                {
                    int categoryId = args.Parameters.GetIntValue("categoryId");
                    if (categoryId > 0)
                    {
                        queryable = queryable.Where(c => c.Product_Category.Any(ct => ct.id == categoryId));
                    }
                }
                if (args.Parameters.IsNullOrEmpty("groupId") == false)
                {
                    int groupId = args.Parameters.GetIntValue("groupId");
                    if (groupId > 0)
                    {
                        queryable = queryable.Where(c => c.Product_Group.Any(g => g.id == groupId));
                    }
                }
                if (args.Parameters.IsNullOrEmpty("attributeIdList") == false)
                {
                    int[] attributeIds = args.Parameters.GetValue<int[]>("attributeIdList");
                    queryable = queryable.Where(c => c.Product_Attribute.Any(g => attributeIds.Any(a=>a == g.id)));
                }
                if (args.Parameters.IsNullOrEmpty("memberType") == false)
                {
                    int memberType = args.Parameters.GetIntValue("memberType");
                    decimal priceFrom = args.Parameters.GetValue<decimal>("priceFrom");
                    decimal priceTo = args.Parameters.GetValue<decimal>("priceTo");
                    if (priceFrom > 0 || priceTo > 0)
                    {
                        //略显诡异的写法
                        if (memberType == 1)
                        {
                            queryable = queryable.Where(c => c.first_distribution_price >= priceFrom && c.first_distribution_price <= priceTo);
                        }
                        if (memberType == 2)
                        {
                            queryable = queryable.Where(c => c.second_distribution_price >= priceFrom && c.second_distribution_price <= priceTo);
                        }
                        if (memberType == 3)
                        {
                            queryable = queryable.Where(c => c.member_price >= priceFrom && c.member_price <= priceTo);
                        }
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

        #region ProductCatalog

        public Product_Category GetProductCatalog(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Category productCatalog = db.Product_Category
                    //.Include(c=>c.Superior_Agent)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return productCatalog;
            }
        }

        public NormalResult CreateProductCatalog(Product_Category productCatalog)
        {
            //productCatalog.Id = Guid.NewGuid();

            using (Entities db = new Entities())
            {
                db.Product_Category.Add(productCatalog);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateProductCatalog(Product_Category productCatalog)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Product_Category> queryable = db.Product_Category;

                Product_Category dbProduct_Category = queryable.FirstOrDefault(e => e.id == productCatalog.id);
                if (dbProduct_Category == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesWithoutProperties(productCatalog, dbProduct_Category, new string[] { "sales_num", "browse_count" }, true);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveProductCatalog(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Category productCatalog = db.Product_Category.FirstOrDefault(e => e.id == id);
                if (productCatalog != null)
                {
                    //先删除子分类
                    //只有二级分类
                    List<Product_Category> productCatalogList = db.Product_Category.Where(e => e.parent_id == id).ToList();
                    foreach (var item in productCatalogList)
                    {
                        db.Product_Category.Remove(item);
                    }

                    db.Product_Category.Remove(productCatalog);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Product_Category> GetProductCatalogList(GetListDataArgs args)
        {
            GetListDataResult<Product_Category> result = new GetListDataResult<Product_Category>();
            using (Entities db = new Entities())
            {
                IQueryable<Product_Category> queryable = db.Product_Category
                    //.Include(c => c.Superior_Agent)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue("Keyword");
                    queryable = queryable.Where(c => c.category_name.Contains(keyword));
                }

                if (args.Parameters.IsNullOrEmpty("ParentId") == false)
                {
                    int parentId = args.Parameters.GetIntValue("ParentId");
                    queryable = queryable.Where(c => c.parent_id == parentId);
                }
                else
                {
                    queryable = queryable.Where(c => c.parent_id.HasValue == false);
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

        public List<Product_Category> GetProductCatalogTree()
        {
            List<Product_Category> result;
            using (Entities db = new Entities())
            {
                IQueryable<Product_Category> queryable = db.Product_Category
                    .Include(d => d.Product_CategoryChild)
                    .OrderBy(d => d.serial_no)
                    .AsNoTracking()
                    .Where(c => c.parent_id.HasValue == false);

                result = queryable.ToList();

                foreach (var item in result)
                {
                    foreach (var childItem in item.Product_CategoryChild)
                    {
                        foreach (var subChildItem in childItem.Product_CategoryChild)
                        {
                            LoadChildProductCatalog(db, subChildItem);
                        }
                    }
                }
            }

            return result;
        }

        private void LoadChildProductCatalog(Entities db, Product_Category item)
        {
            foreach (var childItem in item.Product_CategoryChild)
            {
                LoadChildProductCatalog(db, childItem);
            }
        }


        #endregion

        #region ProductAttribute

        public Product_Attribute GetProductAttribute(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Attribute productAttribute = db.Product_Attribute
                    //.Include(c=>c.Superior_Agent)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return productAttribute;
            }
        }

        public NormalResult CreateProductAttribute(Product_Attribute productAttribute)
        {
            //productAttribute.Id = Guid.NewGuid();

            using (Entities db = new Entities())
            {
                db.Product_Attribute.Add(productAttribute);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateProductAttribute(Product_Attribute productAttribute)
        {
            using (Entities db = new Entities())
            {
                IQueryable<Product_Attribute> queryable = db.Product_Attribute;

                Product_Attribute dbProduct_Attribute = queryable.FirstOrDefault(e => e.id == productAttribute.id);
                if (dbProduct_Attribute == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(productAttribute, dbProduct_Attribute);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveProductAttribute(int id)
        {
            using (Entities db = new Entities())
            {
                Product_Attribute productAttribute = db.Product_Attribute.FirstOrDefault(e => e.id == id);
                if (productAttribute != null)
                {
                    //先删除子属性
                    List< Product_Attribute> productAttributeList = db.Product_Attribute.Where(e => e.parent_id == id).ToList();
                    foreach (var item in productAttributeList)
                    {
                        db.Product_Attribute.Remove(item);
                    }

                    db.Product_Attribute.Remove(productAttribute);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Product_Attribute> GetProductAttributeList(GetListDataArgs args)
        {
            GetListDataResult<Product_Attribute> result = new GetListDataResult<Product_Attribute>();
            using (Entities db = new Entities())
            {
                IQueryable<Product_Attribute> queryable = db.Product_Attribute
                    //.Include(c => c.Superior_Agent)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue("Keyword");
                    queryable = queryable.Where(c => c.attribute_name.Contains(keyword));
                }

                if (args.Parameters.IsNullOrEmpty("category_id") == false)
                {
                    int categoryId = args.Parameters.GetIntValue("category_id");
                    queryable = queryable.Where(c => c.category_id == categoryId);
                }
                if (args.Parameters.IsNullOrEmpty("ParentId") == false)
                {
                    int parentId = args.Parameters.GetIntValue("ParentId");
                    queryable = queryable.Where(c => c.parent_id == parentId);
                }
                else
                {
                    queryable = queryable.Where(c => c.parent_id.HasValue == false);
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

        public List<Product_Attribute> GetProductAttributeTree()
        {
            List<Product_Attribute> result;
            using (Entities db = new Entities())
            {
                IQueryable<Product_Attribute> queryable = db.Product_Attribute
                    .Include(d => d.Product_AttributeChild)
                    .OrderBy(d => d.serial_no)
                    .AsNoTracking()
                    .Where(c => c.parent_id.HasValue == false);

                result = queryable.ToList();

                foreach (var item in result)
                {
                    foreach (var childItem in item.Product_AttributeChild)
                    {
                        foreach (var subChildItem in childItem.Product_AttributeChild)
                        {
                            LoadChildProductAttribute(db, subChildItem);
                        }
                    }
                }
            }

            return result;
        }
        public List<Product_Attribute> GetProductAttributeTreeByCategoryId(int categoryId)
        {
            List<Product_Attribute> result;
            using (Entities db = new Entities())
            {
                IQueryable<Product_Attribute> queryable = db.Product_Attribute
                    .Include(d => d.Product_AttributeChild)
                    .Include(d=>d.Product_Category)
                    .OrderBy(d => d.serial_no)
                    .Where(c => c.parent_id.HasValue == false)
                    .AsNoTracking();
                 
                result = queryable.ToList();

                foreach (var item in result)
                {
                    foreach (var childItem in item.Product_AttributeChild)
                    {
                        foreach (var subChildItem in childItem.Product_AttributeChild)
                        {
                            LoadChildProductAttribute(db, subChildItem);
                        }
                    }
                }
            }

            return result;
        }

        private void LoadChildProductAttribute(Entities db, Product_Attribute item)
        {
            foreach (var childItem in item.Product_AttributeChild)
            {
                LoadChildProductAttribute(db, childItem);
            }
        }

        #endregion

        #region ProductGroup
        public List<Product_Group> GetProductGroupList()
        { 
            using (Entities db = new Entities())
            {
                IQueryable<Product_Group> queryable = db.Product_Group 
                    .AsNoTracking();
 
              return queryable.ToList();
            }
        }
        
        #endregion
    }
}
