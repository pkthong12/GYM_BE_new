using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("CARD_HISTORY")]
    public class CARD_HISTORY : BASE_ENTITY
    {
       public long? CARD_TYPE_ID  { get; set; }

       public bool? WARDROBE  { get; set; }

       public long? SHIFT_ID  { get; set; }

       public long? PRICE  { get; set; }

       public long? CUSTOMER_ID  { get; set; }

       public long? LOCKER_ID  { get; set; }

       public bool? IS_ACTIVE  { get; set; }

       public string? CODE  { get; set; }

       public string? EFFECTED_DATE  { get; set; }

       public string? EXPIRED_DATE  { get; set; }

       public string? NOTE  { get; set; }
        
       public int? ACTION { get; set; }

       public bool? IS_HAVE_PT { get; set; }

    }
}

