using HepsiBuradaExample.Data;
using HepsiBuradaExample.Data.Abstract.Base;
using HepsiBuradaExample.Data.Concrete.EntityFramework;
using HepsiBuradaExample.Services.Abstract;
using HepsiBuradaExample.Services.Concrete;
using HepsiBuradaExample.Services.Helpers.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HepsiBuradaExample.UI
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider;
        public static void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<HepsiBuradaExampleContext>(options =>
            {
                options.UseSqlServer(GetDbConnectionByName("HepsiBuradaExampleConn"));
            });

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IAutoMapperService, AutoMapperService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICampaignService, CampaignService>();

            // arkaplanda çalışacak hosted service register
            services.AddHostedService<BackgroundService>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public static void DisposeServices()
        {
            if (ServiceProvider == null)
            {
                return;
            }
            if (ServiceProvider is IDisposable)
            {
                ((IDisposable)ServiceProvider).Dispose();
            }
        }

        private static string GetDbConnectionByName(string ConnectionName)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            return configuration.GetConnectionString(ConnectionName);
        }
    }
}
