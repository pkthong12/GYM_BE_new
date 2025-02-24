namespace GYM_BE.DTO
{
    public class CardInfoDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? EffectedDate { get; set; }
        public string? ExpiredDate { get; set; }
        public DateTime? EffectedDateTime { get; set; }
        public DateTime? ExpiredDateTime { get; set; }
        public long? CardTypeId { get; set; }
        public string? CardTypeName { get; set; }
        public string? GenderName { get; set; }
        public long? CustomerId { get; set; }
        public bool? Wardrobe { get; set; }
        public long? Price { get; set; }
        public long? ShiftId { get; set; }
        public string? CustomerName { get; set; }
        public string? ShiftName { get; set; }
        public long? LockerId { get; set; }
        public string? LockerName { get; set; }
        public string? Note { get; set; }
        public string? EffectDateString { get; set; }
        public string? ExpiredDateString { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsHavePt { get; set; }
        public string? Status { get; set; }
        public string? CodeCus { get; set; }
    }

    public class CardInfoOutputDTO
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? Wardrobe { get; set; }
        public long? CardPrice { get; set; }
        public int? TotalDay { get; set; }
        public int? HourCard { get; set; }
        public string? PracticeTime { get; set; }
        public bool? IsHavePt { get; set; }
        public string? HoursStart { get; set; }
        public string? HoursEnd { get; set; }
    }

    public class CardInfoPortalDTO : BaseDTO
    {

        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerCode { get; set; }
        public long? CardId { get; set; }
        public string? CardCode { get; set; }
        public string? PracticeTime { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
    }
}
