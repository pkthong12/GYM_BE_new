using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.CardInfo
{
    public interface ICardInfoRepository: IGenericRepository<CARD_INFO, CardInfoDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<CardInfoDTO> pagination);
        Task<FormatedResponse> GetListCustomer();
        Task<FormatedResponse> GetAllCardValid(long? id);
        Task<FormatedResponse> CalculateByCardId(long? id);
        Task<FormatedResponse> DeleteNew(long id, string sid);
        Task<FormatedResponse> DeleteIdsNew(List<long> ids, string sid);
        Task<FormatedResponse> GetCardInfoPortal(string code);
    }
}

