using GYM_BE.All.SysOtherList;
using GYM_BE.All.SysOtherListType;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using ClosedXML.Excel;
using iText.Layout.Properties;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf;

namespace GYM_BE.All.System.SysOtherList
{
    public class SysOtherListRepository : ISysOtherListRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<SYS_OTHER_LIST, SysOtherListDTO> _genericRepository;

        public SysOtherListRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<SYS_OTHER_LIST, SysOtherListDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<SysOtherListDTO> pagination)
        {
            var joined = from p in _dbContext.SysOtherLists.AsNoTracking()
                         from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                         select new SysOtherListDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             TypeId = p.TYPE_ID,
                             TypeName = t.NAME,
                             Note = p.NOTE,
                             Orders = p.ORDERS,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng"
                         };
            if (pagination.Filter != null)
            {
                if (pagination.Filter.TypeId != null)
                {
                    joined = joined.AsNoTracking().Where(p => p.TypeId == pagination.Filter.TypeId);
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
                var list = new List<SYS_OTHER_LIST>
                    {
                        (SYS_OTHER_LIST)response
                    };
                var joined = (from l in list
                              from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == l.TYPE_ID).DefaultIfEmpty()
                              select new SysOtherListDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  TypeId = l.TYPE_ID,
                                  TypeName = t.NAME,
                                  Note = l.NOTE,
                                  Orders = l.ORDERS,
                                  IsActive = l.IS_ACTIVE,
                                  Status = l.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng"
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(SysOtherListDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<SysOtherListDTO> dtos, string sid)
        {
            var add = new List<SysOtherListDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(SysOtherListDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<SysOtherListDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetListByCode(string typeCode)
        {
            var res = await (from p in _dbContext.SysOtherLists.AsNoTracking()
                             from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                             where p.IS_ACTIVE == true && t.CODE == typeCode
                             select new SysOtherListDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }

        public async Task<FormatedResponse> GetOtherListByGroup(string code)
        {
            if (code == null || code.Trim().Length == 0)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
            var response = await (from p in _dbContext.SysOtherLists.AsNoTracking()
                                  from o in _dbContext.SysOtherListTypes.Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                                  where p.IS_ACTIVE == true && o.CODE == code
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = p.NAME,
                                      Code = p.CODE,
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetListByType(string type, long? id)
        {
            var res = await (from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(t => t.CODE!.ToUpper() == type.ToUpper())
                             from p in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == t.ID && x.IS_ACTIVE == true).DefaultIfEmpty()
                             select new
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                             }).ToListAsync();
            if (id != null)
            {
                var x = await (from p in _dbContext.SysOtherLists.Where(p => p.ID == id)
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                               }).FirstOrDefaultAsync();
                if (x != null)
                {
                    var check = res.Find(p => p.Id == x.Id);
                    if (check == null)
                    {
                        res.Add(x);
                        res.OrderBy(p => p.Id);
                    }
                }
            }
            return new FormatedResponse
            {
                InnerBody = res,
            };
        }
        public async Task<FormatedResponse> GetAllUser()
        {
            var res = await (from u in _dbContext.SysUsers.AsNoTracking()
                             select new
                             {
                                 Id = u.ID,
                                 Code = u.USERNAME!,
                                 Name = u.USERNAME!,
                             }).ToListAsync();
            return new FormatedResponse
            {
                InnerBody = res,
            };
        }

        public byte[] ExportExcelSysOtherList()
        {
            var inventory = from p in _dbContext.SysOtherLists.AsNoTracking()
                             from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                             select new SysOtherListDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 TypeId = p.TYPE_ID,
                                 TypeName = t.NAME,
                                 Note = p.NOTE,
                                 Orders = p.ORDERS,
                                 IsActive = p.IS_ACTIVE,
                                 Status = p.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng"
                             };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SYS_OTHER_LIST");

                worksheet.Range("A1", "G1").Merge();
                worksheet.Range("A1", "G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(1, 1).Value = "Danh sách tham số hệ thống";
                worksheet.Range("A1", "G1").Style.Font.FontSize = 15;
                worksheet.Range("A1", "G1").Style.Font.Bold = true;

                // Đặt header
                worksheet.Cell(3, 1).Value = "STT";
                worksheet.Cell(3, 2).Value = "CODE";
                worksheet.Cell(3, 3).Value = "NAME";
                worksheet.Cell(3, 4).Value = "TYPE NAME";
                worksheet.Cell(3, 5).Value = "NOTE";
                worksheet.Cell(3, 6).Value = "ORDERS";
                worksheet.Cell(3, 7).Value = "STATUS";

                // Đổ dữ liệu từ danh sách object vào file Excel
                int row = 4;
                int stt = 1;
                foreach (var item in inventory)
                {
                    worksheet.Cell(row, 1).Value = stt;
                    worksheet.Cell(row, 2).Value = item.Code;
                    worksheet.Cell(row, 3).Value = item.Name;
                    worksheet.Cell(row, 4).Value = item.TypeName;
                    worksheet.Cell(row, 5).Value = item.Note;
                    worksheet.Cell(row, 6).Value = item.Orders;
                    worksheet.Cell(row, 7).Value = item.Status;
                    row++;
                    stt++;
                }
                worksheet.Range("A3", "G" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "G" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "G" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3", "G" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // Chỉnh kích thước của các cột
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 40;
                worksheet.Column(4).Width = 40;
                worksheet.Column(5).Width = 40;
                worksheet.Column(6).Width = 30;
                worksheet.Column(7).Width = 30;

                // Lưu workbook vào MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ExportedPDFSysOtherList(long typeId)
        {
            var type = (from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == typeId)
                        select new 
                        {
                            Id = t.ID,
                            Code = t.CODE,
                            Name = t.NAME,
                        }).FirstOrDefault();

            var otherLists = from l in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == type!.Id).DefaultIfEmpty()
                             from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == type!.Id).DefaultIfEmpty()
                         select new SysOtherListDTO
                         {
                             Id = l.ID,
                             Code = l.CODE,
                             Name = l.NAME,
                             TypeId = l.TYPE_ID,
                             TypeName = t.NAME,
                             Note = l.NOTE,
                             Orders = l.ORDERS,
                             IsActive = l.IS_ACTIVE,
                             Status = l.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng",
                             CreatedDate = l.CREATED_DATE,
                             CreatedBy = l.CREATED_BY,
                             UpdatedBy = l.UPDATED_BY,
                             UpdatedDate = l.UPDATED_DATE,
                         };

            

            if (otherLists == null)
            {
                // Handle case when invoice is not found
                return null;
            }

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new Document(pdf);

                // Add invoice information
                document.Add(new Paragraph($"Transaction invoice:"));
                document.Add(new Paragraph($"Code: {type!.Code}"));
                document.Add(new Paragraph($"Type Name: {type!.Name}"));


                // Create a table with 5 columns
                var table = new Table(5);
                table.SetWidth(UnitValue.CreatePercentValue(100));

                // Add header row
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Code")));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Name")));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Note")));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Order")));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Status")));

                // Add invoice details
                foreach (var otherList in otherLists)
                {
                    // first column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(otherList.Code == null ? "" : otherList.Code)));
                    // second column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(otherList.Name == null ? "" : otherList.Name)));
                    // third column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(otherList.Note == null ? "" : otherList.Note)));
                    // fourth column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(otherList.Orders.ToString())));
                    // fourth column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(otherList.Status == null ? "" : otherList.Status)));
                }

                // Add the table to the document
                document.Add(table);

                // Close the document
                document.Close();

                return stream.ToArray();
            }
        }

    }
}
