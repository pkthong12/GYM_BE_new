using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsDiscountVoucher
{
    public class GoodsDiscountVoucherRepository : IGoodsDiscountVoucherRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<GOODS_DISCOUNT_VOUCHER, GoodsDiscountVoucherDTO> _genericRepository;

        public GoodsDiscountVoucherRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_DISCOUNT_VOUCHER, GoodsDiscountVoucherDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsDiscountVoucherDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsDiscountVouchers.AsNoTracking()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.DISCOUNT_TYPE_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.TARGET_CUSTOMERS).DefaultIfEmpty()
                         from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS).DefaultIfEmpty()
                         from s4 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.DISCOUNT_TYPE_ID).DefaultIfEmpty()
                         select new GoodsDiscountVoucherDTO
                        {
                            Id = p.ID,
                            Code = p.CODE,
                            Name = p.NAME,
                            DiscountTypeId = p.DISCOUNT_TYPE_ID,
                            DiscountTypeName = s1.NAME,
                            DicountValue = p.DICOUNT_VALUE,
                            StartDate = p.START_DATE,
                            EndDate = p.END_DATE,
                            StartDateTime = p.START_DATETIME,
                            EndDateTime = p.END_DATETIME,
                            Condition = p.CONDITION,
                            IssueQuantity = p.ISSUE_QUANTITY,
                            UsedQuantity = p.USED_QUANTITY,
                            UsageLimit = p.USAGE_LIMIT,
                            TargetCustomers = p.TARGET_CUSTOMERS,
                            TargetCustomersName = s2.NAME,
                            Status = p.STATUS,
                            StatusName = s3.NAME,
                            Description = p.DESCRIPTION,
                            Note = p.NOTE,
                            DistributionChannel = p.DISCOUNT_TYPE_ID,
                            DistributionChannelName = s4.NAME,
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.DiscountTypeId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.DiscountTypeId == pagination.Filter.DiscountTypeId);
                }
            }
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
         return new FormatedResponse
         {
             InnerBody = respose,
         };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var joined = await (from p in _dbContext.GoodsDiscountVouchers.AsNoTracking().Where(x => x.ID == id)
                                from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.DISCOUNT_TYPE_ID).DefaultIfEmpty()
                                from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.TARGET_CUSTOMERS).DefaultIfEmpty()
                                from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS).DefaultIfEmpty()
                                from s4 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.DISCOUNT_TYPE_ID).DefaultIfEmpty()
                                select new GoodsDiscountVoucherDTO
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    Name = p.NAME,
                                    DiscountTypeId = p.DISCOUNT_TYPE_ID,
                                    DiscountTypeName = s1.NAME,
                                    DicountValue = p.DICOUNT_VALUE,
                                    StartDate = p.START_DATE,
                                    EndDate = p.END_DATE,
                                    StartDateTime = p.START_DATETIME,
                                    EndDateTime = p.END_DATETIME,
                                    Condition = p.CONDITION,
                                    IssueQuantity = p.ISSUE_QUANTITY,
                                    UsedQuantity = p.USED_QUANTITY,
                                    UsageLimit = p.USAGE_LIMIT,
                                    TargetCustomers = p.TARGET_CUSTOMERS,
                                    TargetCustomersName = s2.NAME,
                                    Status = p.STATUS,
                                    StatusName = s3.NAME,
                                    Description = p.DESCRIPTION,
                                    Note = p.NOTE,
                                    DistributionChannel = p.DISCOUNT_TYPE_ID,
                                    DistributionChannelName = s4.NAME,
                                }).FirstOrDefaultAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsDiscountVoucherDTO dto, string sid)
        {
            dto.Code = CreateNewCode();
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsDiscountVoucherDTO> dtos, string sid)
        {
            var add = new List<GoodsDiscountVoucherDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsDiscountVoucherDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsDiscountVoucherDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(long id)
        {
            var response = await _genericRepository.Delete(id);
            return response;
        }

        public async Task<FormatedResponse> Delete(string id)
        {
            var response = await _genericRepository.Delete(id);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(ids);
            return response;
        }

        public async Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.GoodsDiscountVouchers.Count() == 0)
            {
                newCode = "VOUCHER001";
            }
            else
            {
                string lastestData = _dbContext.GoodsDiscountVouchers.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 6) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return newCode;

        }

    }
}

