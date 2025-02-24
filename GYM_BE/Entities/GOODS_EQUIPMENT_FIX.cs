using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("GOODS_EQUIPMENT_FIX")]
    public class GOODS_EQUIPMENT_FIX : BASE_ENTITY
    {
        public string? CODE { get; set; }

        public int? TYPE_ID  { get; set; }

       public long? RESULT_ID  { get; set; }

       public long? EMPLOYEE_ID  { get; set; }

       public long? EQUIPMENT_ID  { get; set; }

       public string? NOTE  { get; set; }

       public decimal? MONEY  { get; set; }

       public string? EXPECTED_USE_TIME  { get; set; }

       public string? START_DATE  { get; set; }

       public string? END_DATE  { get; set; }

       public long? STATUS_ID { get; set; }
    }
}

