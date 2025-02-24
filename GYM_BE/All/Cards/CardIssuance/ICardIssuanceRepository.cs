using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.CardIssuance
{
    public interface ICardIssuanceRepository: IGenericRepository<CARD_ISSUANCE, CardIssuanceDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<CardIssuanceDTO> pagination);
    }
}

