using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mvc_Apteka
{

    /// <summary>
    /// Контекст EFCore 
    /// </summary>
    public class AppDbContext : DbContext, IMiddleware
    {
        [NotMapped]
        public string ConnectionString { get; set; }

        public virtual DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public virtual DbSet<ProductInfo> ProductInfos { get; set; }
        public virtual DbSet<ProductActivity> Activities { get; set; }
        
        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
             
        }

    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine(ConnectionString);
            optionsBuilder.UseSqlServer(ConnectionString);
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
                    if (this.AddOrUpdate(Product.ProductName, TargetProduct.ProductPrice, TargetProduct.ProductCount))
                        ProductsUpdated++;
                }                                    
            }
        }

       

        public bool AddOrUpdate(string productName, float productPrice, int productCount)
        {
            ProductInfo p = this.GetProductInfo(productName);
            if(p == null)
            {
                ProductInfos.Add(new ProductInfo()
                {
                    ProductName = productName,
                    ProductCount = productCount,
                    ProductPrice = productPrice
                });
            }
            else
            {
                if (Equals(p, productName, productPrice, productCount) == false)
                {
                    p.ProductName = productName;
                    p.ProductCount = productCount;
                    p.ProductPrice = productPrice;
                }
            }
            
            SaveChanges();
            return true;
        }
        public bool AddOrUpdate(ProductInfo info)
        {
            return AddOrUpdate(info.ProductName, info.ProductPrice, info.ProductCount);
        }

        public bool Equals(ProductInfo ProductInfo, string ProductName, float ProductPrice, float ProductCount)
        =>
            ProductInfo.ProductName != ProductName ? false :
            ProductInfo.ProductPrice != ProductPrice ? false :
            ProductInfo.ProductCount != ProductCount ? false : true;

        public void Clear()
        {
            foreach (var act in this.Activities.ToList())
                this.Remove(act);
            foreach (var info in this.ProductInfos.ToList())
                this.Remove(info);
            foreach (var catalog in this.ProductCatalogs.ToList())
                this.Remove(catalog);
            SaveChanges();
        }


        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);


        public void InfoDialog(string title, string text)
        {
            MessageBox((IntPtr)0, text, title, 0);
        }

        private static int code = 1;
        public void OpenCodeEditor( string filepath)
        {
            StartProcess("x"+ code++, $"vscode {filepath}");
        }


        public string StartProcess(string code, string command)
        {
            string batFilePath, outputFilePath = "";
            System.IO.File.WriteAllText((batFilePath=(code + ".bat")), command + " > " + (outputFilePath=(code + ".txt")));


            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + batFilePath);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
          
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            process.WaitForExit();
            string result = System.IO.File.ReadAllText(outputFilePath);
            return result;
        }


        public event EventHandler<DataReceivedEventArgs> Output = (sender, evt) => { 

        };

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {   await next.Invoke(context);
            }
            catch (Exception ex)
            {
                
                //MessageBox((IntPtr)0, ex.Source, ex.Message, 0);
            }
        }
    }
}
