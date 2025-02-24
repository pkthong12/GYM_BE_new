using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES;
[Table("CARD_INFO")]
public class CARD_INFO : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? EFFECTED_DATE { get; set; }
    public string? EXPIRED_DATE { get; set; } 
    public DateTime? EFFECTED_DATE_TIME { get; set; }
    public DateTime? EXPIRED_DATE_TIME { get; set; }
    public long? CARD_TYPE_ID { get; set; }
    public long? CUSTOMER_ID { get; set; }
    public long? LOCKER_ID { get; set; }
    public long? SHIFT_ID { get; set; }
    public long? PRICE { get; set; }
    public string? NOTE { get; set; }
    public bool? WARDROBE { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public bool? IS_HAVE_PT { get; set; }
}
