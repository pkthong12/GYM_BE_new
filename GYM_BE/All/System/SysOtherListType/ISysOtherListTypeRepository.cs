using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.SysOtherListType
{
    public interface ISysOtherListTypeRepository: IGenericRepository<SYS_OTHER_LIST_TYPE, SysOtherListTypeDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<SysOtherListTypeDTO> pagination);

        Task<FormatedResponse> GetList();
    }
}

