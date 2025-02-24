using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;

namespace GYM_BE.All.CardCheckIn
{
    public class CardCheckInRepository : ICardCheckInRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<CARD_CHECK_IN, CardCheckInDTO> _genericRepository;

        public CardCheckInRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<CARD_CHECK_IN, CardCheckInDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<CardCheckInDTO> pagination)
        {
            var joined = from p in _dbContext.CardCheckIns.AsNoTracking()
                         from i in _dbContext.CardInfos.Where(x => x.ID == p.CARD_INFO_ID).DefaultIfEmpty()
                         from sh in _dbContext.GoodsShifts.Where(x => x.ID == i.SHIFT_ID).DefaultIfEmpty()
                         from c in _dbContext.PerCustomers.Where(x => x.ID == i.CUSTOMER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.Where(x => x.ID == i.CARD_TYPE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                         orderby p.ID descending
                         select new CardCheckInDTO
                         {
                             Id = p.ID,
                             CardCode = i.CODE,
                             CodeCus = c.CODE,
                             CustomerName = c.FULL_NAME,
                             CardTypeName = s.NAME,
                             TimeEnd = p.TIME_END,
                             GenderName = g.NAME,
                             TimeStart = p.TIME_START,
                             DayCheckIn = p.DAY_CHECK_IN,
                             DayCheckInString = p.DAY_CHECK_IN!.Value.ToString("dd/MM/yyyy"),
                             TimeStartString = p.TIME_START!.Value.ToString("HH:mm:ss"),
                             TimeEndString = p.TIME_END!.Value.ToString("HH:mm:ss"),
                             TimeStartShiftString = sh.HOURS_START,
                             TimeEndShiftString = sh.HOURS_END,
                             ShiftName = sh.NAME,
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.CardCode != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.CardCode == pagination.Filter.CardCode);
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
            var joined = await (from p in _dbContext.CardCheckIns.AsNoTracking()
                                from i in _dbContext.CardInfos.Where(x => x.ID == p.CARD_INFO_ID).DefaultIfEmpty()
                                from sh in _dbContext.GoodsShifts.Where(x => x.ID == i.SHIFT_ID).DefaultIfEmpty()
                                from c in _dbContext.PerCustomers.Where(x => x.ID == i.CUSTOMER_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.Where(x => x.ID == i.CARD_TYPE_ID).DefaultIfEmpty()
                                from g in _dbContext.SysOtherLists.Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                                where p.ID == id
                                select new CardCheckInDTO
                                {
                                    Id = p.ID,
                                    CardCode = i.CODE,
                                    CodeCus = c.CODE,
                                    CustomerName = c.FULL_NAME,
                                    CardTypeName = s.NAME,
                                    TimeEnd = p.TIME_END,
                                    GenderName = g.NAME,
                                    TimeStart = p.TIME_START,
                                    DayCheckIn = p.DAY_CHECK_IN,
                                    DayCheckInString = p.DAY_CHECK_IN!.Value.ToString("dd/MM/yyyy"),
                                    TimeStartString = p.TIME_START!.Value.ToString("HH:mm:ss"),
                                    TimeEndString = p.TIME_END!.Value.ToString("HH:mm:ss"),
                                    TimeStartShiftString = sh.HOURS_START,
                                    TimeEndShiftString = sh.HOURS_END,
                                    ShiftName = sh.NAME,
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

        public async Task<FormatedResponse> Create(CardCheckInDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<CardCheckInDTO> dtos, string sid)
        {
            var add = new List<CardCheckInDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(CardCheckInDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<CardCheckInDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(long id)
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

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }


        public async Task<FormatedResponse> CheckIn(string cardCode, string sid)
        {
            try
            {
                var timeCheckIn = DateTime.UtcNow.AddHours(7);
                var checkExistCard = await _dbContext.CardInfos.FirstOrDefaultAsync(x => x.CODE!.ToUpper() == cardCode.ToUpper());
                if (checkExistCard == null)
                {
                    return new FormatedResponse() { MessageCode = "Can't find card information", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }

                // check the gan voi hoi vien
                if (checkExistCard.CUSTOMER_ID == null)
                {
                    return new FormatedResponse() { MessageCode = "The card has not been attached to members", StatusCode = EnumStatusCode.StatusCode400 };
                }

                string notification = "";
                // thong bao het han the
                var expireDate = _dbContext.CardInfos.FirstOrDefault(x => x.CODE!.ToUpper() == cardCode.ToUpper())!.EXPIRED_DATE;
                TimeSpan difference = DateTime.ParseExact(expireDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.UtcNow;
                double days = difference.TotalDays;
                if (days <= 0)
                {
                    return new FormatedResponse() { MessageCode = "CARD_EXPIRED", StatusCode = EnumStatusCode.StatusCode400 };
                }
                else if (days <= 7)
                {
                    notification = "THIS CARD HAS " + days + " DAYS LEFT";
                }


                var cardId = _dbContext.CardInfos.FirstOrDefault(x => x.CODE!.ToUpper() == cardCode.ToUpper())!.ID;
                var checkExsist = _dbContext.CardCheckIns.Where(x => x.CARD_INFO_ID == cardId && x.DAY_CHECK_IN!.Value.Date == DateTime.Now.Date).ToList();
                if (checkExsist.Count() == 0)
                {
                    var checkIn = new CardCheckInDTO
                    {
                        CardInfoId = cardId,
                        TimeStart = timeCheckIn,
                        DayCheckIn = timeCheckIn.Date,
                    };
                    var response = await _genericRepository.Create(checkIn, sid);

                    // thay doi trang thai tu
                    var lockerId = _dbContext.CardIssuances.FirstOrDefault(x => x.CARD_ID == cardId)!.LOCKER_ID;
                    var locker = await _dbContext.GoodsLockers.FirstOrDefaultAsync(x => x.ID == lockerId);
                    locker!.STATUS_ID = 10031;
                    await _dbContext.SaveChangesAsync();

                    response.MessageCode = notification;
                    return response;
                }
                else
                {
                    var checkIn = _dbContext.CardCheckIns.FirstOrDefault(x => x.CARD_INFO_ID == cardId && x.DAY_CHECK_IN!.Value.Date == DateTime.Now.Date);
                    var card = new CardCheckInDTO();
                    card.Id = checkIn!.ID;
                    card.DayCheckIn = checkIn.DAY_CHECK_IN!.Value;
                    card.CardInfoId = cardId;
                    card.TimeStart = checkIn.TIME_START;
                    card.TimeEnd = timeCheckIn;
                    var response = await _genericRepository.Update(card, sid, true);

                    // thay doi trang thai tu
                    var lockerId = _dbContext.CardIssuances.FirstOrDefault(x => x.CARD_ID == cardId)!.LOCKER_ID;
                    var locker = await _dbContext.GoodsLockers.FirstOrDefaultAsync(x => x.ID == lockerId);
                    locker!.STATUS_ID = 10032;
                    await _dbContext.SaveChangesAsync();

                    response.MessageCode = notification;
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetListCardCode()
        {
            var listCode = await (from i in _dbContext.CardInfos.AsNoTracking()
                                  from c in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == i.CUSTOMER_ID).DefaultIfEmpty()
                                  where i.CUSTOMER_ID != null
                                  orderby i.CODE
                                  select new
                                  {
                                      Code = i.CODE,
                                      Name = c.FULL_NAME,
                                  }).Distinct().ToListAsync();
            return new FormatedResponse() { InnerBody = listCode };
        }
    }
}

