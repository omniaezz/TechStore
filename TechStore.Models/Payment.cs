using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Payment : BaseEntity
    {
        public string? PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionID { get; set; }
        public string? PaymentStatus { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
