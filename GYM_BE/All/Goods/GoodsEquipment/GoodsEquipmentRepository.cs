using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsEquipment
{
    public class GoodsEquipmentRepository : IGoodsEquipmentRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<GOODS_EQUIPMENT, GoodsEquipmentDTO> _genericRepository;

        public GoodsEquipmentRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_EQUIPMENT, GoodsEquipmentDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsEquipmentDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsEquipments.AsNoTracking()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EQUIPMENT_TYPE).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.MANAGER_ID).DefaultIfEmpty()
                         select new GoodsEquipmentDTO
                        {
                            Id = p.ID,
                            Code = p.CODE,
                            Name = p.NAME,
                            EquipmentType = p.EQUIPMENT_TYPE,
                            EquipmentTypeName = s1.NAME,
                            Manufacturer = p.MANUFACTURER,
                            PurchaseDate = p.PURCHASE_DATE,
                            StatusId = p.STATUS_ID,
                            Status = s2.NAME,
                            WarrantyExpiryDate = p.WARRANTY_EXPIRY_DATE,
                            Cost = p.COST,
                            Address = p.ADDRESS,
                            ManagerId = p.MANAGER_ID,
                            ManagerName = e.FULL_NAME,
                            Note = p.NOTE,  
                        };
        if (pagination.Filter != null)
        {
            if (pagination.Filter.EquipmentType != null)
            {
                joined = joined.AsNoTracking().Where(p => p.EquipmentType == pagination.Filter.EquipmentType);
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

            var joined = await (from p in _dbContext.GoodsEquipments.AsNoTracking().Where(x => x.ID == id)
                                from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EQUIPMENT_TYPE).DefaultIfEmpty()
                                from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty()
                                from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.MANAGER_ID).DefaultIfEmpty()
                                select new GoodsEquipmentDTO
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    Name = p.NAME,
                                    EquipmentType = p.EQUIPMENT_TYPE,
                                    EquipmentTypeName = s1.NAME,
                                    Manufacturer = p.MANUFACTURER,
                                    PurchaseDate = p.PURCHASE_DATE,
                                    StatusId = p.STATUS_ID,
                                    Status = s2.NAME,
                                    WarrantyExpiryDate = p.WARRANTY_EXPIRY_DATE,
                                    Cost = p.COST,
                                    Address = p.ADDRESS,
                                    ManagerId = p.MANAGER_ID,
                                    ManagerName = e.FULL_NAME,
                                    Note = p.NOTE,
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

        public async Task<FormatedResponse> Create(GoodsEquipmentDTO dto, string sid)
        {
            dto.Code = CreateNewCode();
            dto.StatusId = 10042;
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsEquipmentDTO> dtos, string sid)
        {
            var add = new List<GoodsEquipmentDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsEquipmentDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsEquipmentDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetListByTypeCode(string typeCode)
        {
            var res = await (from e in _dbContext.GoodsEquipments.AsNoTracking()
                             from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.CODE == typeCode).DefaultIfEmpty()
                             where e.EQUIPMENT_TYPE == s.ID
                             select new SysOtherListDTO
                             {
                                 Id = e.ID,
                                 Code = e.CODE,
                                 Name = e.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }

        public async Task<FormatedResponse> GetListByTypeId(long id)
        {
            var res = await (from e in _dbContext.GoodsEquipments.AsNoTracking()
                             where e.EQUIPMENT_TYPE == id
                             select new SysOtherListDTO
                             {
                                 Id = e.ID,
                                 Code = e.CODE,
                                 Name = e.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }

        public async Task<FormatedResponse> GetList()
        {
            var res = await (from e in _dbContext.GoodsEquipments.AsNoTracking()
                             select new SysOtherListDTO
                             {
                                 Id = e.ID,
                                 Code = e.CODE,
                                 Name = e.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }

        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.GoodsEquipments.Count() == 0)
            {
                newCode = "TB0001";
            }
            else
            {
                string lastestData = _dbContext.GoodsEquipments.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 4)) + 1).ToString("D3");
            }

            return newCode;

        }
    }
}

