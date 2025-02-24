using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.Gym.GymPackage
{
    public class GymPackageRepository : IGymPackageRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<GOODS_PACKAGE, GoodsPackageDTO> _genericRepository;

        public GymPackageRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_PACKAGE, GoodsPackageDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsPackageDTO> pagination)
        {
            var joined = from p in _dbContext.GymPackages.AsNoTracking()
                         from s in _dbContext.GoodsShifts.AsNoTracking().Where(s => s.ID == p.SHIFT_ID).DefaultIfEmpty()
                         select new GoodsPackageDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Money = p.MONEY,
                             Period = p.PERIOD,
                             ShiftId = p.SHIFT_ID,
                             ShiftName = s.NAME,
                             Description = p.DESCRIPTION,
                             IsPrivate = p.IS_PRIVATE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng"
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
                var list = new List<GOODS_PACKAGE>
                    {
                        (GOODS_PACKAGE)response
                    };
                var joined = (from l in list
                              from s in _dbContext.GoodsShifts.AsNoTracking().Where(s => s.ID == l.SHIFT_ID).DefaultIfEmpty()
                              select new GoodsPackageDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Money = l.MONEY,
                                  Period = l.PERIOD,
                                  ShiftId = l.SHIFT_ID,
                                  ShiftName = s.NAME,
                                  Description = l.DESCRIPTION,
                                  IsPrivate = l.IS_PRIVATE,
                                  IsActive = l.IS_ACTIVE,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsPackageDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsPackageDTO> dtos, string sid)
        {
            var add = new List<GoodsPackageDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsPackageDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsPackageDTO> dtos, string sid, bool patchMode = true)
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
    }
}
