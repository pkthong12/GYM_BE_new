using GYM_BE.Core.Dto;
using GYM_BE.DTO;

namespace GYM_BE.All.Report
{
    public interface IReportRepository
    {
        Task<FormatedResponse> TestStore();

        Task<FormatedResponse> GetStats(ReportDTO request);

        Task<FormatedResponse> GetBarChart(ReportDTO request);

        Task<FormatedResponse> GetPieChart(ReportDTO request);
        byte[] GetReport(ReportDTO request);

    }
}
