using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.PerCusTransaction
{
    public class PerCusTransactionRepository : IPerCusTransactionRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<PER_CUS_TRANSACTION, PerCusTransactionDTO> _genericRepository;

        public PerCusTransactionRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<PER_CUS_TRANSACTION, PerCusTransactionDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<PerCusTransactionDTO> pagination)
        {
            var joined = from p in _dbContext.PerCusTransactions.AsNoTracking()
                         from e in _dbContext.PerCustomers.AsNoTracking().Where(e => e.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == e.GENDER_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s2=> s2.ID == e.CUSTOMER_CLASS_ID).DefaultIfEmpty()
                             //tuy chinh
                         select new PerCusTransactionDTO
                         {
                             Id = p.ID,
                             FullName = e.FULL_NAME,
                             Address = e.ADDRESS,
                             BirthDateString = e.BIRTH_DATE!,
                             Code = p.CODE,
                             CustomerCode = e.CODE,
                             GenderName = s.NAME,
                             TransDateString = p.TRANS_DATE!,
                             PhoneNumber = e.PHONE_NUMBER,
                             IdNo = e.ID_NO,
                             CustomerId = p.CUSTOMER_ID,
                             TransDate = p.TRANS_DATE,
                             TransForm = p.TRANS_FORM,
                             CustomerClassName = s2.NAME,
                             CustomerClassId = s2.ID,
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
                var list = new List<PER_CUS_TRANSACTION>
                    {
                        (PER_CUS_TRANSACTION)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PerCusTransactionDTO
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

        public async Task<FormatedResponse> Create(PerCusTransactionDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<PerCusTransactionDTO> dtos, string sid)
        {
            var add = new List<PerCusTransactionDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(PerCusTransactionDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<PerCusTransactionDTO> dtos, string sid, bool patchMode = true)
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

