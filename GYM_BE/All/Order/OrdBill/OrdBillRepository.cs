﻿using API;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Extentions;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Linq;

namespace GYM_BE.All.OrdBill
{
    public class OrdBillRepository : IOrdBillRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<ORD_BILL, OrdBillDTO> _genericRepository;
        private AppSettings _appSettings;
        public OrdBillRepository(FullDbContext context, IOptions<AppSettings> options)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<ORD_BILL, OrdBillDTO>(_dbContext);
            _appSettings = options.Value;
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<OrdBillDTO> pagination)
        {
            var joined = from p in _dbContext.OrdBills.AsNoTracking()
                         from c in _dbContext.PerCustomers.AsNoTracking().Where(c=> c.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                         from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.ID == p.PAY_METHOD).DefaultIfEmpty()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.PER_SELL_ID).DefaultIfEmpty()
                         from t in _dbContext.SysOtherLists.AsNoTracking().Where(t => t.ID == p.TYPE_TRANSFER).DefaultIfEmpty()
                         from v in _dbContext.GoodsDiscountVouchers.AsNoTracking().Where(v=> v.ID == p.VOUCHER_ID).DefaultIfEmpty()
                         orderby  p.CREATED_DATE descending

                             //tuy chinh
                         select new OrdBillDTO
                         {
                             Id = p.ID,
                             Code= p.CODE,
                             CreatedDate = p.CREATED_DATE,
                             MoneyHavePay= p.MONEY_HAVE_PAY,
                             TotalMoney = p.TOTAL_MONEY,
                             DiscPercent= p.DISC_PERCENT,
                             PercentVat = p.PERCENT_VAT,
                             VoucherId= p.VOUCHER_ID,
                             TypeTransfer= p.TYPE_TRANSFER,
                             TypeTransferName  = t.NAME,
                             CustomerName = c.FULL_NAME,
                             PerSellName= e.FULL_NAME,
                             PayMethodName = o.NAME,
                             IsConfirm = p.IS_CONFIRM,
                             Printed = p.PRINTED,
                             PrintNumber = p.PRINT_NUMBER,
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
                var list = new List<ORD_BILL>
                    {
                        (ORD_BILL)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new OrdBillDTO
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

        public async Task<FormatedResponse> Create(OrdBillDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<OrdBillDTO> dtos, string sid)
        {
            var add = new List<OrdBillDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(OrdBillDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<OrdBillDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<ResultMemory> PrintBills(IdsRequest model, string sid)
        {
            // Truy vấn thủ tục SQL để lấy dữ liệu
            string cnnString = _appSettings.ConnectionStrings.CoreDb;
            using SqlConnection cnn = new(cnnString);
            using SqlCommand cmd = new();
            using DataSet dataset = new();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PKG_PRINT_BILLS";
            if (model.Ids != null)
            {
               
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "P_IDS",
                    SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), "VarChar", true),
                    Direction = ParameterDirection.Input,
                    Value = string.Join(",", model.Ids),
                });
            }
            if (sid != null)
            {
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "P_SID",
                    SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), "VarChar", true),
                    Direction = ParameterDirection.Input,
                    Value =  sid,
                });
            }

            using SqlDataAdapter da = new(cmd);
            da.Fill(dataset);
            dataset.Tables[0].TableName = "DATA";
            var memory = HttpRequestExtensions.FillTemplatePDF(EnumStatic.ORER_BILL, dataset);
            return new ResultMemory() {memoryStream = memory };
        }
    }
}

