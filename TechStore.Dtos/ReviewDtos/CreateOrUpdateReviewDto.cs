using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ReviewDtos
{
    public class CreateOrUpdateReviewDto
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Rating is Required")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is Required")]
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }



    }
}
