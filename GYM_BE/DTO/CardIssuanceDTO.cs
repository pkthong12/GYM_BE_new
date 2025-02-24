namespace GYM_BE.DTO
{
    public class CardIssuanceDTO : BaseDTO
    {
        public string? DocumentNumber { get; set; }
        public string? DocumentDate { get; set; }
        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerCode { get; set; }
        public long? CardId { get; set; }
        public string? CardCode { get; set; }
        public string? PracticeTime { get; set; }
        public string? CardTypeName { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public long? HourCard { get; set; }
        public long? HourCardBonus { get; set; }
        public long? TotalHourCard { get; set; }
        public long? PerSellId { get; set; }
        public bool? Wardrobe { get; set; }
        public long? LockerId { get; set; }
        public string? LockerCode { get; set; }
        public bool? IsHavePt { get; set; }
        public long? PerPtId { get; set; }
        public string? PerPtName { get; set; }
        public bool? IsRealPrice { get; set; }
        public long? CardPrice { get; set; }
        public long? PercentDiscount { get; set; }
        public long? AfterDiscount { get; set; }
        public long? PercentVat { get; set; }
        public long? TotalPrice { get; set; }
        public long? MoneyHavePay { get; set; }
        public long? PaidMoney { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsExpired { get; set; }
    }
}

