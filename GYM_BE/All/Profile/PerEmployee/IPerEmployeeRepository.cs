using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.PerEmployee
{
    public interface IPerEmployeeRepository: IGenericRepository<PER_EMPLOYEE, PerEmployeeDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<PerEmployeeDTO> pagination);

        byte[] ExportExcelPerEmployee();
    }
}

