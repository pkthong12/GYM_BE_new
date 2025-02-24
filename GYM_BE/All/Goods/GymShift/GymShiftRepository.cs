using GYM_BE.All.Gym.GymPackage;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;

namespace GYM_BE.All.Gym.GymShift
{
    public class GymShiftRepository : IGymShiftRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<GOODS_SHIFT, GoodsShiftDTO> _genericRepository;

        public GymShiftRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_SHIFT, GoodsShiftDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsShiftDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsShifts.AsNoTracking()
                         select new GoodsShiftDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             TotalDays = p.TOTAL_DAYS,
                             HoursStart = p.HOURS_START,
                             HoursStartString = p.HOURS_START,
                             HoursEnd = p.HOURS_END,
                             HoursEndString = p.HOURS_END,
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE!.Value ? "Active" : "Inactive"
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<GOODS_SHIFT>
                    {
                        (GOODS_SHIFT)response
                    };
                var joined = (from l in list
                              select new GoodsShiftDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  TotalDays = l.TOTAL_DAYS,
                                  HoursStart = l.HOURS_START,
                                  HoursStartString = l.HOURS_START,
                                  HoursEnd = l.HOURS_END,
                                  HoursEndString = l.HOURS_END,
                                  Note = l.NOTE,
                                  IsActive = l.IS_ACTIVE,
                                  Status = l.IS_ACTIVE!.Value ? "Active" : "Inactive"
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsShiftDTO dto, string sid)
        {
            DateTime hoursStart = DateTime.Parse(dto.HoursStart!);
            DateTime hoursEnd = DateTime.Parse(dto.HoursEnd!);

            if(dto.TotalDays != null)
            {
                if (dto.TotalDays < 0 || dto.TotalDays > 7)
                {
                    return new FormatedResponse() { MessageCode = "Number of Days in a Week must between 0 and 7 ", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }

            if(hoursStart > hoursEnd)
            {
                return new FormatedResponse() { MessageCode = "Start Time must less than End Time", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            dto.Code = CreateNewCode();
            dto.IsActive = true;
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsShiftDTO> dtos, string sid)
        {
            var add = new List<GoodsShiftDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsShiftDTO dto, string sid, bool patchMode = true)
        {
            var checkUsed = await (from c in _dbContext.CardInfos.AsNoTracking().Where(c => c.SHIFT_ID == dto.Id)
                                   from i in _dbContext.CardIssuances.AsNoTracking().Where(i => i.CARD_ID == c.ID)
                                   select i.ID).AnyAsync();
            if (checkUsed)
            {
                return new FormatedResponse() { MessageCode = "Cannot edit data in use", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            if (dto.TotalDays != null)
            {
                if (dto.TotalDays < 0 || dto.TotalDays > 7)
                {
                    return new FormatedResponse() { MessageCode = "Number of Days in a Week must between 0 and 7 ", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            DateTime hoursStart = DateTime.Parse(dto.HoursStart!);
            DateTime hoursEnd = DateTime.Parse(dto.HoursEnd!);
            if (hoursStart > hoursEnd)
            {
                return new FormatedResponse() { MessageCode = "Start Time must less than End Time", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsShiftDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(long id)
        {
            var checkUsed = await _dbContext.CardInfos.AsNoTracking().AnyAsync(c => c.SHIFT_ID == id);
            if (checkUsed)
            {
                return new FormatedResponse() { MessageCode = "Cannot delete data in use", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
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
            var checkUsed = await _dbContext.CardInfos.AsNoTracking().AnyAsync(c => ids.Contains(c.SHIFT_ID!.Value));
            if (checkUsed)
            {
                return new FormatedResponse() { MessageCode = "Cannot delete data in use", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var response = await _genericRepository.DeleteIds(ids);
            return response;
        }

        public async Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetList()
        {
            var res = await (from p in _dbContext.GoodsShifts.AsNoTracking()
                             where p.IS_ACTIVE == true 
                             select new GoodsShiftDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }

        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.GoodsShifts.Count() == 0)
            {
                newCode = "SHIFT0001";
            }
            else
            {
                string lastestData = _dbContext.GoodsShifts.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 5) + (int.Parse(lastestData.Substring(lastestData.Length - 4)) + 1).ToString("D4");
            }

            return newCode;

        }
    }
}
