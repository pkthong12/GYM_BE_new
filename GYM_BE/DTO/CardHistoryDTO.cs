namespace GYM_BE.DTO
{
    public class CardHistoryDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? EffectedDate { get; set; }
        public string? ExpiredDate { get; set; }
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
        public string? Status { get; set; }
        public string? CodeCus { get; set; }
        public int? Action { get; set; }
        public string? ActionStr { get; set; }
        public bool? IsHavePt { get; set; }

        public string? CreatedDateStr { get; set; }


    }
}

