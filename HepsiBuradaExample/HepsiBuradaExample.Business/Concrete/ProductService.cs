using HepsiBuradaExample.Data.Abstract.Base;
using HepsiBuradaExample.Data.Entities;
using HepsiBuradaExample.Services.Abstract;
using HepsiBuradaExample.Services.Dtos;
using HepsiBuradaExample.Services.Dtos.Product;
using HepsiBuradaExample.Services.Helpers;
using HepsiBuradaExample.Services.Helpers.AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace HepsiBuradaExample.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IAutoMapperService _mapper;
        public ProductService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductDTO GetProductByCode(string productCode)
        {
            var dto = new ProductDTO { IsSucessed = false };

            try
            {
                var data = _unitOfWork.ProductRepository.Get(x => x.ProductCode == productCode).FirstOrDefault();

                if (data == null)
                {
                    dto.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.NotFound, DataType.Product);
                    return dto;
                }

                dto = _mapper.Mapper.Map<ProductDTO>(data);
                dto.IsSucessed = true;
            }
            catch
            {
                dto.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return dto;
        }

        public ProductDTO GetProductById(int productId)
        {
            var data = _unitOfWork.ProductRepository.GetById(productId);
            return _mapper.Mapper.Map<ProductDTO>(data);
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            var data = _unitOfWork.ProductRepository.Get();
            return _mapper.Mapper.Map<IEnumerable<ProductDTO>>(data);
        }
        public ProductDTO CreateProduct(ProductDTO productDTO)
        {
            try
            {
                var data = _mapper.Mapper.Map<Product>(productDTO);
                _unitOfWork.ProductRepository.Insert(data);
                _unitOfWork.Save();
                productDTO.IsSucessed = true;
            }
            catch 
            {
                productDTO.IsSucessed = false;
                productDTO.ServiceMessage = ServiceMessageHelper.GetExceptionMessage(ErrorType.UnexpectedError);
            }

            return productDTO;
        }
    }
}
