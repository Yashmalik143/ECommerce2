using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class AllCategoriesDTO
    {
        public int ID { get; set; }
        public string categoryName { get; set; }

        public List<ProductDTO> CatProduct { get; set; }
    }
}