using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc_Apteka
{

    /// <summary>
    /// Контекст EFCore 
    /// </summary>
    public class AppDbContext : DbContext
    {
        public virtual DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public virtual DbSet<ProductInfo> ProductInfos { get; set; }
        public virtual DbSet<ProductActivity> Activities { get; set; }
        
        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
       
        }

        /// <summary>
        /// Обнаружение изменений сведений о продукции на складе,
        /// при обнаружении выполняется запись в журнал
        /// </summary>
        public void BeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();         
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if(entry.Entity.ToString() == typeof(Entities.ProductInfo).FullName)
                {
                    var activity = new ProductActivity();
                    bool save = false;
                    foreach (PropertyEntry property in entry.Properties)
                    {
                        if (property.IsModified)
                        {
                            
                            Console.WriteLine(property.OriginalValue + "=>" + property.CurrentValue);
                            switch (property.Metadata.Name)
                            {
                                case nameof(ProductInfo.ProductCount):
                                    save = true;
                                    activity.ProductCount = (int)property.CurrentValue;
                                    activity.ProductCountDev = (int)property.CurrentValue - (int)property.OriginalValue;
                                    break;
                                case nameof(ProductInfo.ProductPrice):
                                    save = true;
                                    activity.ProductPrice = (float)property.CurrentValue;
                                    activity.ProductPriceDev = (float)property.CurrentValue - (float)property.OriginalValue;
                                    break;
                            }

                        }
                        switch (property.Metadata.Name)
                        {
                            case nameof(ProductInfo.ProductName):
                                activity.ProductName = (string)property.CurrentValue;                                
                                break;
                            case nameof(ProductInfo.ID):
                                activity.ProductID = (int)property.CurrentValue;                                
                                break;
                        }
                    }
                    if(save)
                        this.Activities.Add(activity);
                }                                
            }
        }


        /// <summary>
        /// Фиксация изменений 
        /// </summary>
        public override int SaveChanges()
        {
            this.BeforeSaveChanges();
            return base.SaveChanges();
        }



        /// <summary>
        /// Фиксация изменений 
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {           
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }



        public IQueryable<ProductInfo> ProductsSearch(IQueryable<ProductInfo> products, int minCount, int maxCount, float minPrice, float maxPrice)
        {
            var productsInCountingRange = ProductCountInRange(products, minCount, maxCount);
            var productsInPriceRange = ProductPriceInRange(productsInCountingRange, minPrice, maxPrice);            
            return productsInPriceRange;
        }

        public IQueryable<ProductInfo> ProductCountInRange(IQueryable<ProductInfo> products, int min, int max)
            => products.Where(p => p.ProductCount >= min && p.ProductCount <= max);

        public IQueryable<ProductInfo> ProductPriceInRange(IQueryable<ProductInfo> products, float min, float max)
            => products.Where(p => p.ProductPrice >= min && p.ProductPrice <= max);

        public IQueryable<ProductInfo> ProductCountInRange(int min, int max)
            => this.ProductCountInRange(this.ProductInfos, min, max);

        public IQueryable<ProductInfo> ProductPriceInRange(float min, float max)
            => this.ProductPriceInRange(this.ProductInfos, min, max);

        public bool HasProductWithName(string Name)
            => this.ProductInfos.Any(p => p.ProductName.ToUpper() == Name.ToUpper());

        public ProductCatalog GetProductCatalog(string ProductCatalogName)
            => this.ProductCatalogs.Where(p => p.ProductCatalogName == ProductCatalogName).FirstOrDefault();

        public ProductInfo GetProductInfo(string ProductName)
            => this.ProductInfos.Where(p => p.ProductName == ProductName).FirstOrDefault();


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

        public bool UpdateProductInfo(string productName, float productPrice, int productCount)
        {
            ProductInfo p = this.GetProductInfo(productName);
            if (Equals(p, productName, productPrice, productCount) == false)
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
