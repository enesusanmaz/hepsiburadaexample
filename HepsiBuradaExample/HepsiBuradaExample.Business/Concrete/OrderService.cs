using HepsiBuradaExample.Data.Abstract.Base;
using HepsiBuradaExample.Data.Entities;
using HepsiBuradaExample.Services.Abstract;
using HepsiBuradaExample.Services.Dtos.Order;
using HepsiBuradaExample.Services.Helpers;
using HepsiBuradaExample.Services.Helpers.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HepsiBuradaExample.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IAutoMapperService _mapper;
        public OrderService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public OrderDTO CreateOrder(OrderDTO orderDTO)
        {
            orderDTO.IsSucessed = false;

            try
            {
                var product = _unitOfWork.ProductRepository.Get(x => x.ProductCode == orderDTO.ProductCode).FirstOrDefault();

                //Sipariş geçilen ilgili ürün var mı?

                if (product == null)
                {
                    orderDTO.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.NotFound, DataType.Order);
                    return orderDTO;
                }

                var campaign =_unitOfWork.CampaignRepository.Get(x => x.ProductId == product.ProductId && x.IsActive).FirstOrDefault();
                // Sipariş geçilmek istenilen ürün ile ilgili aktif bir kampanya var mı?
                if (campaign == null)
                {
                    //Kampanya yoksa ürünün fiyatını al

                    orderDTO.UnitPrice = product.UnitPrice;
                    orderDTO.TotalPrice = product.UnitPrice * orderDTO.Quantity;
                }
                else // Kampanya varsa siparişe kampanya fiyatını uygula
                {
                    orderDTO.UnitPrice = campaign.Price;
                    orderDTO.TotalPrice = campaign.Price * orderDTO.Quantity;
                    orderDTO.Discount = campaign.DiscountPercent;
                    orderDTO.CampaignId = campaign.CampaignId;
                }

                orderDTO.ProductId = product.ProductId;

                var data = _mapper.Mapper.Map<Order>(orderDTO);
                _unitOfWork.OrderRepository.Insert(data);

                //Siparişi geçilen ürünün stok bilgisini güncelle

                product.UnitsInStock = product.UnitsInStock - orderDTO.Quantity;
                _unitOfWork.ProductRepository.Update(product);

                _unitOfWork.Save();

                orderDTO.IsSucessed = true;
            }
            catch(Exception ex)
            {
                orderDTO.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return orderDTO;
        }

        public List<OrderDTO> GetOrderListByCampaignId(int campaignId)
        {
            var data = _unitOfWork.OrderRepository.Get(x=>x.CampaignId == campaignId).ToList();
            return _mapper.Mapper.Map<List<OrderDTO>>(data);
        }
    }
}
