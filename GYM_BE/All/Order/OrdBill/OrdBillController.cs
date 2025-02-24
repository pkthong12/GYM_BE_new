using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Main;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace GYM_BE.All.OrdBill
{
    [ApiExplorerSettings(GroupName = "027-ORDER-ORD_BILL")]
    [ApiController]
    //[GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class OrdBillController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IOrdBillRepository _OrdBillRepository;
        private readonly AppSettings _appSettings;

        public OrdBillController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _OrdBillRepository = new OrdBillRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<OrdBillDTO> pagination)
        {
            var response = await _OrdBillRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _OrdBillRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrdBillDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _OrdBillRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<OrdBillDTO> models)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _OrdBillRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(OrdBillDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _OrdBillRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<OrdBillDTO> models)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _OrdBillRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(OrdBillDTO model)
        {
            if (model.Id != null)
            {
                var response = await _OrdBillRepository.Delete((long)model.Id);
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
            var response = await _OrdBillRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _OrdBillRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PrintBills(IdsRequest model)
        {
            var result = await _OrdBillRepository.PrintBills(model);
            if (result == null)
            {
                return Ok(new { StatusCode = 404, StatusMessage = "Not Found!" }); // Handle case when invoice is not found
            }

            return File(result.memoryStream!, "application/pdf", $"BILL.pdf");
        }
    }
}

