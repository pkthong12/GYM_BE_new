using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.GoodsEquipment
{
    public interface IGoodsEquipmentRepository: IGenericRepository<GOODS_EQUIPMENT, GoodsEquipmentDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsEquipmentDTO> pagination);

        Task<FormatedResponse> GetListByTypeCode(string typeCode);
        Task<FormatedResponse> GetListByTypeId(long id);
        Task<FormatedResponse> GetList();
    }
}

