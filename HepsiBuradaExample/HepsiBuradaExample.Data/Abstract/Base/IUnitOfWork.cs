using System;

namespace HepsiBuradaExample.Data.Abstract.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICampaignRepository CampaignRepository { get; }
        void Save();
    }
}
