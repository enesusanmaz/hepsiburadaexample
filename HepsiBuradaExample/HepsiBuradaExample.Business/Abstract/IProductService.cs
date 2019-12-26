using HepsiBuradaExample.Services.Dtos.Product;
using System.Collections.Generic;

namespace HepsiBuradaExample.Services.Abstract
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetProducts();
        ProductDTO CreateProduct(ProductDTO productDTO);
        ProductDTO GetProductById(int productId);
        ProductDTO GetProductByCode(string productCode);
    }
}
