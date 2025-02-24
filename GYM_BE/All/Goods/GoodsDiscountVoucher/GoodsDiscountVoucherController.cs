using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.GoodsDiscountVoucher
{
    [ApiExplorerSettings(GroupName = "016-GOODS-GOODS_DISCOUNT_VOUCHER")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GoodsDiscountVoucherController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGoodsDiscountVoucherRepository _GoodsDiscountVoucherRepository;
        private readonly AppSettings _appSettings;

        public GoodsDiscountVoucherController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GoodsDiscountVoucherRepository = new GoodsDiscountVoucherRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GoodsDiscountVoucherDTO> pagination)
        {
            var response = await _GoodsDiscountVoucherRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GoodsDiscountVoucherRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GoodsDiscountVoucherDTO model)
        {
            var response = await _GoodsDiscountVoucherRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GoodsDiscountVoucherDTO> models)
        {
            var response = await _GoodsDiscountVoucherRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GoodsDiscountVoucherDTO model)
        {
            var response = await _GoodsDiscountVoucherRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GoodsDiscountVoucherDTO> models)
        {
            var response = await _GoodsDiscountVoucherRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GoodsDiscountVoucherDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GoodsDiscountVoucherRepository.Delete((long)model.Id);
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
            var response = await _GoodsDiscountVoucherRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _GoodsDiscountVoucherRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }

    }
}

