using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Dtos.ReviewDtos
{
    public class GetAllReviewDto
    {
        public int Id { get; set; }
        //  public int TechUserId { get; set; }
        public int? ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        //  public bool? IsDeleted { get; set; }
        public TechUser User { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}
