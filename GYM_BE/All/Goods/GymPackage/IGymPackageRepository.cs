using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.Gym.GymPackage
{
    public interface IGymPackageRepository : IGenericRepository<GOODS_PACKAGE, GoodsPackageDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsPackageDTO> pagination);

    }
}
