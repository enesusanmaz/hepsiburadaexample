using HepsiBuradaExample.Dtos;

namespace HepsiBuradaExample.Services.Dtos.Product
{
    public class ProductDTO: BaseDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
