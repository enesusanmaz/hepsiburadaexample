using HepsiBuradaExample.Data.Abstract;
using HepsiBuradaExample.Data.Entities;

namespace HepsiBuradaExample.Data.Concrete.EntityFramework
{
    public class EfProductRepository : EfBaseRepository<Product>, IProductRepository
    {
        public EfProductRepository(HepsiBuradaExampleContext context) : base(context)
        {

        }


    }
}
