using ClosedXML.Excel;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.PerEmployee
{
    public class PerEmployeeRepository : IPerEmployeeRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<PER_EMPLOYEE, PerEmployeeDTO> _genericRepository;

        public PerEmployeeRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<PER_EMPLOYEE, PerEmployeeDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<PerEmployeeDTO> pagination)
        {
            var joined = from p in _dbContext.PerEmployees.AsNoTracking()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1 => s1.ID == p.GENDER_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s2=> s2.ID == p.STAFF_GROUP_ID).DefaultIfEmpty()
                         from stt in _dbContext.SysOtherLists.AsNoTracking().Where(stt=> stt.ID == p.STATUS_ID).DefaultIfEmpty()

                             //tuy chinh
                         select new PerEmployeeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             FullName = p.FULL_NAME,
                             IdNo = p.ID_NO,
                             Address = p.ADDRESS,
                             BirthDate = p.BIRTH_DATE,
                             PhoneNumber = p.PHONE_NUMBER,
                             Email = p.EMAIL,
                             GenderId = p.GENDER_ID,
                             GenderName = s1.NAME,
                             StaffGroupId = p.STAFF_GROUP_ID,
                             StaffGroupName = s2.NAME,
                             StatusId = p.STATUS_ID,
                             StatusName = stt.NAME,
                             Note = p.NOTE,
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.StaffGroupId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.StaffGroupId == pagination.Filter.StaffGroupId);
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
                var list = new List<PER_EMPLOYEE>
                    {
                        (PER_EMPLOYEE)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PerEmployeeDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  FullName = l.FULL_NAME,
                                  IdNo = l.ID_NO,
                                  Address = l.ADDRESS,
                                  BirthDate = l.BIRTH_DATE,
                                  PhoneNumber = l.PHONE_NUMBER,
                                  Email = l.EMAIL,
                                  GenderId = l.GENDER_ID,
                                  StaffGroupId = l.STAFF_GROUP_ID,
                                  StatusId = l.STATUS_ID,
                                  Note = l.NOTE,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(PerEmployeeDTO dto, string sid)
        {
            dto.Code = CreateNewCode();
            var stt = await _dbContext.SysOtherLists.AsNoTracking().FirstAsync(p => p.CODE == "ESW");
            dto.StatusId = stt.ID;
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<PerEmployeeDTO> dtos, string sid)
        {
            var add = new List<PerEmployeeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(PerEmployeeDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<PerEmployeeDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.PerEmployees.Count() == 0)
            {
                newCode = "GYM001";
            }
            else
            {
                string lastestData = _dbContext.PerEmployees.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return newCode;

        }

        public byte[] ExportExcelPerEmployee()
        {
            var inventory = from p in _dbContext.PerEmployees.AsNoTracking()
                            from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1 => s1.ID == p.GENDER_ID).DefaultIfEmpty()
                            from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s2 => s2.ID == p.STAFF_GROUP_ID).DefaultIfEmpty()
                            from stt in _dbContext.SysOtherLists.AsNoTracking().Where(stt => stt.ID == p.STATUS_ID).DefaultIfEmpty()
                            select new
                            {
                                Code = p.CODE,
                                FullName = p.FULL_NAME,
                                IdNo = p.ID_NO,
                                Address = p.ADDRESS,
                                BirthDate = p.BIRTH_DATE,
                                PhoneNumber = p.PHONE_NUMBER,
                                Email = p.EMAIL,
                                GenderName = s1.NAME,
                                StaffGroupName = s2.NAME,
                                StatusName = stt.NAME,
                                Note = p.NOTE,
                            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PER_EMPLOYEE");

                worksheet.Range("A1", "L1").Merge();
                worksheet.Range("A1", "L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(1, 1).Value = "Danh sách nhân viên";
                worksheet.Range("A1", "L1").Style.Font.FontSize = 15;
                worksheet.Range("A1", "L1").Style.Font.Bold = true;

                // Đặt header
                worksheet.Cell(3, 1).Value = "STT";
                worksheet.Cell(3, 2).Value = "CODE";
                worksheet.Cell(3, 3).Value = "FULL NAME";
                worksheet.Cell(3, 4).Value = "ID NO";
                worksheet.Cell(3, 5).Value = "ADDRESS";
                worksheet.Cell(3, 6).Value = "BIRTH DATE";
                worksheet.Cell(3, 7).Value = "PHONE NUMBER";
                worksheet.Cell(3, 8).Value = "EMAIL";
                worksheet.Cell(3, 9).Value = "GENDER";
                worksheet.Cell(3, 10).Value = "STAFF GROUP";
                worksheet.Cell(3, 11).Value = "STATUS";
                worksheet.Cell(3, 12).Value = "NOTE";

                // Đổ dữ liệu từ danh sách object vào file Excel
                int row = 4;
                int stt = 1;
                foreach (var item in inventory)
                {
                    worksheet.Cell(row, 1).Value = stt;
                    worksheet.Cell(row, 2).Value = item.Code;
                    worksheet.Cell(row, 3).Value = item.FullName;
                    worksheet.Cell(row, 4).Value = item.IdNo;
                    worksheet.Cell(row, 5).Value = item.Address;
                    worksheet.Cell(row, 6).Value = item.BirthDate;
                    worksheet.Cell(row, 7).Value = item.PhoneNumber;
                    worksheet.Cell(row, 8).Value = item.Email;
                    worksheet.Cell(row, 9).Value = item.GenderName;
                    worksheet.Cell(row, 10).Value = item.StaffGroupName;
                    worksheet.Cell(row, 11).Value = item.StatusName;
                    worksheet.Cell(row, 12).Value = item.Note;
                    row++;
                    stt++;
                }
                worksheet.Range("A3", "L" + (row - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "L" + (row - 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "L" + (row - 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "L" + (row - 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // Chỉnh kích thước của các cột
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 40;
                worksheet.Column(4).Width = 40;
                worksheet.Column(5).Width = 40;
                worksheet.Column(6).Width = 30;
                worksheet.Column(7).Width = 30;
                worksheet.Column(8).Width = 30;
                worksheet.Column(9).Width = 30;
                worksheet.Column(10).Width = 30;
                worksheet.Column(11).Width = 30;
                worksheet.Column(12).Width = 30;

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

