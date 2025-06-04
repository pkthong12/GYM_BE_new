using API;
using Azure;
using GYM_BE.All.GoodsEquipment;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.Report
{
    [ApiExplorerSettings(GroupName = "010-SYSTEM-REPORT")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class ReportController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IReportRepository _ReportRepository;
        private readonly AppSettings _appSettings;

        public ReportController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options,
            IReportRepository reportRepository
            )
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _ReportRepository = reportRepository;
            _appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult TestStore()
        {
            var response = _ReportRepository.TestStore();
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetReport(ReportDTO request)
        {
            DateTime now = DateTime.Now;
            string dateTimeStr = now.ToString("yyyyMMddHHmmss");
            var result = _ReportRepository.GetReport(request);

            return File(result, "application/octet-stream", $"{request.Name}.xlsx");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetStats(ReportDTO request)
        {
            var result = await _ReportRepository.GetStats(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetBarChart(ReportDTO request)
        {
            var result = await _ReportRepository.GetBarChart(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetPieChart(ReportDTO request)
        {
            var result = await _ReportRepository.GetPieChart(request);
            return Ok(result);
        }
    }
}
