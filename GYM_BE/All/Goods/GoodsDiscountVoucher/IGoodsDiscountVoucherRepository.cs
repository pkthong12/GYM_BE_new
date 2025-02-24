using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.GoodsDiscountVoucher
{
    public interface IGoodsDiscountVoucherRepository: IGenericRepository<GOODS_DISCOUNT_VOUCHER, GoodsDiscountVoucherDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GoodsDiscountVoucherDTO> pagination);
    }
}

