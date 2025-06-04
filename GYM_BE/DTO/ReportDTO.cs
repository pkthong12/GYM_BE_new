namespace GYM_BE.DTO
{
    public class ReportDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? Month { get; set; }
        public string? Year { get; set; }

        public int? DayLeft { get; set; }
        public int? Type { get; set; }
    }
}
