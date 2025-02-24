using Azure.Core;
using ClosedXML.Excel;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace GYM_BE.All.Profile.PerCustomer
{
    public class PerCustomerRepository : IPerCustomerRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<PER_CUSTOMER, PerCustomerDTO> _genericRepository;

        public PerCustomerRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<PER_CUSTOMER, PerCustomerDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<PerCustomerDTO> pagination)
        {
            var joined = from p in _dbContext.PerCustomers.AsNoTracking()
                         from e in _dbContext.PerEmployees.Where(x => x.ID == p.PER_PT_ID).DefaultIfEmpty()
                         from gr in _dbContext.SysOtherLists.Where(x => x.ID == p.CUSTOMER_CLASS_ID).DefaultIfEmpty()
                         from gender in _dbContext.SysOtherLists.Where(x => x.ID == p.GENDER_ID).DefaultIfEmpty()
                         from nav in _dbContext.SysOtherLists.Where(x => x.ID == p.NATIVE_ID).DefaultIfEmpty()
                         from re in _dbContext.SysOtherLists.Where(x => x.ID == p.RELIGION_ID).DefaultIfEmpty()
                         from b in _dbContext.SysOtherLists.Where(x => x.ID == p.BANK_ID).DefaultIfEmpty()
                         from bb in _dbContext.SysOtherLists.Where(x => x.ID == p.BANK_BRANCH).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                         select new PerCustomerDTO
                         {
                             Id = p.ID,
                             Avatar = p.AVATAR,
                             FullName = p.FULL_NAME,
                             Code = p.CODE,
                             IdNo = p.ID_NO,
                             CustomerClassId = p.CUSTOMER_CLASS_ID,
                             CustomerClassName = gr.NAME,
                             BirthDate = p.BIRTH_DATE,
                             BirthDateString = p.BIRTH_DATE!,
                             GenderId = p.GENDER_ID,
                             GenderName = gender.NAME,
                             Address = p.ADDRESS,
                             PhoneNumber = p.PHONE_NUMBER,
                             Email = p.EMAIL,
                             NativeId = p.NATIVE_ID,
                             NativeName = nav.NAME,
                             ReligionId = p.RELIGION_ID,
                             ReligionName = re.NAME,
                             BankId = p.BANK_ID,
                             BankName = b.NAME,
                             BankBranch = p.BANK_BRANCH,
                             BankBranchName = bb.NAME,
                             BankNo = p.BANK_NO,
                             IsGuestPass = p.IS_GUEST_PASS,
                             JoinDate = p.JOIN_DATE,
                             Height = p.HEIGHT,
                             Weight = p.WEIGHT,
                             CardId = p.CARD_ID,
                             ExpireDate = p.EXPIRE_DATE,
                             GymPackageId = p.GYM_PACKAGE_ID,
                             PerPtId = p.PER_PT_ID,
                             PerPtName = e.FULL_NAME,
                             PerSaleId = p.PER_SALE_ID,
                             Note = p.NOTE,
                             Status = s.NAME
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.CustomerClassId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.CustomerClassId == pagination.Filter.CustomerClassId);
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
            var joined = await (from l in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == id)
                                from gr in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.CUSTOMER_CLASS_ID).DefaultIfEmpty()
                                from gender in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.GENDER_ID).DefaultIfEmpty()
                                from nav in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.NATIVE_ID).DefaultIfEmpty()
                                from re in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.RELIGION_ID).DefaultIfEmpty()
                                from b in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.BANK_ID).DefaultIfEmpty()
                                from bb in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.BANK_BRANCH).DefaultIfEmpty()
                                select new PerCustomerDTO
                                {
                                    Id = l.ID,
                                    Avatar = l.AVATAR,
                                    FullName = l.FULL_NAME,
                                    Code = l.CODE,
                                    CustomerClassId = l.CUSTOMER_CLASS_ID,
                                    CustomerClassName = gr.NAME,
                                    IdNo = l.ID_NO,
                                    BirthDate = l.BIRTH_DATE,
                                    GenderId = l.GENDER_ID,
                                    GenderName = gender.NAME,
                                    Address = l.ADDRESS,
                                    PhoneNumber = l.PHONE_NUMBER,
                                    Email = l.EMAIL,
                                    NativeId = l.NATIVE_ID,
                                    NativeName = nav.NAME,
                                    ReligionId = l.RELIGION_ID,
                                    ReligionName = re.NAME,
                                    BankId = l.BANK_ID,
                                    BankName = b.NAME,
                                    BankBranch = l.BANK_BRANCH,
                                    BankBranchName = bb.NAME,
                                    BankNo = l.BANK_NO,
                                    IsGuestPass = l.IS_GUEST_PASS,
                                    JoinDate = l.JOIN_DATE,
                                    Height = l.HEIGHT,
                                    Weight = l.WEIGHT,
                                    CardId = l.CARD_ID,
                                    ExpireDate = l.EXPIRE_DATE,
                                    GymPackageId = l.GYM_PACKAGE_ID,
                                    PerPtId = l.PER_PT_ID,
                                    PerSaleId = l.PER_SALE_ID,
                                    StatusId = l.STATUS_ID,
                                    Note = l.NOTE,
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

        public async Task<FormatedResponse> Create(PerCustomerDTO dto, string sid)
        {
            dto.IsActive = true;
            dto.Code = CreateNewCode();
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<PerCustomerDTO> dtos, string sid)
        {
            var add = new List<PerCustomerDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(PerCustomerDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<PerCustomerDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.ToggleActiveIds(ids, valueToBind, sid);
            return response;
        }

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetAllCustomer()
        {
            var joined = await (from c in _dbContext.PerCustomers.AsNoTracking()
                                  select new
                                  {
                                      Id = c.ID,
                                      Name =c.CODE +" - "+ c.FULL_NAME
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.PerCustomers.Count() == 0)
            {
                newCode = "CUS001";
            }
            else
            {
                string lastestData = _dbContext.PerCustomers.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return newCode;

        }

        public byte[] ExportExcelPerCustomer()
        {
            var inventory = from p in _dbContext.PerCustomers.AsNoTracking()
                            from e in _dbContext.PerEmployees.Where(x => x.ID == p.PER_PT_ID).DefaultIfEmpty()
                            from gr in _dbContext.SysOtherLists.Where(x => x.ID == p.CUSTOMER_CLASS_ID).DefaultIfEmpty()
                            from gender in _dbContext.SysOtherLists.Where(x => x.ID == p.GENDER_ID).DefaultIfEmpty()
                            from nav in _dbContext.SysOtherLists.Where(x => x.ID == p.NATIVE_ID).DefaultIfEmpty()
                            from re in _dbContext.SysOtherLists.Where(x => x.ID == p.RELIGION_ID).DefaultIfEmpty()
                            from b in _dbContext.SysOtherLists.Where(x => x.ID == p.BANK_ID).DefaultIfEmpty()
                            from bb in _dbContext.SysOtherLists.Where(x => x.ID == p.BANK_BRANCH).DefaultIfEmpty()
                            from s in _dbContext.SysOtherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                            select new
                            {
                                FullName = p.FULL_NAME,
                                Code = p.CODE,
                                IdNo = p.ID_NO,
                                CustomerClassName = gr.NAME,
                                BirthDate = p.BIRTH_DATE,
                                BirthDateString = p.BIRTH_DATE!,
                                GenderName = gender.NAME,
                                Address = p.ADDRESS,
                                PhoneNumber = p.PHONE_NUMBER,
                                Email = p.EMAIL,
                                NativeName = nav.NAME,
                                ReligionName = re.NAME,
                                BankName = b.NAME,
                                BankBranch = p.BANK_BRANCH,
                                BankBranchName = bb.NAME,
                                BankNo = p.BANK_NO,
                                IsGuestPass = p.IS_GUEST_PASS,
                                JoinDate = p.JOIN_DATE,
                                Height = p.HEIGHT,
                                Weight = p.WEIGHT,
                                CardId = p.CARD_ID,
                                ExpireDate = p.EXPIRE_DATE,
                                GymPackageId = p.GYM_PACKAGE_ID,
                                PerPtName = e.FULL_NAME,
                                PerSaleId = p.PER_SALE_ID,
                                Note = p.NOTE,
                                Status = s.NAME
                            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PER_CUSTOMER");

                worksheet.Range("A1", "L1").Merge();
                worksheet.Range("A1", "L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(1, 1).Value = "Danh sách hội viên";
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
                    //worksheet.Cell(row, 10).Value = item.StaffGroupName;
                    //worksheet.Cell(row, 11).Value = item.StatusName;
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

