using Azure;
using ClosedXML.Excel;
using GYM_BE.All.System.Authentication;
using GYM_BE.All.SysUser;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

namespace GYM_BE.All.System.SysUser
{
    public class SysUserRepository : ISysUserRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<SYS_USER, SysUserDTO> _genericRepository;

        public SysUserRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<SYS_USER, SysUserDTO>(_dbContext);

        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<SysUserDTO> pagination)
        {
            var joined = from l in _dbContext.SysUsers.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == l.GROUP_ID).DefaultIfEmpty()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                         select new SysUserDTO
                         {
                             Id = l.ID,
                             GroupId = l.GROUP_ID,
                             Username = l.USERNAME,
                             Fullname = l.FULLNAME,
                             GroupName = s.NAME,
                             Avatar = l.AVATAR,
                             EmployeeId = l.EMPLOYEE_ID,
                             EmployeeCode = e.CODE
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.GroupId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.GroupId == pagination.Filter.GroupId);
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SYS_USER>
                    {
                        (SYS_USER)response
                    };
                var joined = (from l in list
                              from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == l.GROUP_ID).DefaultIfEmpty()
                              from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                              select new SysUserDTO
                              {
                                  Id = l.ID,
                                  Username = l.USERNAME,
                                  Fullname = l.FULLNAME,
                                  GroupName = s.NAME,
                                  Avatar = l.AVATAR,
                                  EmployeeId = l.EMPLOYEE_ID,
                                  EmployeeCode = e.CODE
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> CreateUser(SysUserCreateUpdateRequest dto, string sid)
        {
            var tryFind = _dbContext.SysUsers.SingleOrDefault(x => x.USERNAME!.ToLower() == dto.Username.ToLower());
            if (dto.EmployeeId != null)
            {
                var checkDataBeforeAdd = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == dto.EmployeeId).ToListAsync();
                if (checkDataBeforeAdd.Count > 0)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "DUBLICATE_VALUE EMPLOYEE",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            if (tryFind != null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "DUBLICATE_VALUE USERNAME",
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }

            if (dto.Password != dto.RePassword)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "PASSWORD_AND_PASSWORD_CONFIRN_DO_NOT_MATCH",
                    StatusCode = EnumStatusCode.StatusCode400
                };

            }

            SysUserDTO entityCreateRequest = new()
            {
                Id = Guid.NewGuid().ToString(),
                Username = dto.Username,
                GroupId = dto.GroupId,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsAdmin = dto.IsAdmin,
                EmployeeId = dto.EmployeeId,
                EmployeeCode = dto.EmployeeCode,
                Fullname = dto.Fullname,
                Decentralization = dto.DecentralizationList == null ? "" : string.Join(",", dto.DecentralizationList)
                //Avatar = avatar
            };
            var response = await _genericRepository.Create(entityCreateRequest, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<SysUserDTO> dtos, string sid)
        {
            var add = new List<SysUserDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(SysUserDTO dto, string sid, bool patchMode = true)
        {
            dto.Decentralization = dto.DecentralizationList == null ? "" : string.Join(",", dto.DecentralizationList);
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<SysUserDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
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

        public Task<FormatedResponse> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ClientsLogin(string UserName, string password)
        {
            try
            {
                UserName = UserName.ToLower().Trim();
                var r = await _dbContext.SysUsers.Where(p => p.USERNAME!.ToLower() == UserName).FirstOrDefaultAsync();
                if (r != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, r.PASSWORDHASH))
                    {
                        var userID = r.ID;
                        var data = new AuthResponse()
                        {
                            Id = r.ID,
                            UserName = r.USERNAME!,
                            Password = r.PASSWORDHASH ?? "",
                            FullName = r.FULLNAME!,
                            IsAdmin = r.IS_ADMIN,
                            IsRoot = r.IS_ROOT,
                            Avatar = r.AVATAR!,
                            EmployeeId = r.EMPLOYEE_ID,
                            IsLock = r.IS_LOCK,
                            Decentralization = r.DECENTRALIZATION != null ? r.DECENTRALIZATION.Split(",").ToList() : new List<string>()
                        };
                        return new FormatedResponse() { InnerBody = data };
                    }
                    else
                    {
                        return new FormatedResponse() { MessageCode = "ERROR_PASSWORD_INCORRECT" };
                    }
                }
                else
                {
                    return new FormatedResponse() { MessageCode = "ERROR_USERNAME_INCORRECT" };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message };
            }

        }

        public Task<FormatedResponse> Create(SysUserDTO dto, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> UpdateUser(SysUserCreateUpdateRequest request, string sid)
        {
            var tryFind = _dbContext.SysUsers.SingleOrDefault(x => x.USERNAME!.ToLower() == request.Username!.ToLower() && x.ID != request.Id);

            if (request.EmployeeId != null)
            {
                var checkDataBeforeAdd = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == request.EmployeeId).ToListAsync();
                if (checkDataBeforeAdd.Count > 1)
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "DUBLICATE_VALUE EMPLOYEE",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            if (tryFind != null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "DUBLICATE_VALUE USERNAME",
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }

            if (request.Id == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "NO_ID_PROVIED",
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }

            if (request.Password != request.RePassword)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = "PASSWORD_AND_PASSWORD_CONFIRN_DO_NOT_MATCH",
                    StatusCode = EnumStatusCode.StatusCode400
                };

            }

            SysUserDTO entityUpdateRequest = new()
            {
                Id = request.Id,
                Username = request.Username,
                GroupId = request.GroupId,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsAdmin = request.IsAdmin,
                EmployeeId = request.EmployeeId,
                EmployeeCode = request.EmployeeCode,
                Fullname = request.Fullname,
                Decentralization = request.DecentralizationList == null ? "" : string.Join(",", request.DecentralizationList)
                //Avatar = avatar
            };
            var response = await _genericRepository.Update(entityUpdateRequest, sid);
            return response;
        }

        public async Task<FormatedResponse> GetByIdString(string id)
        {
            try
            {
                var joined = await (from u in _dbContext.SysUsers.Where(p => p.ID!.ToLower() == id.ToLower())
                                    select new SysUserDTO
                                    {
                                        Id = u.ID,
                                        Username = u.USERNAME,
                                        Fullname = u.FULLNAME,
                                        IsAdmin = u.IS_ADMIN,
                                        IsRoot = u.IS_ROOT,
                                        GroupId = u.GROUP_ID,
                                        IsLock = u.IS_LOCK,
                                        Avatar = u.AVATAR,
                                        Decentralization = u.DECENTRALIZATION,
                                        DecentralizationList = new List<long>()
                                    }).FirstOrDefaultAsync();
                if (joined == null)
                {
                    return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                else
                {
                    if (!joined.Decentralization.IsNullOrEmpty())
                    {
                        var dec = joined.Decentralization!.Split(",").ToList();
                        dec.ForEach(e =>
                        {
                            joined.DecentralizationList!.Add(Convert.ToInt64(e));
                        });
                    }
                    return new FormatedResponse() { InnerBody = joined };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message };
            }

        }

        public byte[] ExportExcelSysUser()
        {
            var inventory = from u in _dbContext.SysUsers.AsNoTracking()
                            from t in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == u.GROUP_ID).DefaultIfEmpty()
                            from e in _dbContext.PerEmployees.AsNoTracking().Where(x => x.ID == u.EMPLOYEE_ID).DefaultIfEmpty()
                            select new 
                            {
                                Username = u.USERNAME,
                                Fullname = u.FULLNAME,
                                EmployeeCode = e.CODE,
                                IsAdmin = u.IS_ADMIN,
                                IsRoot = u.IS_ROOT,
                                GroupName = t.NAME,
                                IsLock = u.IS_LOCK,
                            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SYS_USER");

                worksheet.Range("A1", "H1").Merge();
                worksheet.Range("A1", "H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(1, 1).Value = "Danh sách tài khoản";
                worksheet.Range("A1", "H1").Style.Font.FontSize = 15;
                worksheet.Range("A1", "H1").Style.Font.Bold = true;

                // Đặt header
                worksheet.Cell(3, 1).Value = "STT";
                worksheet.Cell(3, 2).Value = "USERNAME";
                worksheet.Cell(3, 3).Value = "FULLNAME";
                worksheet.Cell(3, 4).Value = "EMPLOYEE CODE";
                worksheet.Cell(3, 5).Value = "ADMIN";
                worksheet.Cell(3, 6).Value = "ROOT";
                worksheet.Cell(3, 7).Value = "GROUP NAME";
                worksheet.Cell(3, 8).Value = "LOCK";

                // Đổ dữ liệu từ danh sách object vào file Excel
                int row = 4;
                int stt = 1;
                foreach (var item in inventory)
                {
                    worksheet.Cell(row, 1).Value = stt;
                    worksheet.Cell(row, 2).Value = item.Username;
                    worksheet.Cell(row, 3).Value = item.Fullname;
                    worksheet.Cell(row, 4).Value = item.EmployeeCode;
                    worksheet.Cell(row, 5).Value = (item.IsAdmin == true ? "Có" : "Không");
                    worksheet.Cell(row, 6).Value = (item.IsRoot == true ? "Có" : "Không");
                    worksheet.Cell(row, 7).Value = item.GroupName;
                    worksheet.Cell(row, 8).Value = (item.IsLock == true ? "Có" : "Không");
                    row++;
                    stt++;
                }
                worksheet.Range("A3", "H" + (row - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "H" + (row - 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "H" + (row - 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "H" + (row - 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // Chỉnh kích thước của các cột
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 40;
                worksheet.Column(4).Width = 40;
                worksheet.Column(5).Width = 30;
                worksheet.Column(6).Width = 30;
                worksheet.Column(7).Width = 40;
                worksheet.Column(8).Width = 30;

                // Lưu workbook vào MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

    }
}
