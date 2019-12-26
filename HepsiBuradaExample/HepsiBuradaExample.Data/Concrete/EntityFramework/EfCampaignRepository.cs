using HepsiBuradaExample.Data.Abstract;
using HepsiBuradaExample.Data.Entities;
using System.Collections.Generic;

namespace HepsiBuradaExample.Data.Concrete.EntityFramework
{
    public class EfCampaignRepository : EfBaseRepository<Campaign>, ICampaignRepository
    {
        public EfCampaignRepository(HepsiBuradaExampleContext context) : base(context)
        {

        }


    }
    
}
