namespace GYM_BE.DTO
{
    public class GoodsDiscountVoucherDTO : BaseDTO
    {
       public int? UsageLimit  { get; set; }

       public long? DistributionChannel  { get; set; }
       public string? DistributionChannelName  { get; set; }

        public long? DiscountTypeId  { get; set; }
       public string? DiscountTypeName  { get; set; }

        public long? IssueQuantity  { get; set; }

       public long? UsedQuantity  { get; set; }

       public long? TargetCustomers  { get; set; }
       public string? TargetCustomersName  { get; set; }

        public long? Status  { get; set; }
        public string? StatusName  { get; set; }

        public string? Description  { get; set; }

       public string? DicountValue  { get; set; }

       public string? StartDate  { get; set; }

       public string? EndDate  { get; set; }

       public string? Code  { get; set; }

       public string? Name  { get; set; }

       public string? Note  { get; set; }

       public string? Condition  { get; set; }

       public DateTime? StartDateTime  { get; set; }

       public DateTime? EndDateTime  { get; set; }


    }
}

