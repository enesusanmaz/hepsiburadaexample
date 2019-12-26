using HepsiBuradaExample.Dtos;
using System;

namespace HepsiBuradaExample.Services.Dtos.Campaign
{
    public class CampaignDTO: BaseDTO
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public int TargetSalesCount { get; set; }
        public decimal Limit { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public string Status => IsActive ? "Active" : "Passive";
    }
}
