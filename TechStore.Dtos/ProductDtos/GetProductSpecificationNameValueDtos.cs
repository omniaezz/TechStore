using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class GetProductSpecificationNameValueDtos
    {
        public GetAllProductsDtos productsDtos { get; set; }

        public List<GetSpecificationsNameValueDtos> specificationsNameValueDtos { get; set;}
    }
}
