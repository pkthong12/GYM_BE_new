using API;
using GYM_BE.All.SysOtherList;
using GYM_BE.All.SysOtherListType;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;

namespace GYM_BE.All.System.SysOtherList
{
    [ApiExplorerSettings(GroupName = "014-SYSTEM-SYS_OTHER_LIST")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]

    public class SysOtherListController : ControllerBase
    {
        private readonly FullDbContext _dbContext;
        private readonly ISysOtherListRepository _SysOtherListRepository;
        private readonly AppSettings _appSettings;

        public SysOtherListController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _SysOtherListRepository = new SysOtherListRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<SysOtherListDTO> pagination)
        {
            var response = await _SysOtherListRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysOtherListRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysOtherListDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysOtherListDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysOtherListDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysOtherListDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysOtherListDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysOtherListRepository.Delete((long)model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "DELETE_REQUEST_NULL_ID" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _SysOtherListRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListByCode(string typeCode)
        {
            var response = await _SysOtherListRepository.GetListByCode(typeCode);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOtherListByGroup(string code)
        {
            var response = await _SysOtherListRepository.GetOtherListByGroup(code);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListByType(string type, long? id)
        {
            var response = await _SysOtherListRepository.GetListByType(type, id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListRepository.GetAllUser();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult ExportExcelSysOtherList()
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _SysOtherListRepository.ExportExcelSysOtherList();

            return File(result, "application/octet-stream", $"sys_other_list_{dateTimeStr}.xlsx");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ExportedPDFSysOtherList(long id)
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _SysOtherListRepository.ExportedPDFSysOtherList(id);
            if (result == null)
            {
                return Ok(new { StatusCode = 404, StatusMessage = "Not Found!" }); // Handle case when invoice is not found
            }

            return File(result, "application/pdf", $"sys_other_list_{dateTimeStr}.pdf");
        }
    }
}
