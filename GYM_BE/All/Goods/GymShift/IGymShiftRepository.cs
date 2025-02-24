using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.Gym.GymShift
{
    public interface IGymShiftRepository : IGenericRepository<GOODS_SHIFT, GoodsShiftDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsShiftDTO> pagination);

        Task<FormatedResponse> GetList();

    }
}
