using System.ComponentModel.DataAnnotations;

namespace Mvc_Apteka.Entities
{
    public class ProductInfo
    {
        [Key]
        public int ID { get; set; }
        public int ProductCatalogID { get; set; }        

        public string ProductName { get; set; }
        public float ProductCount { get; set; }
        public float ProductPrice { get; set; }
    }
}
