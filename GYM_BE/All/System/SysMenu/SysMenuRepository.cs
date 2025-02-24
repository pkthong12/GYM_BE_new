using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.SysMenu
{
    public class SysMenuRepository : ISysMenuRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<SYS_MENU, SysMenuDTO> _genericRepository;

        public SysMenuRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<SYS_MENU, SysMenuDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<SysMenuDTO> pagination)
        {
            var joined = from p in _dbContext.SysMenus.AsNoTracking()
                             //tuy chinh
                         select new SysMenuDTO
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
                var list = new List<SYS_MENU>
                    {
                        (SYS_MENU)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SysMenuDTO
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

        public async Task<FormatedResponse> Create(SysMenuDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<SysMenuDTO> dtos, string sid)
        {
            var add = new List<SysMenuDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(SysMenuDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<SysMenuDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> GetAllAction()
        {
            var response = await (from p in _dbContext.SysMenus.AsNoTracking().Where(p=> p.IS_HIDDEN != true)
                                  select new
                                  {
                                      Id = p.ID,
                                      Code = p.NAME+ " ("+p.URL+")",
                                      Name = p.NAME + " (" + p.URL + ")",
                                  }).ToListAsync();
            return new FormatedResponse
            {
                InnerBody = response,
            };
        }

        public async Task<FormatedResponse> GetActionByUser(SysUserDTO userDTO)
        {
            var user = _dbContext.SysUsers.AsNoTracking().SingleOrDefault(p => p.ID == userDTO.Id);
            if (user!.DECENTRALIZATION == null)
            {
                return new FormatedResponse
                {
                    InnerBody = null,
                };
            }

            var de = user.DECENTRALIZATION!.Split(",");
            return new FormatedResponse
            {
                InnerBody = de,
            };
            var action = await (from m in _dbContext.SysMenus.AsNoTracking().Where(m => de.Contains(m.ID.ToString()))
                                orderby m.ID ascending
                                select new SysMenuDTO
                                {
                                    Id = m.ID,
                                    Name = m.NAME,
                                    Url = m.URL,
                                    Parent = m.PARENT,
                                    Icon = m.ICON,
                                    ChildMenu = new List<SysMenuDTO>()
                                }).ToListAsync();
            if (action.Count > 0)
            {
                var actionParent = action.Where(p => p.Parent == null).ToList();
                actionParent.ForEach(p =>
                {
                    p.ChildMenu = action.Where(c=>c.Parent == p.Id).ToList();
                });
                return new FormatedResponse
                {
                    InnerBody = actionParent,
                };
            }
            return new FormatedResponse
            {
                InnerBody = action,
            };
        }
    }
}

