using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("PER_CUS_LIST_CARD")]
    public class PER_CUS_LIST_CARD : BASE_ENTITY
    {
       public long? CARD_ID  { get; set; }

       public long? CARD_PRICE  { get; set; }

       public long? CUSTOMER_ID  { get; set; }

       public float? USED​_TIME  { get; set; }

       public float? EXTENSION_PERIOD  { get; set; }

       public float? TOTAL_TIME  { get; set; }

       public DateTime? START_DATE  { get; set; }

       public DateTime? EXPIRE_DATE  { get; set; }


    }
}

