using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.Profile.PerCustomer
{
    public interface IPerCustomerRepository : IGenericRepository<PER_CUSTOMER, PerCustomerDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<PerCustomerDTO> pagination);
        Task<FormatedResponse> GetAllCustomer();

        byte[] ExportExcelPerCustomer();
    }
}

