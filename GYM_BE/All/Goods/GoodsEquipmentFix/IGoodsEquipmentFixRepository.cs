using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.GoodsEquipmentFix
{
    public interface IGoodsEquipmentFixRepository: IGenericRepository<GOODS_EQUIPMENT_FIX, GoodsEquipmentFixDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsEquipmentFixDTO> pagination);
    }
}

