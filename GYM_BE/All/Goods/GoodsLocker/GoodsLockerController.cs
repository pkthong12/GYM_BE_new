using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Main;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GYM_BE.All.System.Common.Middleware;

namespace GYM_BE.All.GoodsLocker
{
    [ApiExplorerSettings(GroupName = "019-GOODS-GOODS_LOCKER")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class GoodsLockerController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGoodsLockerRepository _GoodsLockerRepository;
        private readonly AppSettings _appSettings;

        public GoodsLockerController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GoodsLockerRepository = new GoodsLockerRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GoodsLockerDTO> pagination)
        {
            var response = await _GoodsLockerRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GoodsLockerRepository.GetById(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLockerValid(long? id, long? cusId, long cardId)
        {
            var response = await _GoodsLockerRepository.GetAllLockerValid(id, cusId, cardId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GoodsLockerDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _GoodsLockerRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GoodsLockerDTO> models)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _GoodsLockerRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GoodsLockerDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _GoodsLockerRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GoodsLockerDTO> models)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _GoodsLockerRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GoodsLockerDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GoodsLockerRepository.Delete((long)model.Id);
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
            var response = await _GoodsLockerRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
             var sid = Request.Sid(_appSettings);
             if (sid == null) return Unauthorized();
            var response = await _GoodsLockerRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetLockerStatus(long area)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _GoodsLockerRepository.GetLockerStatus(area);
            return Ok(response);
        }
    }
}

