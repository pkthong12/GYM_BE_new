using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("CARD_ISSUANCE")]
    public class CARD_ISSUANCE : BASE_ENTITY
    {
       public long? CUSTOMER_ID  { get; set; }

       public long? CARD_ID  { get; set; }

       public long? HOUR_CARD  { get; set; }

       public long? HOUR_CARD_BONUS  { get; set; }

       public long? TOTAL_HOUR_CARD  { get; set; }

       public long? PER_SELL_ID  { get; set; }

       public bool? WARDROBE  { get; set; }

       public long? LOCKER_ID  { get; set; }

       public bool? IS_HAVE_PT  { get; set; }

       public long? PER_PT_ID  { get; set; }

       public bool? IS_REAL_PRICE  { get; set; }
       public bool? IS_ACTIVE { get; set; }

       public long? CARD_PRICE  { get; set; }

       public long? PERCENT_DISCOUNT  { get; set; }

       public long? AFTER_DISCOUNT  { get; set; }

       public long? PERCENT_VAT  { get; set; }

       public long? TOTAL_PRICE  { get; set; }

       public long? MONEY_HAVE_PAY  { get; set; }

       public long? PAID_MONEY  { get; set; }

       public string? NOTE  { get; set; }

       public string? DOCUMENT_NUMBER  { get; set; }

       public string? DOCUMENT_DATE  { get; set; }


    }
}

