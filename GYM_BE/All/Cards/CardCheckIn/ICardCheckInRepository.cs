using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;
using GYM_BE.Core.Dto;

namespace GYM_BE.All.CardCheckIn
{
    public interface ICardCheckInRepository: IGenericRepository<CARD_CHECK_IN, CardCheckInDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<CardCheckInDTO> pagination);
        Task<FormatedResponse> CheckIn(string cardCode, string sid);
        Task<FormatedResponse> GetListCardCode();
    }
}

