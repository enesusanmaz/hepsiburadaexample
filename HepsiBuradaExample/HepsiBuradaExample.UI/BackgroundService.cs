using HepsiBuradaExample.Services.Abstract;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using HepsiBuradaExample.Services.Dtos;
using HepsiBuradaExample.Services.Dtos.Order;

namespace HepsiBuradaExample.UI
{
    public class BackgroundService : IHostedService, IDisposable
    {
        private Timer _timerOrderManager;
        private Timer _timerCampaignManager;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timerOrderManager = new Timer(PurchaseOrderRequest,null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15)); //her saniye dakikayı temsil etmektedir.sisteme 1 saniye aralıklarla sipariş geliyor

            _timerCampaignManager = new Timer(EndCampaignsByEndDate, null, TimeSpan.Zero,
    TimeSpan.FromMinutes(1)); // her dakika aslında saati temsil ediyor. Saat başı süresi geçmiş kampanyaların iptali gerçekleşiyor.

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timerOrderManager?.Change(Timeout.Infinite, 0);
            _timerCampaignManager?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerOrderManager?.Dispose();
            _timerCampaignManager?.Dispose();
        }

        private void PurchaseOrderRequest(object state)
        {
            Random random = new Random();

            var orderDTO = new OrderDTO
            {
                ProductCode = "P" + random.Next(1, 77).ToString(),
                Quantity = random.Next(1, 20),
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            var orderService = Startup.ServiceProvider.GetService<IOrderService>();
            var serviceResult = orderService.CreateOrder(orderDTO);

            if (serviceResult.IsSucessed)
                Console.WriteLine($"Order is created; Product Code : {serviceResult.ProductCode} , Quantity : {serviceResult.Quantity}");

        }

        // Sistem her dakika (saat) ilgili günün kampanyalarını kontrol ediyor. Süresi geçmiş kampanyaları sonlandırıyor.
        private void EndCampaignsByEndDate(object state)
        {
            var campaignService = Startup.ServiceProvider.GetService<ICampaignService>();
            var serviceResult = campaignService.EndCampaignsByEndDate();
        }

    }
}
