using API;
using Azure;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.Profile.PerCustomer
{
    [ApiExplorerSettings(GroupName = "001-PERSONAL-PER_CUSTOMER")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class PerCustomerController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IPerCustomerRepository _PerCustomerRepository;
        private readonly AppSettings _appSettings;

        public PerCustomerController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _PerCustomerRepository = new PerCustomerRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<PerCustomerDTO> pagination)
        {
            var response = await _PerCustomerRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PerCustomerRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PerCustomerDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PerCustomerRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PerCustomerDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PerCustomerRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PerCustomerDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PerCustomerRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PerCustomerDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PerCustomerRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PerCustomerDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PerCustomerRepository.Delete((long)model.Id);
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
            var response = await _PerCustomerRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PerCustomerRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult ExportExcelPerCustomer()
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _PerCustomerRepository.ExportExcelPerCustomer();

            return File(result, "application/force-download", $"per_customer_{dateTimeStr}.xlsx");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            var response =await _PerCustomerRepository.GetAllCustomer();
            return Ok(response);
        }
    }
}

