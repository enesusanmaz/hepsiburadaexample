using HepsiBuradaExample.Services.Dtos.Order;
using System.Collections.Generic;

namespace HepsiBuradaExample.Services.Abstract
{
    public interface IOrderService
    {
        public OrderDTO CreateOrder(OrderDTO orderDTO);
        public List<OrderDTO> GetOrderListByCampaignId(int campaignId);

    }
}
