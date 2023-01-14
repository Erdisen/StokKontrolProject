using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Entities.Entities
{
    public class Category : BaseEntity
    {
        public Category() // Product çağırdığımızda çakışmaması için
        {
            Products = new List<Product>();
        }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        //Navgigation property
        //Bir kategorinin birden fazla ürünü olabilir.

        public virtual List<Product> Products { get; set; }

    }
}
