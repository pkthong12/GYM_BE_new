using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsLocker
{
    public class GoodsLockerRepository : IGoodsLockerRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<GOODS_LOCKER, GoodsLockerDTO> _genericRepository;

        public GoodsLockerRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_LOCKER, GoodsLockerDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsLockerDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsLockers.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s=> s.ID == p.AREA).DefaultIfEmpty()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1=> s1.ID == p.STATUS_ID).DefaultIfEmpty()
                             //tuy chinh
                         select new GoodsLockerDTO
                         {
                             Id = p.ID,
                             Code= p.CODE,
                             Price= p.PRICE,
                             Area= p.AREA,
                             AreaName = s.NAME,
                             MaintenanceFromDate = p.MAINTENANCE_FROM_DATE,
                             MaintenanceToDate = p.MAINTENANCE_TO_DATE,
                             StatusId = p.STATUS_ID,
                             StatusName = s1.NAME,
                             Note= p.NOTE
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
           
                var joined = (from p in _dbContext.GoodsLockers.AsNoTracking().Where(p=> p.ID == id)
                              from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.AREA).DefaultIfEmpty()
                              from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1 => s1.ID == p.STATUS_ID).DefaultIfEmpty()
                              
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new GoodsLockerDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE,
                                  Price = p.PRICE,
                                  Area = p.AREA,
                                  AreaName = s.NAME,
                                  MaintenanceFromDate = p.MAINTENANCE_FROM_DATE,
                                  MaintenanceToDate = p.MAINTENANCE_TO_DATE,
                                  StatusId = p.STATUS_ID,
                                  StatusName = s1.NAME,
                                  Note = p.NOTE
                              }).FirstOrDefault();

            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsLockerDTO dto, string sid)
        {
            dto.Code = CreateNewCode();
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsLockerDTO> dtos, string sid)
        {
            var add = new List<GoodsLockerDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsLockerDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsLockerDTO> dtos, string sid, bool patchMode = true)
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
            if (_dbContext.GoodsLockers.Count() == 0)
            {
                newCode = "LOC001";
            }
            else
            {
                string lastestData = _dbContext.GoodsLockers.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return newCode;

        }

        public async Task<FormatedResponse> GetAllLockerValid(long? id,long? cusId,long cardId)
        {
            var card = await _dbContext.CardInfos.AsNoTracking().SingleAsync(p=> p.ID == cardId);
            var genderId = _dbContext.PerCustomers.AsNoTracking().First(p => p.ID == cusId).GENDER_ID;
            if(card == null)
            {
                return new FormatedResponse() { InnerBody = null };
            }
            else if(card.WARDROBE ==  null || card.WARDROBE == false)
            {
                return new FormatedResponse() { InnerBody = null };
            }
            else if (genderId == null && id == null)
            {
                return new FormatedResponse() { InnerBody = null };
            }
            else
            {
                var shift = await _dbContext.GoodsShifts.AsNoTracking().SingleAsync(p => p.ID == card.SHIFT_ID);

                var lockerInvalid = await (from ci in _dbContext.CardIssuances.AsNoTracking()
                                           select ci.LOCKER_ID).Distinct().ToListAsync();

                var res = await (from p in _dbContext.GoodsLockers.AsNoTracking().Where(p => !lockerInvalid.Contains(p.ID) && p.AREA == genderId).DefaultIfEmpty()
                                 select new
                                 {
                                     Id = p.ID,
                                     Name = p.CODE,
                                 }).ToListAsync();
                if (id != null)
                {
                    var x = await (from p in _dbContext.GoodsLockers.Where(p => p.ID == id).DefaultIfEmpty()
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.CODE,
                                   }).FirstOrDefaultAsync();
                    if (x != null)
                    {
                        var check = res.Find(p => p.Id == x.Id);
                        if (check == null)
                        {
                            res.Add(x);
                            res.OrderBy(p => p.Id);
                        }
                    }
                }
                return new FormatedResponse() { InnerBody = res };
            }
        }
    
        public async Task<FormatedResponse> GetLockerStatus(long area)
        {
            var listLocker = await (from p in _dbContext.GoodsLockers.AsNoTracking().Where(p => p.AREA == area)
                                    from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty()
                                    select new GoodsLockerStatusDTO
                                    {
                                        CodeLoc = p.CODE,
                                        Status = s.CODE == "LOC1" ? "M" : s.CODE == "LOC2" ? "U" :"E"
                                    }).ToListAsync();
            var list = new GoodsLockerStatusListAllDTO() { List = new List<GoodsLockerStatusListDTO>() };
            if (listLocker != null)
            {
                var row = listLocker.Count / 10;
                for(int i = 0; i <= row; i++)
                {
                    var items = listLocker.Skip(i*10).Take(10).ToList();
                    var listItems = new GoodsLockerStatusListDTO() { Items = items };
                    list.List!.Add(listItems);
                }
            }
            return new FormatedResponse() { InnerBody = list };
        }
    }
}

