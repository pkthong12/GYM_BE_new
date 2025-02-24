using API;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using GYM_BE.Core.DataContract;
using GYM_BE.All.System.Common.Middleware;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GYM_BE.All.Report
{
    [ScopedRegistration]
    public class ReportRepository : IReportRepository
    {
        private readonly FullDbContext _dbContext;
        private AppSettings _appSettings;


        public ReportRepository(FullDbContext context, IOptions<AppSettings> options)
        {
            _dbContext = context;
            _appSettings = options.Value;

        }


        public async Task<FormatedResponse> TestStore()
        {


            // Truy vấn thủ tục SQL để lấy dữ liệu
            string cnnString = _appSettings.ConnectionStrings.CoreDb;
            using SqlConnection cnn = new(cnnString);
            using SqlCommand cmd = new();
            using DataSet ds = new();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PKG_TEST_STORE";

            using SqlDataAdapter da = new(cmd);
            da.Fill(ds);


            var result = ds.Tables[0];
            return new FormatedResponse
            {
                InnerBody = result,
            };
        }


        public byte[] GetReport(ReportDTO request)
        {

            // Truy vấn thủ tục SQL để lấy dữ liệu
            string cnnString = _appSettings.ConnectionStrings.CoreDb;
            using SqlConnection cnn = new(cnnString);
            using SqlCommand cmd = new();
            using DataSet ds = new();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = request.Code;
            string year = "";
            string month = "";
            if (request.Month != null && request.Month != "")
            {
                year = request.Month.Split('-')[0];
                month = request.Month.Split('-')[1];
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "P_MONTH_YEAR",
                    SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), "VarChar", true),
                    Direction = ParameterDirection.Input,
                    Value = request.Month,
                });
            }
            if (request.DayLeft != null)
            {
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "P_DAY_LEFT",
                    SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), "int", true),
                    Direction = ParameterDirection.Input,
                    Value = request.DayLeft,
                });
            }

            using SqlDataAdapter da = new(cmd);
            da.Fill(ds);
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("SHEET1");
            var stream = new MemoryStream();
            switch (request.Code)
            {
                case "PKG_BIRTH_DATE_IN_MONTH_REPORT":
                    {
                        var result = ds.Tables[0].ToList<REPORT_BIRTHDAY>();

                        worksheet.Range("A1", "G1").Merge();
                        worksheet.Range("A1", "G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(1, 1).Value = request.Name + " " + month + "/" + year;
                        worksheet.Range("A1", "G1").Style.Font.FontSize = 15;
                        worksheet.Range("A1", "G1").Style.Font.Bold = true;

                        // Đặt header
                        //worksheet.Cell(3, 1).Value = "STT";
                        //worksheet.Cell(3, 2).Value = "CODE";
                        //worksheet.Cell(3, 3).Value = "FULL NAME";
                        //worksheet.Cell(3, 4).Value = "BIRTH DAY";
                        //worksheet.Cell(3, 5).Value = "GENDER";
                        //worksheet.Cell(3, 6).Value = "PHONE NUMBER";
                        //worksheet.Cell(3, 7).Value = "EMAIL";

                        worksheet.Cell(3, 1).Value = "No";
                        worksheet.Cell(3, 2).Value = "Customer Code";
                        worksheet.Cell(3, 3).Value = "Customer Name";
                        worksheet.Cell(3, 4).Value = "Date Of Birth";
                        worksheet.Cell(3, 5).Value = "Gender";
                        worksheet.Cell(3, 6).Value = "Phone Number";
                        worksheet.Cell(3, 7).Value = "Email";
                        // Đổ dữ liệu từ danh sách object vào file Excel
                        int row = 4;
                        int stt = 1;
                        foreach (var item in result)
                        {
                            worksheet.Cell(row, 1).Value = item.STT;
                            worksheet.Cell(row, 2).Value = item.CODE;
                            worksheet.Cell(row, 3).Value = item.FULL_NAME;
                            worksheet.Cell(row, 4).Value = item.BIRTH_DAY;
                            worksheet.Cell(row, 5).Value = item.GENDER;
                            worksheet.Cell(row, 6).Value = item.PHONE_NUMBER;
                            worksheet.Cell(row, 7).Value = item.EMAIL;
                            row++;
                            stt++;
                        }
                        worksheet.Range("A3", "G" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A3", "G" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A3", "G" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A3", "G" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        // Chỉnh kích thước của các cột
                        worksheet.Column(1).Width = 10;
                        worksheet.Column(2).Width = 20;
                        worksheet.Column(3).Width = 30;
                        worksheet.Column(4).Width = 20;
                        worksheet.Column(5).Width = 20;
                        worksheet.Column(6).Width = 20;
                        worksheet.Column(7).Width = 20;
                        // Lưu workbook vào MemoryStream

                        workbook.SaveAs(stream);

                        break;
                    }
                    

                case "PKG_CARDS_ABOUT_TO_EXPIRE_REPORT":
                    {
                        var result = ds.Tables[0].ToList<CARD_EXPIRE_REPORT>();

                        worksheet.Range("A1", "J1").Merge();
                        worksheet.Range("A1", "J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(1, 1).Value = request.Name;
                        worksheet.Range("A1", "J1").Style.Font.FontSize = 15;
                        worksheet.Range("A1", "J1").Style.Font.Bold = true;

                        // Đặt header
                        //worksheet.Cell(3, 1).Value = "STT";
                        //worksheet.Cell(3, 2).Value = "CARD CODE";
                        //worksheet.Cell(3, 3).Value = "CARD TYPE";
                        //worksheet.Cell(3, 4).Value = "EMPLOYEE CODE";
                        //worksheet.Cell(3, 5).Value = "FULL NAME";
                        //worksheet.Cell(3, 6).Value = "GENDER";
                        //worksheet.Cell(3, 7).Value = "PHONE NUMBER";
                        //worksheet.Cell(3, 8).Value = "EFFECTED DATE";
                        //worksheet.Cell(3, 9).Value = "EXPIRED DATE";
                        //worksheet.Cell(3, 10).Value = "DAY LEFT";


                        worksheet.Cell(3, 1).Value = "No";
                        worksheet.Cell(3, 2).Value = "Card Code";
                        worksheet.Cell(3, 3).Value = "Card Type";
                        worksheet.Cell(3, 4).Value = "Employee Code";
                        worksheet.Cell(3, 5).Value = "Fullname";
                        worksheet.Cell(3, 6).Value = "Gender";
                        worksheet.Cell(3, 7).Value = "Phone Number";
                        worksheet.Cell(3, 8).Value = "Effected Date";
                        worksheet.Cell(3, 9).Value = "Expired Date";
                        worksheet.Cell(3, 10).Value = "Day Left";
                        // Đổ dữ liệu từ danh sách object vào file Excel
                        int row = 4;
                        int stt = 1;
                        foreach (var item in result)
                        {
                            worksheet.Cell(row, 1).Value = item.STT;
                            worksheet.Cell(row, 2).Value = item.CARD_CODE;
                            worksheet.Cell(row, 3).Value = item.CARD_TYPE;
                            worksheet.Cell(row, 4).Value = item.EMPLOYEE_CODE;
                            worksheet.Cell(row, 5).Value = item.FULL_NAME;
                            worksheet.Cell(row, 6).Value = item.GENDER;
                            worksheet.Cell(row, 7).Value = item.PHONE_NUMBER;
                            worksheet.Cell(row, 8).Value = item.EFFECTED_DATE;
                            worksheet.Cell(row, 9).Value = item.EXPIRED_DATE;
                            worksheet.Cell(row, 10).Value = item.DAY_LEFT;
                            row++;
                            stt++;
                        }
                        worksheet.Range("A3", "J" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A3", "J" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A3", "J" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A3", "J" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        // Chỉnh kích thước của các cột
                        worksheet.Column(1).Width = 5;
                        worksheet.Column(2).Width = 20;
                        worksheet.Column(3).Width = 20;
                        worksheet.Column(4).Width = 20;
                        worksheet.Column(5).Width = 20;
                        worksheet.Column(6).Width = 20;
                        worksheet.Column(7).Width = 20;
                        worksheet.Column(8).Width = 20;
                        worksheet.Column(9).Width = 20;
                        worksheet.Column(10).Width = 20;
                        // Lưu workbook vào MemoryStream

                        workbook.SaveAs(stream);

                        break;
                    }
                    


                default:
                    break;


            }
            return stream.ToArray();
        }


    }
}
