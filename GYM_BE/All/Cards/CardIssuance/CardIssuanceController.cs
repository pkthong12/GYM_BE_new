using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Main;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GYM_BE.All.System.Common.Middleware;

namespace GYM_BE.All.CardIssuance
{
    [ApiExplorerSettings(GroupName = "004-CARD-CARD_ISSUANCE")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class CardIssuanceController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ICardIssuanceRepository _CardIssuanceRepository;
        private readonly AppSettings _appSettings;

        public CardIssuanceController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _CardIssuanceRepository = new CardIssuanceRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<CardIssuanceDTO> pagination)
        {
            var response = await _CardIssuanceRepository.QueryList(pagination);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _CardIssuanceRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CardIssuanceDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _CardIssuanceRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<CardIssuanceDTO> models)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _CardIssuanceRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CardIssuanceDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _CardIssuanceRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<CardIssuanceDTO> models)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _CardIssuanceRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CardIssuanceDTO model)
        {
            if (model.Id != null)
            {
                var response = await _CardIssuanceRepository.Delete((long)model.Id);
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
            var response = await _CardIssuanceRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _CardIssuanceRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

    }
}

