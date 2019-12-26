using HepsiBuradaExample.Services.Dtos.Campaign;

namespace HepsiBuradaExample.Services.Abstract
{
    public interface ICampaignService
    {
        CampaignDTO GetCampaignByName(string CampaignName);
        CampaignDTO CreateCampaign(CampaignDTO Campaign);
        CampaignDTO EndCampaignsByEndDate();
        CampaignDetailDTO GetCampaignDetailByName(string CampaignName);
    }
}
