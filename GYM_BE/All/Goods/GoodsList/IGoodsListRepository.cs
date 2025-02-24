using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.GoodsList
{
    public interface IGoodsListRepository: IGenericRepository<GOODS_LIST, GoodsListDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsListDTO> pagination);
    }
}

