using API;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.CardInfo
{
    [ApiExplorerSettings(GroupName = "004-CARD-CARD_INFO")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class CardInfoController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ICardInfoRepository _CardInfoRepository;
        private readonly AppSettings _appSettings;

        public CardInfoController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _CardInfoRepository = new CardInfoRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<CardInfoDTO> pagination)
        {
            var response = await _CardInfoRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _CardInfoRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CardInfoDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _CardInfoRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<CardInfoDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _CardInfoRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CardInfoDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _CardInfoRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<CardInfoDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _CardInfoRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CardInfoDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            if (model.Id != null)
            {
                var response = await _CardInfoRepository.DeleteNew((long)model.Id, sid);
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
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _CardInfoRepository.DeleteIdsNew(model.Ids, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _CardInfoRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        } 
        [HttpGet]
        public async Task<IActionResult> GetListCustomer()
        {
            var response = await _CardInfoRepository.GetListCustomer();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCardValid(long? id)
        {
            var response = await _CardInfoRepository.GetAllCardValid(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> CalculateByCardId(long? id)
        {
            var response = await _CardInfoRepository.CalculateByCardId(id);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCardInfoPortal(string code)
        {
            var response = await _CardInfoRepository.GetCardInfoPortal(code);
            return Ok(response);
        }
    }
}

