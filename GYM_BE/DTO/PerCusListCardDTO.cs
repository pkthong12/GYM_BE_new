namespace GYM_BE.DTO
{
    public class PerCusListCardDTO : BaseDTO
    {
       public long? CardId  { get; set; }

       public long? CardPrice  { get; set; }

       public long? CustomerId  { get; set; }

       public float? Used​Time  { get; set; }

       public float? ExtensionPeriod  { get; set; }

       public float? TotalTime  { get; set; }

       public DateTime? StartDate  { get; set; }

       public DateTime? ExpireDate  { get; set; }


    }
}

