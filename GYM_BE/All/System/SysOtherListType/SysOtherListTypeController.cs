using API;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace GYM_BE.All.SysOtherListType
{
    [ApiExplorerSettings(GroupName = "005-SYSTEM-SYS_OTHER_LIST_TYPE")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysOtherListTypeController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ISysOtherListTypeRepository _SysOtherListTypeRepository;
        private readonly AppSettings _appSettings;

        public SysOtherListTypeController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _SysOtherListTypeRepository = new SysOtherListTypeRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var response = await _SysOtherListTypeRepository.GetList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<SysOtherListTypeDTO> pagination)
        {
            var response = await _SysOtherListTypeRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysOtherListTypeRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysOtherListTypeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListTypeRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysOtherListTypeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListTypeRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysOtherListTypeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListTypeRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysOtherListTypeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListTypeRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysOtherListTypeDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysOtherListTypeRepository.Delete((long)model.Id);
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
            var response = await _SysOtherListTypeRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysOtherListTypeRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

    }
}

