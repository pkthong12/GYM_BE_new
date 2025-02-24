using API;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.SysMenu
{
    [ApiExplorerSettings(GroupName = "046-SYSTEM-SYS_MENU")]
    [ApiController]
    //[GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysMenuController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ISysMenuRepository _SysMenuRepository;
        private readonly AppSettings _appSettings;

        public SysMenuController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _SysMenuRepository = new SysMenuRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<SysMenuDTO> pagination)
        {
            var response = await _SysMenuRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysMenuRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysMenuDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.Create(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysMenuDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.CreateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysMenuDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.Update(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysMenuDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.UpdateRange(models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysMenuDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysMenuRepository.Delete((long)model.Id);
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
            var response = await _SysMenuRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.ToggleActiveIds(model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetActionByUser(SysUserDTO userDTO)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.GetActionByUser(userDTO);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAction()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysMenuRepository.GetAllAction();
            return Ok(response);
        }
    }
}

