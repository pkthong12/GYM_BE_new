namespace GYM_BE.DTO
{
    public class GoodsShiftDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? TotalDays { get; set; }
        public string? HoursStart { get; set; }
        public string? HoursStartString { get; set; }
        public string? HoursEnd { get; set; }
        public string? HoursEndString { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
