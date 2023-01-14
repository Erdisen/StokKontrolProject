﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Entities.Entities
{
    public class OrderDetails : BaseEntity
    {

        [ForeignKey("Siparis")]
        public int OrderID { get; set; }
        [ForeignKey("Urun")]
        public int ProductID { get; set; }

        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }



        //Navigation Properties

        public virtual Product Urun { get; set; }
        public virtual Order Siparis { get; set; }
    }
}
