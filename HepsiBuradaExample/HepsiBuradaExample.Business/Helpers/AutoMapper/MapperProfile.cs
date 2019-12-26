using AutoMapper;
using HepsiBuradaExample.Data.Entities;
using HepsiBuradaExample.Services.Dtos.Campaign;
using HepsiBuradaExample.Services.Dtos.Order;
using HepsiBuradaExample.Services.Dtos.Product;
using System;

namespace HepsiBuradaExample.Services.Helpers.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<Campaign, CampaignDTO>();
            CreateMap<CampaignDTO, Campaign>();
        }
    }

    public class ObjectMapper
    {
        public static IMapper Mapper
        {
            get { return mapper.Value; }
        }

        public static IConfigurationProvider Configuration
        {
            get { return config.Value; }
        }

        public static Lazy<IMapper> mapper = new Lazy<IMapper>(() =>
        {
            var mapper = new Mapper(Configuration);
            return mapper;
        });

        public static Lazy<IConfigurationProvider> config = new Lazy<IConfigurationProvider>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            return config;
        });
    }
}
