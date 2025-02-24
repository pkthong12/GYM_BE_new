using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("GOODS_LOCKER")]
    public class GOODS_LOCKER : BASE_ENTITY
    {
       public long? AREA  { get; set; }

       public long? PRICE  { get; set; }
       public long? STATUS_ID { get; set; }

       public string? MAINTENANCE_FROM_DATE  { get; set; }

       public string? MAINTENANCE_TO_DATE  { get; set; }

       public string? CODE  { get; set; }
       public string? NOTE  { get; set; }


    }
}

