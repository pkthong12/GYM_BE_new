using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.SysOtherListType
{
    public class SysOtherListTypeRepository : ISysOtherListTypeRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<SYS_OTHER_LIST_TYPE, SysOtherListTypeDTO> _genericRepository;

        public SysOtherListTypeRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<SYS_OTHER_LIST_TYPE, SysOtherListTypeDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<SysOtherListTypeDTO> pagination)
        {
            var joined = from p in _dbContext.SysOtherListTypes.AsNoTracking()
                             //tuy chinh
                         select new SysOtherListTypeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             Note = p.NOTE,
                             Orders = p.ORDERS,
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
                var list = new List<SYS_OTHER_LIST_TYPE>
                    {
                        (SYS_OTHER_LIST_TYPE)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SysOtherListTypeDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  Note = l.NOTE,
                                  Orders = l.ORDERS,
                                  IsActive = l.IS_ACTIVE,
                                  Status = l.IS_ACTIVE!.Value ? "Áp dụng":"Ngừng áp dụng"
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(SysOtherListTypeDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<SysOtherListTypeDTO> dtos, string sid)
        {
            var add = new List<SysOtherListTypeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(SysOtherListTypeDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<SysOtherListTypeDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetList()
        {
            var res = await (from p in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.IS_ACTIVE == true)
                                select new 
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    Name = p.NAME,
                                }).ToListAsync();
            return new FormatedResponse
            {
                InnerBody = res,
            };
        }

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}

