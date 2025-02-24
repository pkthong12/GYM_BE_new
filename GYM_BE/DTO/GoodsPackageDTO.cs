namespace GYM_BE.DTO
{
    public class GoodsPackageDTO : BaseDTO
    {
        public string? Code { get; set; }
        public decimal? Money { get; set; }
        public double? Period { get; set; }
        public long? ShiftId { get; set; }
        public string? ShiftName { get; set; }
        public string? Description { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
