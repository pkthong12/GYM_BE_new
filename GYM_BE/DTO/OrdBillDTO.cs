namespace GYM_BE.DTO
{
    public class OrdBillDTO : BaseDTO
    {
       public long? TypeTransfer  { get; set; }
       public string? TypeTransferName  { get; set; }

       public long? CustomerId  { get; set; }
       public string? CustomerName  { get; set; }

       public long? PerSellId  { get; set; }
       public string? PerSellName  { get; set; }

       public long? MoneyHavePay  { get; set; }

       public long? TotalMoney  { get; set; }

       public long? PayMethod  { get; set; }
       public string? PayMethodName  { get; set; }

       public decimal? DiscPercent  { get; set; }
       public decimal? PercentVat { get; set; }
       public string? Code  { get; set; }
       public long? PkRef { get; set; }
       public long? VoucherId { get; set; }
       public long? VoucherDiscountPer { get; set; }
        public bool? IsConfirm { get; set; }
        public bool? Printed { get; set; }
        public int? PrintNumber { get; set; }
    }
}

