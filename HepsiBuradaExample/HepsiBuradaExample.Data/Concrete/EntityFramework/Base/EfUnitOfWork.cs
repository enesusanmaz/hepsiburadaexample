using HepsiBuradaExample.Data.Abstract;
using HepsiBuradaExample.Data.Abstract.Base;
using HepsiBuradaExample.Data.Entities;
using System;

namespace HepsiBuradaExample.Data.Concrete.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {

        private HepsiBuradaExampleContext _dbContext;
        private EfCampaignRepository _campaigns;
        private EfOrderRepository _orders;
        private EfProductRepository _products;

        public EfUnitOfWork(HepsiBuradaExampleContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IProductRepository ProductRepository
        {
            get
            {
                return _products ??
                    (_products = new EfProductRepository(_dbContext));
            }
        }

        public ICampaignRepository CampaignRepository
        {
            get
            {
                return _campaigns ??
                    (_campaigns = new EfCampaignRepository(_dbContext));
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orders ??
                    (_orders = new EfOrderRepository(_dbContext));
            }
        }

        public void Save()
        {

            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                //TODO:Exception Handling
                //TODO:Logging
            }
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
