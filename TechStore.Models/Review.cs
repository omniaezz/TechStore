using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Review :BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime? ReviewDate { get; set; } = DateTime.Now;

        public string? UserId { get; set; }
        public TechUser? User { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
