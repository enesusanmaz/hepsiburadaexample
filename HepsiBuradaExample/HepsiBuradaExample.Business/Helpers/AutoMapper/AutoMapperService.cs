using AutoMapper;

namespace HepsiBuradaExample.Services.Helpers.AutoMapper
{
    public class AutoMapperService : IAutoMapperService
    {
        public IMapper Mapper
        {
            get { return ObjectMapper.Mapper; }
        }
    }
}
