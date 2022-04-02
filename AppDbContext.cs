using Microsoft.EntityFrameworkCore;

using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc_Apteka
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public virtual DbSet<ProductInfo> ProductInfos { get; set; }

        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public IQueryable<ProductInfo> ProductsSearch(IQueryable<ProductInfo> products, int minCount, int maxCount, float minPrice, float maxPrice)
        {
            var productsInCountingRange = ProductCountInRange(products, minCount, maxCount);
            var productsInPriceRange = ProductPriceInRange(products, minPrice, maxPrice);
            return products.Where(p =>
                productsInCountingRange.Select(item => item.ID).Contains(p.ID) &&
                productsInPriceRange.Select(item => item.ID).Contains(p.ID)
            );
        }
        public IQueryable<ProductInfo> ProductCountInRange(IQueryable<ProductInfo> products, int min, int max)
            => products.Where(p => p.ProductCount >= min && p.ProductCount <= max);

        public IQueryable<ProductInfo> ProductPriceInRange(IQueryable<ProductInfo> products, float min, float max)
            => products.Where(p => p.ProductPrice >= min && p.ProductCount <= max);

        public IQueryable<ProductInfo> ProductCountInRange(int min, int max)
            => this.ProductCountInRange(this.ProductInfos, min, max);

        public IQueryable<ProductInfo> ProductPriceInRange(float min, float max)
            => this.ProductPriceInRange(this.ProductInfos, min, max);


        public void AddOrUpdate(ProductCatalog TargetCatalog)
        {
            var CurrentCatalog = this.ProductCatalogs.Where(p => p.ProductCatalogName == TargetCatalog.ProductCatalogName).FirstOrDefault();
            if( CurrentCatalog == null )
            {
                this.ProductCatalogs.Add(TargetCatalog);
                this.SaveChanges();
            }
            else
            {
 
                var CurrentProducts = this.ProductInfos.Where(p => p.ProductCatalogID == TargetCatalog.ID);
                var TargetProducts = TargetCatalog.Products;

                HashSet<string> CurrentProductNames = CurrentProducts.Select(p => p.ProductName).ToHashSet();
                HashSet<string> TargetProductNames = TargetProducts.Select(p => p.ProductName).ToHashSet();
                
                HashSet<string> CurrentExpectTarget = new HashSet<string>();
                HashSet<string> TargetExpectCurrent = new HashSet<string>();
                HashSet<string> TargetInspectCurrent = new HashSet<string>();
                foreach (var ProductName in TargetProductNames.Intersect(CurrentProductNames))
                    TargetInspectCurrent.Add(ProductName);
                foreach (var ProductName in TargetProductNames.Except(CurrentProductNames))
                    TargetExpectCurrent.Add(ProductName);
                foreach (var ProductName in CurrentProductNames.Except(TargetProductNames))
                    CurrentExpectTarget.Add(ProductName);


                /// удаляем записи которых нет в текущем наборе
                foreach (var Product in TargetProducts.Where(p => TargetExpectCurrent.Contains(p.ProductName)).ToList())
                    this.ProductInfos.Add(Product);
                int ProductsAdded = this.SaveChanges();

                /// удаляем записи которых нет в целевом наборе
                foreach (var Product in CurrentProducts.Where(p => CurrentExpectTarget.Contains(p.ProductName)).ToList())
                    this.ProductInfos.Remove(Product);
                int ProductsRemoved = this.SaveChanges();

                int ProductsUpdated = 0;
                /// остальные записи сравниваем и обновляем
                foreach (var Product in CurrentProducts.Where(p => TargetInspectCurrent.Contains(p.ProductName)).ToList())
                {
                    var TargetProduct = TargetProducts.Where(p => p.ProductName == Product.ProductName).First();
                    if (this.UpdateProductInfo(Product.ProductName, TargetProduct.ProductPrice, TargetProduct.ProductCount))
                        ProductsUpdated++;
                }
                    
                


            }
        }

        public ProductCatalog GetProductCatalog(string ProductCatalogName)
            => this.ProductCatalogs.Where(p => p.ProductCatalogName == ProductCatalogName).FirstOrDefault();

        public ProductInfo GetProductInfo(string ProductName)
            => this.ProductInfos.Where(p => p.ProductName == ProductName).FirstOrDefault();

        public bool UpdateProductInfo(string productName, float productPrice, float productCount)
        {
            ProductInfo p = this.GetProductInfo(productName);
            if( Equals(p, productName, productPrice, productCount)==false )
            {
                p.ProductName = productName;
                p.ProductCount = productCount;
                p.ProductPrice = productPrice;
            }
            return true;
        }

        public bool Equals(ProductInfo ProductInfo, string ProductName, float ProductPrice, float ProductCount)
        =>
            ProductInfo.ProductName != ProductName ? false :
            ProductInfo.ProductPrice != ProductPrice ? false :
            ProductInfo.ProductCount != ProductCount ? false : true;

    }
}
