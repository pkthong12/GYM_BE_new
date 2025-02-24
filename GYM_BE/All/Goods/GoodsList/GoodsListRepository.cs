using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsList
{
    public class GoodsListRepository : IGoodsListRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<GOODS_LIST, GoodsListDTO> _genericRepository;

        public GoodsListRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_LIST, GoodsListDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsListDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsLists.AsNoTracking()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.PRODUCT_TYPE_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.MEASURE_ID).DefaultIfEmpty()
                         from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS).DefaultIfEmpty()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(x => x.ID == p.MANAGER_ID).DefaultIfEmpty()
                         select new GoodsListDTO
                        {
                            Id = p.ID,
                            Code = p.CODE,
                            Name = p.NAME,
                            ProductTypeId = p.PRODUCT_TYPE_ID,
                            ProductTypeName = s1.NAME,
                            Supplier = p.SUPPLIER,
                            ImportPrice = p.IMPORT_PRICE,
                            Price = p.PRICE,
                            Quantity = p.QUANTITY,
                            MeasureId = p.MEASURE_ID,
                            MeasureName = s2.NAME,
                            ReceivingDate = p.RECEIVING_DATE,
                            ReceivingDateTime = p.RECEIVING_DATETIME,
                            ExpireDate = p.EXPIRE_DATE,
                            ExpireDateTime = p.EXPIRE_DATETIME,
                            Location = p.LOCATION,
                            Note = p.NOTE,
                            BatchNo = p.BATCH_NO,
                            WarrantyInfor = p.WARRANTY_INFOR,
                            Description = p.DESCRIPTION,
                            Source = p.SOURCE,
                            ManagerId = p.MANAGER_ID,
                            ManagerName = e.FULL_NAME,
                            Status = p.STATUS,
                            StatusName = s3.NAME,
                        };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.ProductTypeId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.ProductTypeId == pagination.Filter.ProductTypeId);
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
            var joined = await (from p in _dbContext.GoodsLists.AsNoTracking().Where(x => x.ID == id)
                                from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.PRODUCT_TYPE_ID).DefaultIfEmpty()
                                from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.MEASURE_ID).DefaultIfEmpty()
                                from s3 in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS).DefaultIfEmpty()
                                from e in _dbContext.PerEmployees.AsNoTracking().Where(x => x.ID == p.MANAGER_ID).DefaultIfEmpty()
                                select new GoodsListDTO
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    Name = p.NAME,
                                    ProductTypeId = p.PRODUCT_TYPE_ID,
                                    ProductTypeName = s1.NAME,
                                    Supplier = p.SUPPLIER,
                                    ImportPrice = p.IMPORT_PRICE,
                                    Price = p.PRICE,
                                    Quantity = p.QUANTITY,
                                    MeasureId = p.MEASURE_ID,
                                    MeasureName = s2.NAME,
                                    ReceivingDate = p.RECEIVING_DATE,
                                    ReceivingDateTime = p.RECEIVING_DATETIME,
                                    ExpireDate = p.EXPIRE_DATE,
                                    ExpireDateTime = p.EXPIRE_DATETIME,
                                    Location = p.LOCATION,
                                    Note = p.NOTE,
                                    BatchNo = p.BATCH_NO,
                                    WarrantyInfor = p.WARRANTY_INFOR,
                                    Description = p.DESCRIPTION,
                                    Source = p.SOURCE,
                                    ManagerId = p.MANAGER_ID,
                                    ManagerName = e.FULL_NAME,
                                    Status = p.STATUS,
                                    StatusName = s3.NAME,
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

        public async Task<FormatedResponse> Create(GoodsListDTO dto, string sid)
        {
            dto.ReceivingDateTime = Convert.ToDateTime(dto.ReceivingDate);
            dto.ExpireDateTime = Convert.ToDateTime(dto.ExpireDate);
            dto.Code = CreateNewCode();
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsListDTO> dtos, string sid)
        {
            var add = new List<GoodsListDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsListDTO dto, string sid, bool patchMode = true)
        {
            dto.ReceivingDateTime = Convert.ToDateTime(dto.ReceivingDate);
            dto.ExpireDateTime = Convert.ToDateTime(dto.ExpireDate);
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsListDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> Delete(string id)
        {
            var response = await _genericRepository.Delete(id);
            return response;
        }

        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.GoodsLists.Count() == 0)
            {
                newCode = "GOODS001";
            }
            else
            {
                string lastestData = _dbContext.GoodsLists.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 5) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return newCode;

        }
    }
}

