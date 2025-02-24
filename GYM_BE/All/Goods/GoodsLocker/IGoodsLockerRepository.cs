using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.GoodsLocker
{
    public interface IGoodsLockerRepository: IGenericRepository<GOODS_LOCKER, GoodsLockerDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsLockerDTO> pagination);
        Task<FormatedResponse> GetAllLockerValid(long? id, long? genderId, long cardId);
        Task<FormatedResponse> GetLockerStatus(long area);
    }
}

