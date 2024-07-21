using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Image:BaseEntity
    {
        [Base64String]
        public string Name { get; set; } 

        [ForeignKey("Product")]
        public int ProductId { get; set; }//**
        public Product Product { get; set; }//**
    }
}
