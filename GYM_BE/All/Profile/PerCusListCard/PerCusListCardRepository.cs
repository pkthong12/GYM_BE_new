using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.PerCusListCard
{
    public class PerCusListCardRepository : IPerCusListCardRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<PER_CUS_LIST_CARD, PerCusListCardDTO> _genericRepository;

        public PerCusListCardRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<PER_CUS_LIST_CARD, PerCusListCardDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<PerCusListCardDTO> pagination)
        {
            var joined = from p in _dbContext.PerCusListCards.AsNoTracking()
                                       //tuy chinh
                       select new PerCusListCardDTO
                        {
                            Id = p.ID,
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
                var list = new List<PER_CUS_LIST_CARD>
                    {
                        (PER_CUS_LIST_CARD)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PerCusListCardDTO
                              {
                                  Id = l.ID
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(PerCusListCardDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<PerCusListCardDTO> dtos, string sid)
        {
            var add = new List<PerCusListCardDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(PerCusListCardDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<PerCusListCardDTO> dtos, string sid, bool patchMode = true)
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

