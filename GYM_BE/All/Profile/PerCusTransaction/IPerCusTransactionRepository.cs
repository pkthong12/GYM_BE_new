using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.PerCusTransaction
{
    public interface IPerCusTransactionRepository: IGenericRepository<PER_CUS_TRANSACTION, PerCusTransactionDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<PerCusTransactionDTO> pagination);
    }
}

