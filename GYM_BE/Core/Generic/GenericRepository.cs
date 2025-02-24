using CORE.AutoMapper;
using GYM_BE.Core.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using GYM_BE.DTO;
namespace GYM_BE.Core.Generic
{
    public class GenericRepository<TEntity, TDTO> : IGenericRepository<TEntity, TDTO> where TEntity : class where TDTO : class
    {
        internal DbContext _dbContext;

        internal DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public virtual async Task<QueryListResponse<TDTO>> PagingQueryList(IQueryable<TDTO> dTOs, PaginationDTO<TDTO> pagination)
        {
            var list = await Task.Run(() => dTOs.Skip((pagination.Page - 1) * pagination.Take).Take(pagination.Take));
            return new QueryListResponse<TDTO>
            {
                Count = dTOs.Count(),
                List = list,
                Page = pagination.Page,
                PageCount = list.Count(),
                Skip = pagination.Skip,
                Take = pagination.Take,
                MessageCode = "QUERY_LIST_SUCCESS",
            };
        }
        public virtual async Task<FormatedResponse> Create(TDTO dto, string sid)
        {
            _dbContext.Database.BeginTransaction();
            object obj = Activator.CreateInstance(typeof(TEntity)) ?? throw new Exception("");
            TEntity entity = (TEntity)obj;
            TEntity mapped = CoreMapper<TDTO, TEntity>.DtoToEntity(dto, entity, true);
            if (mapped != null)
            {
                Type typeFromHandle = typeof(TEntity);
                ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle);
                PropertyInfo propertyInfo = (from pi in typeFromHandle.GetProperties()
                                             where pi.Name == "CREATED_DATE"
                                             select pi).SingleOrDefault();
                PropertyInfo? propertyInfo2 = (from pi in typeFromHandle.GetProperties()
                                               where pi.Name == "CREATED_BY"
                                               select pi).SingleOrDefault();
                propertyInfo?.SetValue(entity, DateTime.UtcNow.AddHours(7));
                propertyInfo2?.SetValue(entity, sid);
                try
                {
                    _dbSet.Add(mapped!);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Entry(mapped).ReloadAsync();
                    await _dbContext.Database.CommitTransactionAsync();
                    return new FormatedResponse()
                    {
                        InnerBody = mapped,
                        StatusCode = EnumStatusCode.StatusCode200,
                        MessageCode = "CREATE_SUCCESS",
                    };
                }
                catch (Exception ex)
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    await _dbContext.DisposeAsync();
                    return new FormatedResponse()
                    {
                        MessageCode = ex.Message,
                        StatusCode = EnumStatusCode.StatusCode400
                    };

                }
            }
            return new FormatedResponse()
            {
                MessageCode = "CREATE_FAILED",
                StatusCode = EnumStatusCode.StatusCode400
            };
        }
        public virtual async Task<FormatedResponse> CreateRange(List<TDTO> dtos, string sid)
        {
            _dbContext.Database.BeginTransaction();
            List<TEntity> mappeds = new List<TEntity>();
            for (int i = 0; i < dtos.Count; i++)
            {
                TDTO dto = dtos[i];
                object obj = Activator.CreateInstance(typeof(TEntity)) ?? throw new Exception("ERROR");
                TEntity entity = (TEntity)obj;
                TEntity mapped = CoreMapper<TDTO, TEntity>.DtoToEntity(dto, entity, true);
                if (mapped != null)
                {
                    Type typeFromHandle = typeof(TEntity);
                    ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle);
                    PropertyInfo propertyInfo = (from pi in typeFromHandle.GetProperties()
                                                 where pi.Name == "CREATED_DATE"
                                                 select pi).SingleOrDefault();
                    PropertyInfo? propertyInfo2 = (from pi in typeFromHandle.GetProperties()
                                                   where pi.Name == "CREATED_BY"
                                                   select pi).SingleOrDefault();
                    propertyInfo?.SetValue(entity, DateTime.UtcNow);
                    propertyInfo2?.SetValue(entity, sid);
                    mappeds.Add(mapped);
                    continue;
                }
            }
            try
            {
                await _dbSet.AddRangeAsync(mappeds);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
                return new FormatedResponse()
                {
                    InnerBody = mappeds,
                    StatusCode = EnumStatusCode.StatusCode200,
                    MessageCode = "CREATE_SUCCESS",
                };
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                await _dbContext.DisposeAsync();
                return new FormatedResponse()
                {
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400
                };

            }
        }
        public virtual async Task<FormatedResponse> GetById(long id)
        {
            TEntity val = await _dbSet.FindAsync(id);
            if (val != null)
            {
                return new FormatedResponse
                {
                    InnerBody = val
                };
            }

            return new FormatedResponse
            {
                StatusCode = EnumStatusCode.StatusCode400,
                ErrorType = EnumErrorType.CATCHABLE,
                MessageCode = "ENTITY_NOT_FOUND"
            };
        }
        public virtual async Task<FormatedResponse> Update(TDTO dto, string sid, bool patchMode = true)
        {
            _dbContext.Database.BeginTransaction();
            PropertyInfo propertyInfo = (from pi in typeof(TDTO).GetProperties()
                                         where pi.Name! == "Id"
                                         select pi).SingleOrDefault();
            if (propertyInfo == null)
            {
                return new FormatedResponse
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "DTO_HAS_NO_PPROPERTY_ID"
                };
            }

            ConstantExpression constantExpression = Expression.Constant(propertyInfo.GetValue(dto), propertyInfo.PropertyType);
            Type entityType = typeof(TEntity);
            ParameterExpression entityExpressionParameter = Expression.Parameter(entityType);
            PropertyInfo entityId = (from pi in entityType.GetProperties()
                                     where pi.Name == "ID"
                                     select pi).SingleOrDefault();
            if (entityId == null)
            {
                return new FormatedResponse
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "ENTITY_HAS_NO_PPROPERTY_ID"
                };
            }

            TEntity entity = _dbSet.AsQueryable().Where("x => x.ID == @0", constantExpression.Value).FirstOrDefault();
            if (entity == null)
            {
                return new FormatedResponse
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "ENTITY_NOT_FOUND"
                };
            }

            ConstantExpression entityIdConstant = Expression.Constant(entityId.GetValue(entity), entityId.PropertyType);
            TEntity mapped = CoreMapper<TDTO, TEntity>.DtoToEntity(dto, entity, patchMode);

            if (mapped != null)
            {
                Type typeFromHandle = typeof(TEntity);
                ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle);
                PropertyInfo propertyInfo2 = (from pi in typeFromHandle.GetProperties()
                                              where pi.Name == "UPDATED_DATE"
                                              select pi).SingleOrDefault();
                PropertyInfo? propertyInfo3 = (from pi in typeFromHandle.GetProperties()
                                               where pi.Name == "UPDATED_BY"
                                               select pi).SingleOrDefault();
                propertyInfo2?.SetValue(entity, DateTime.UtcNow.AddHours(7));
                propertyInfo3?.SetValue(entity, sid);
                try
                {
                    _dbSet.Update(mapped!);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();
                    return new FormatedResponse()
                    {
                        InnerBody = dto,
                        StatusCode = EnumStatusCode.StatusCode200,
                        MessageCode = "UPDATE_SUCCESS",
                    };
                }
                catch (Exception ex)
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    await _dbContext.DisposeAsync();
                    return new FormatedResponse()
                    {
                        MessageCode = ex.Message,
                        StatusCode = EnumStatusCode.StatusCode400
                    };

                }
            }
            return new FormatedResponse()
            {
                MessageCode = "UPDATE_FAILED",
                StatusCode = EnumStatusCode.StatusCode400
            };
            throw new NotImplementedException();
        }
        public virtual async Task<FormatedResponse> UpdateRange(List<TDTO> dtos, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }
        public virtual async Task<FormatedResponse> Delete(long id)
        {
            TEntity val = await _dbSet.FindAsync(id);
            if (val != null)
            {
                _dbContext.Database.BeginTransaction();
                try
                {
                    _dbSet.Remove(val);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();
                    return new FormatedResponse
                    {
                        InnerBody = val,
                        MessageCode = "DELETE_SUCCESS",
                        StatusCode = EnumStatusCode.StatusCode200
                    };
                }
                catch (Exception ex)
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    await _dbContext.DisposeAsync();
                    return new FormatedResponse()
                    {
                        MessageCode = ex.Message,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            return new FormatedResponse
            {
                StatusCode = EnumStatusCode.StatusCode400,
                ErrorType = EnumErrorType.CATCHABLE,
                MessageCode = "ENTITY_NOT_FOUND"
            };
        }

        public virtual async Task<FormatedResponse> Delete(string id)
        {
            TEntity val = await _dbSet.FindAsync(id);
            if (val != null)
            {
                _dbContext.Database.BeginTransaction();
                try
                {
                    _dbSet.Remove(val);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();
                    return new FormatedResponse
                    {
                        InnerBody = val,
                        MessageCode = "DELETE_SUCCESS",
                        StatusCode = EnumStatusCode.StatusCode200
                    };
                }
                catch (Exception ex)
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    await _dbContext.DisposeAsync();
                    return new FormatedResponse()
                    {
                        MessageCode = ex.Message,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            return new FormatedResponse
            {
                StatusCode = EnumStatusCode.StatusCode400,
                ErrorType = EnumErrorType.CATCHABLE,
                MessageCode = "ENTITY_NOT_FOUND"
            };
        }

        public virtual async Task<FormatedResponse> DeleteIds(List<long> ids)
        {
            _dbContext.Database.BeginTransaction();
            string idsString = ids.Aggregate("_", (string prev, long curr) => prev + $"{curr}_");
            string predicate = await Task.Run(() => "x => @0.Contains(@1 + x.ID.ToString() + @1)");
            IQueryable<TEntity> queryable = _dbSet.Where(predicate, idsString, "_");
            if (queryable != null)
            {
                List<TEntity> innerBody = queryable.ToList();
                try
                {
                    _dbSet.RemoveRange(queryable);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.CommitTransactionAsync();
                    return new FormatedResponse
                    {
                        InnerBody = innerBody,
                        MessageCode = "DELETE_SUCCESS"
                    };
                }
                catch (Exception ex)
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    await _dbContext.DisposeAsync();
                    return new FormatedResponse
                    {
                        ErrorType = EnumErrorType.UNCATCHABLE,
                        MessageCode = ex.Message,
                        StatusCode = EnumStatusCode.StatusCode500
                    };
                }
            }

            return new FormatedResponse
            {
                StatusCode = EnumStatusCode.StatusCode400,
                ErrorType = EnumErrorType.CATCHABLE,
                MessageCode = "ENTITY_NOT_FOUND"
            };
        }
        public virtual async Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }


    }
}
