using GYM_BE.Core.Dto;

namespace GYM_BE.Core.Generic
{
    public interface IGenericRepository<TEntity, TDTO> where TEntity : class where TDTO : class
    {
        Task<FormatedResponse> GetById(long id);

        Task<FormatedResponse> Create( TDTO dto, string sid);

        Task<FormatedResponse> CreateRange(List<TDTO> dtos, string sid);

        Task<FormatedResponse> Update(TDTO dto, string sid, bool patchMode = true);

        Task<FormatedResponse> UpdateRange(List<TDTO> dtos, string sid, bool patchMode = true);

        Task<FormatedResponse> Delete(long id);

        Task<FormatedResponse> Delete(string id);

        Task<FormatedResponse> DeleteIds(List<long> ids);

        Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid);
    }
}
