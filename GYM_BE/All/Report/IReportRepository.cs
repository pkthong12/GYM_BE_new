using GYM_BE.Core.Dto;
using GYM_BE.DTO;

namespace GYM_BE.All.Report
{
    public interface IReportRepository
    {
        Task<FormatedResponse> TestStore();
        byte[] GetReport(ReportDTO request);

    }
}
