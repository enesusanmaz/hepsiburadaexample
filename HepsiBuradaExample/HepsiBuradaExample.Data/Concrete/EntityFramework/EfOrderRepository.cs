using System.Collections.Generic;
using HepsiBuradaExample.Data.Abstract;
using HepsiBuradaExample.Data.Entities;

namespace HepsiBuradaExample.Data.Concrete.EntityFramework
{
    public class EfOrderRepository : EfBaseRepository<Order>, IOrderRepository
    {
        public EfOrderRepository(HepsiBuradaExampleContext context) : base(context)
        {

        }


    }
    
}
