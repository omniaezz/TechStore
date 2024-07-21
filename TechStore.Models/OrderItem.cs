﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class OrderItem :BaseEntity
    {
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice => Quantity * UnitPrice; 

        // Navigation properties
        public int? OrderId { get; set; } 
        public Order? Order { get; set; }
        public int ProductId { get; set; } 
        public Product? Product { get; set; }
    }
}
