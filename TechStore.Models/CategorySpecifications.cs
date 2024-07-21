using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class CategorySpecifications:BaseEntity
    {
        [ForeignKey("Category")]
        public int? CategoryId {  get; set; } 
        public Category? Category { get; set; }


        [ForeignKey("Specification")]
        public int? SpecificationId { get; set; }
        public Specification? Specification { get; set; }
    }
}
