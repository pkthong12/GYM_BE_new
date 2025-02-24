using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.SysMenu
{
    public interface ISysMenuRepository: IGenericRepository<SYS_MENU, SysMenuDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<SysMenuDTO> pagination);
        Task<FormatedResponse> GetActionByUser(SysUserDTO userDTO);
        Task<FormatedResponse> GetAllAction();
    }
}

