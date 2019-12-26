using HepsiBuradaExample.Data.Abstract.Base;
using HepsiBuradaExample.Data.Entities;
using HepsiBuradaExample.Services.Abstract;
using HepsiBuradaExample.Services.Dtos.Campaign;
using HepsiBuradaExample.Services.Helpers;
using HepsiBuradaExample.Services.Helpers.AutoMapper;
using System;
using System.Linq;

namespace HepsiBuradaExample.Services.Concrete
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IAutoMapperService _mapper;
        public CampaignService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CampaignDTO CreateCampaign(CampaignDTO campaignDTO)
        {
            campaignDTO.IsSucessed = false;

            try
            {
                var product = _unitOfWork.ProductRepository.Get(x => x.ProductCode == campaignDTO.ProductCode).FirstOrDefault();

                if (product == null)
                {
                    campaignDTO.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.NotFound, DataType.Product);
                    return campaignDTO;
                }

                campaignDTO.ProductId = product.ProductId;

                //Ürün fiyatı üzerine kampanya indirim yüzdesi uygulanarak ürünün kampanya süresince satış fiyatı belirleniyor.
                campaignDTO.Price = product.UnitPrice - (product.UnitPrice * campaignDTO.DiscountPercent / 100);

                var data = _mapper.Mapper.Map<Campaign>(campaignDTO);
                _unitOfWork.CampaignRepository.Insert(data);
                _unitOfWork.Save();

                campaignDTO.IsSucessed = true;
            }
            catch
            {
                campaignDTO.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return campaignDTO;
        }

        /// <summary>
        /// Süresi bitmiş kampanyaları pasife çeker
        /// </summary>
        /// <returns></returns>
        public CampaignDTO EndCampaignsByEndDate()
        {
            var campaignDTO = new CampaignDTO { IsSucessed = false };
            var now = DateTime.Now;
            try
            {
                var campaigns = _unitOfWork.CampaignRepository.Get(x => x.EndDate >= now && x.IsActive).ToList();

                foreach (var campaign in campaigns)
                {
                    campaign.IsActive = false;
                    _unitOfWork.CampaignRepository.Update(campaign);
                }
                _unitOfWork.Save();
                campaignDTO.IsSucessed = true;
            }
            catch
            {
                campaignDTO.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return campaignDTO;
        }

        public CampaignDTO GetCampaignByName(string CampaignName)
        {
            var dto = new CampaignDTO { IsSucessed = false };

            try
            {
                var data = _unitOfWork.CampaignRepository.Get(x => x.CampaignName == CampaignName).FirstOrDefault();

                if (data == null)
                {
                    dto.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.NotFound, DataType.Campaign);
                    return dto;
                }

                dto = _mapper.Mapper.Map<CampaignDTO>(data);
                dto.IsSucessed = true;
            }
            catch
            {
                dto.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return dto;
        }

        public CampaignDetailDTO GetCampaignDetailByName(string CampaignName)
        {
            var dto = new CampaignDetailDTO { IsSucessed = false};

            try
            {
                var campaign = GetCampaignByName(CampaignName);

                if (campaign != null)
                    _unitOfWork.OrderRepository.Get(x => x.CampaignId == campaign.CampaignId).ToList();
                else
                {
                    dto.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.NotFound, DataType.Campaign);
                    return dto;
                }
            }
            catch (Exception)
            {
                dto.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return dto;
        }

    }
}
