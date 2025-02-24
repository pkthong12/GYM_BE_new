using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.PerCusListCard
{
    public interface IPerCusListCardRepository: IGenericRepository<PER_CUS_LIST_CARD, PerCusListCardDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<PerCusListCardDTO> pagination);
    }
}

