using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.CardIssuance
{
    public class CardIssuanceRepository : ICardIssuanceRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<CARD_ISSUANCE, CardIssuanceDTO> _genericRepository;
        private readonly GenericRepository<ORD_BILL, OrdBillDTO> _genericOrdBillRepository;

        public CardIssuanceRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<CARD_ISSUANCE, CardIssuanceDTO>(_dbContext);
            _genericOrdBillRepository = new GenericRepository<ORD_BILL, OrdBillDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<CardIssuanceDTO> pagination)
        {
            var joined = from p in _dbContext.CardIssuances.AsNoTracking()

                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.PER_SELL_ID).DefaultIfEmpty()
                         from e1 in _dbContext.PerEmployees.AsNoTracking().Where(e1 => e1.ID == p.PER_PT_ID).DefaultIfEmpty()
                         from c in _dbContext.CardInfos.AsNoTracking().Where(c => c.ID == p.CARD_ID).DefaultIfEmpty()
                         from sh in _dbContext.GoodsShifts.AsNoTracking().Where(sh => sh.ID == c.SHIFT_ID).DefaultIfEmpty()
                         from cu in _dbContext.PerCustomers.AsNoTracking().Where(cu => cu.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                         from l in _dbContext.GoodsLockers.AsNoTracking().Where(l => l.ID == p.LOCKER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysUsers.AsNoTracking().Where(s => s.ID == p.CREATED_BY).DefaultIfEmpty()
                         from ot in _dbContext.SysOtherLists.AsNoTracking().Where(ot => ot.ID == c.CARD_TYPE_ID).DefaultIfEmpty()
                             //tuy chinh
                         select new CardIssuanceDTO
                         {
                             Id = p.ID,
                             DocumentNumber = p.DOCUMENT_NUMBER,
                             DocumentDate = p.DOCUMENT_DATE,
                             AfterDiscount = p.AFTER_DISCOUNT,
                             CardId = p.CARD_ID,
                             CardPrice = p.CARD_PRICE,
                             CustomerId = p.CUSTOMER_ID,
                             CustomerName = cu.FULL_NAME,
                             CustomerCode = cu.CODE,
                             CardCode = c.CODE,
                             StartDate = c.EFFECTED_DATE,
                             EndDate = c.EXPIRED_DATE,
                             CardTypeName = ot.NAME,
                             PracticeTime = sh.HOURS_START + " - " + sh.HOURS_END,
                             LockerCode = c.CODE,
                             PerPtName = e1.FULL_NAME,
                             CreatedByUsername = s.USERNAME,
                             HourCard = p.HOUR_CARD,
                             HourCardBonus = p.HOUR_CARD_BONUS,
                             IsHavePt = p.IS_HAVE_PT,
                             IsRealPrice = p.IS_REAL_PRICE,
                             LockerId = p.LOCKER_ID,
                             MoneyHavePay = p.MONEY_HAVE_PAY,
                             PaidMoney = p.MONEY_HAVE_PAY,
                             PercentDiscount = p.PERCENT_DISCOUNT,
                             PercentVat = p.PERCENT_VAT,
                             PerPtId = p.PER_PT_ID,
                             PerSellId = p.PER_SELL_ID,
                             TotalHourCard = p.TOTAL_HOUR_CARD,
                             TotalPrice = p.TOTAL_PRICE,
                             Wardrobe = p.WARDROBE,
                             Note = p.NOTE,
                             IsExpired = DateTime.Now.Date < c.EXPIRED_DATE_TIME!.Value.Date ? false:true,
                         };
            if(pagination.Filter != null)
            {
                if(pagination.Filter.CustomerId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.CustomerId == pagination.Filter.CustomerId);
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
            var joined = await (from p in _dbContext.CardIssuances.AsNoTracking().Where(p => p.ID == id)
                                from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.PER_SELL_ID).DefaultIfEmpty()
                                from e1 in _dbContext.PerEmployees.AsNoTracking().Where(e1 => e1.ID == p.PER_PT_ID).DefaultIfEmpty()
                                from c in _dbContext.CardInfos.AsNoTracking().Where(c => c.ID == p.CARD_ID).DefaultIfEmpty()
                                from sh in _dbContext.GoodsShifts.AsNoTracking().Where(sh => sh.ID == c.SHIFT_ID).DefaultIfEmpty()
                                from cu in _dbContext.PerCustomers.AsNoTracking().Where(cu => cu.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                                from l in _dbContext.GoodsLockers.AsNoTracking().Where(l => l.ID == p.LOCKER_ID).DefaultIfEmpty()
                                from s in _dbContext.SysUsers.AsNoTracking().Where(s => s.ID == p.CREATED_BY).DefaultIfEmpty()
                                    //tuy chinh
                                select new CardIssuanceDTO
                                {
                                    Id = p.ID,
                                    DocumentNumber = p.DOCUMENT_NUMBER,
                                    DocumentDate = p.DOCUMENT_DATE,
                                    AfterDiscount = p.AFTER_DISCOUNT,
                                    CardId = p.CARD_ID,
                                    CardPrice = p.CARD_PRICE,
                                    CustomerId = p.CUSTOMER_ID,
                                    CustomerName = cu.FULL_NAME,
                                    CustomerCode = cu.CODE,
                                    CardCode = c.CODE,
                                    LockerCode = c.CODE,
                                    PerPtName = e1.FULL_NAME,
                                    CreatedByUsername = s.USERNAME,
                                    HourCard = p.HOUR_CARD,
                                    HourCardBonus = p.HOUR_CARD_BONUS,
                                    PracticeTime = sh.HOURS_START + " - " + sh.HOURS_END,
                                    StartDate = c.EFFECTED_DATE,
                                    EndDate = c.EXPIRED_DATE,
                                    IsHavePt = p.IS_HAVE_PT,
                                    IsRealPrice = p.IS_REAL_PRICE,
                                    LockerId = p.LOCKER_ID,
                                    MoneyHavePay = p.MONEY_HAVE_PAY,
                                    PaidMoney = p.MONEY_HAVE_PAY,
                                    PercentDiscount = p.PERCENT_DISCOUNT,
                                    PercentVat = p.PERCENT_VAT,
                                    PerPtId = p.PER_PT_ID,
                                    PerSellId = p.PER_SELL_ID,
                                    TotalHourCard = p.TOTAL_HOUR_CARD,
                                    TotalPrice = p.TOTAL_PRICE,
                                    Wardrobe = p.WARDROBE,
                                    Note = p.NOTE,
                                }).FirstAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(CardIssuanceDTO dto, string sid)
        {
            dto.DocumentNumber = CreateNewCode();

            var startDate = Convert.ToDateTime(dto.StartDate).Date;
            var endDate = Convert.ToDateTime(dto.EndDate).Date;
            if(startDate > endDate)
            {
                return new FormatedResponse() { MessageCode = "Start Date must be less than End Date", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var checkExist = await (from p in _dbContext.CardIssuances.AsNoTracking().Where(p => p.CUSTOMER_ID == dto.CustomerId).DefaultIfEmpty()
                                    from c in _dbContext.CardInfos.AsNoTracking().Where(c => c.ID == p.CARD_ID).DefaultIfEmpty()
                                    where (c.EFFECTED_DATE_TIME!.Value.Date <= startDate && c.EXPIRED_DATE_TIME!.Value.Date <= endDate) ||
                                          (c.EFFECTED_DATE_TIME!.Value.Date <= startDate && startDate <= c.EXPIRED_DATE_TIME!.Value.Date) ||
                                          (c.EFFECTED_DATE_TIME!.Value.Date <= endDate && endDate <= c.EXPIRED_DATE_TIME!.Value.Date)
                                    select p
                                    ).AnyAsync();
            if (checkExist)
            {
                return new FormatedResponse() { MessageCode = "The customer has been issued a card.", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            // check locker status
            var locker = await _dbContext.GoodsLockers.FirstOrDefaultAsync(x => x.ID == dto.LockerId);
            if(locker!.STATUS_ID == 10030)
            {
                return new FormatedResponse() { MessageCode = "Locker has been maintained.", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            var response = await _genericRepository.Create(dto, sid);
            if (response != null)
            {
                if (response.StatusCode == EnumStatusCode.StatusCode200)
                {
                    var card = await _dbContext.CardInfos.FirstOrDefaultAsync(x => x.ID == dto.CardId);
                    if (card == null)
                    {
                        return new FormatedResponse() { MessageCode = "CARD_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    else
                    {
                        try
                        {
                            card.CUSTOMER_ID = dto.CustomerId;
                            _dbContext.CardInfos.Update(card);
                            await _dbContext.SaveChangesAsync();

                            var type = _dbContext.SysOtherLists.AsNoTracking().First(p => p.CODE == "TTRANS1").ID;

                            var bill = await _genericOrdBillRepository.Create(new OrdBillDTO
                            {
                                CustomerId = dto.CustomerId,
                                DiscPercent = dto.PercentDiscount,
                                PercentVat = dto.PercentVat,
                                MoneyHavePay = dto.MoneyHavePay,
                                TypeTransfer = type,
                                TotalMoney = dto.TotalPrice,
                                PayMethod = 0,
                                PerSellId = dto.PerSellId,
                                Code = CreateNewCodeBill(),
                                IsConfirm= false,
                                Printed = false,
                                PrintNumber =0,
                                PkRef = Convert.ToInt64(response.InnerBody!.GetType().GetProperty("Id")!.GetValue(response.InnerBody, null))
                            }, sid);
                            return response;
                        }
                        catch (Exception ex)
                        {
                            return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }

                    }
                }
            }
            return new FormatedResponse() { MessageCode = "CREATE_FAILED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
        }

        public async Task<FormatedResponse> CreateRange(List<CardIssuanceDTO> dtos, string sid)
        {
            var add = new List<CardIssuanceDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(CardIssuanceDTO dto, string sid, bool patchMode = true)
        {
            var startDate = Convert.ToDateTime(dto.StartDate).Date;
            var endDate = Convert.ToDateTime(dto.EndDate).Date;
            if (startDate > endDate)
            {
                return new FormatedResponse() { MessageCode = "Start Date must be less than End Date", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var checkExist = await (from p in _dbContext.CardIssuances.AsNoTracking().Where(p => p.CUSTOMER_ID == dto.CustomerId && p.ID != dto.Id).DefaultIfEmpty()
                                    from c in _dbContext.CardInfos.AsNoTracking().Where(c => c.ID == p.CARD_ID).DefaultIfEmpty()
                                    where (c.EFFECTED_DATE_TIME!.Value.Date <= startDate && c.EXPIRED_DATE_TIME!.Value.Date <= endDate) ||
                                          (c.EFFECTED_DATE_TIME!.Value.Date <= startDate && startDate <= c.EXPIRED_DATE_TIME!.Value.Date) ||
                                          (c.EFFECTED_DATE_TIME!.Value.Date <= endDate && endDate <= c.EXPIRED_DATE_TIME!.Value.Date)
                                    select p
                                    ).AnyAsync();
            if (checkExist)
            {
                return new FormatedResponse() { MessageCode = "The customer has been issued a card", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            // check locker status
            var locker = await _dbContext.GoodsLockers.FirstOrDefaultAsync(x => x.ID == dto.LockerId);
            if (locker!.STATUS_ID == 10030)
            {
                return new FormatedResponse() { MessageCode = "Locker has been maintained.", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            var response = await _genericRepository.Update(dto, sid, patchMode);
            if (response != null)
            {
                if (response.StatusCode == EnumStatusCode.StatusCode200)
                {
                    var card = await _dbContext.CardInfos.SingleAsync(x => x.ID == dto.Id);
                    if (card == null)
                    {
                        return new FormatedResponse() { MessageCode = "CARD_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    else
                    {
                        try
                        {
                            card.CUSTOMER_ID = dto.CustomerId;
                            _dbContext.CardInfos.Update(card);
                            await _dbContext.SaveChangesAsync();
                            return response;
                        }
                        catch (Exception ex)
                        {
                            return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }

                    }
                }
            }
            return new FormatedResponse() { MessageCode = "UPDATE_FAILED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
        }

        public async Task<FormatedResponse> UpdateRange(List<CardIssuanceDTO> dtos, string sid, bool patchMode = true)
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
            if (_dbContext.CardIssuances.Count() == 0)
            {
                newCode = "GYM_HEX/0001";
            }
            else
            {
                string lastestData = _dbContext.CardIssuances.OrderByDescending(t => t.DOCUMENT_NUMBER).First().DOCUMENT_NUMBER!.ToString();

                newCode = lastestData.Substring(0, 8) + (int.Parse(lastestData.Substring(lastestData.Length - 4)) + 1).ToString("D4");
            }
            return newCode;
        }
        public string CreateNewCodeBill()
        {
            string newCode = "";
            if (_dbContext.OrdBills.Count() == 0)
            {
                newCode = "GYM_OD/0001";
            }
            else
            {
                string lastestData = _dbContext.OrdBills.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 7) + (int.Parse(lastestData.Substring(lastestData.Length - 4)) + 1).ToString("D4");
            }
            return newCode;
        }
    }
}

