using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.CardHistory
{
    public class CardHistoryRepository : ICardHistoryRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<CARD_HISTORY, CardHistoryDTO> _genericRepository;

        public CardHistoryRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<CARD_HISTORY, CardHistoryDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<CardHistoryDTO> pagination)
        {
            var joined = from p in _dbContext.CardHistorys.AsNoTracking()
                         from sh in _dbContext.GoodsShifts.AsNoTracking().Where(sh => sh.ID == p.SHIFT_ID).DefaultIfEmpty()
                         from c in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.CARD_TYPE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                         select new CardHistoryDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             EffectDateString = p.EFFECTED_DATE!,
                             ExpiredDateString = p.EXPIRED_DATE!,
                             CardTypeName = s.NAME,
                             CustomerName = c.FULL_NAME,
                             GenderName = g.NAME,
                             LockerId = p.LOCKER_ID,
                             Status = p.IS_ACTIVE!.Value == true ? "Hoạt động" : "Ngừng hoạt động",
                             Note = p.NOTE,
                             CodeCus = c.CODE,
                             Wardrobe = p.WARDROBE,
                             Price = p.PRICE,
                             ShiftId = p.SHIFT_ID,
                             ShiftName = sh.NAME,
                             Action = p.ACTION,
                             ActionStr = p.ACTION!.Value == 1 ? "Thêm mới" : (p.ACTION!.Value == 2 ? "Sửa" : "Xóa"),
                             IsHavePt = p.IS_HAVE_PT,
                             CreatedDateStr = p.CREATED_DATE!.Value.ToString("HH:mm:ss dd/MM/yyyy"),
                             CreatedByUsername = u.FULLNAME,
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.Code != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.Code == pagination.Filter.Code);
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
            var joined = await (from p in _dbContext.CardHistorys.AsNoTracking().Where(x => x.ID == id)
                                from sh in _dbContext.GoodsShifts.AsNoTracking().Where(sh => sh.ID == p.SHIFT_ID).DefaultIfEmpty()
                                from c in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.CARD_TYPE_ID).DefaultIfEmpty()
                                from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                                select new CardHistoryDTO
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    EffectDateString = p.EFFECTED_DATE!,
                                    ExpiredDateString = p.EXPIRED_DATE!,
                                    CardTypeName = s.NAME,
                                    CustomerName = c.FULL_NAME,
                                    GenderName = g.NAME,
                                    LockerId = p.LOCKER_ID,
                                    Status = p.IS_ACTIVE!.Value == true ? "Hoạt động" : "Ngừng hoạt động",
                                    Note = p.NOTE,
                                    CodeCus = c.CODE,
                                    Wardrobe = p.WARDROBE,
                                    Price = p.PRICE,
                                    ShiftId = p.SHIFT_ID,
                                    ShiftName = sh.NAME,
                                    Action = p.ACTION,
                                    ActionStr = p.ACTION!.Value == 1 ? "Thêm mới" : (p.ACTION!.Value == 2 ? "Sửa" : "Xóa"),
                                    IsHavePt = p.IS_HAVE_PT
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

        public async Task<FormatedResponse> Create(CardHistoryDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<CardHistoryDTO> dtos, string sid)
        {
            var add = new List<CardHistoryDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(CardHistoryDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<CardHistoryDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, "root", patchMode);
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

        public async Task<FormatedResponse> GetListCardCode()
        {
            var listCode = await (from c in _dbContext.CardHistorys.AsNoTracking()
                                  orderby c.CODE
                                  select new
                                  {
                                      Code = c.CODE
                                  }).Distinct().ToListAsync();
            return new FormatedResponse() { InnerBody = listCode };
        }
    }
}

