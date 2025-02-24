using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.CardHistory
{
    public interface ICardHistoryRepository: IGenericRepository<CARD_HISTORY, CardHistoryDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<CardHistoryDTO> pagination);
        Task<FormatedResponse> GetListCardCode();

    }
}

