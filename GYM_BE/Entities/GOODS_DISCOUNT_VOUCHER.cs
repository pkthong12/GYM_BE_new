using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("GOODS_DISCOUNT_VOUCHER")]
    public class GOODS_DISCOUNT_VOUCHER : BASE_ENTITY
    {
       public int? USAGE_LIMIT  { get; set; }

       public long? DISTRIBUTION_CHANNEL  { get; set; }

       public long? DISCOUNT_TYPE_ID  { get; set; }

       public long? ISSUE_QUANTITY  { get; set; }

       public long? USED_QUANTITY  { get; set; }

       public long? TARGET_CUSTOMERS  { get; set; }

       public long? STATUS  { get; set; }

       public string? DESCRIPTION  { get; set; }

       public string? DICOUNT_VALUE  { get; set; }

       public string? START_DATE  { get; set; }

       public string? END_DATE  { get; set; }

       public string? CODE  { get; set; }

       public string? NAME  { get; set; }

       public string? NOTE  { get; set; }

       public string? CONDITION  { get; set; }

       public DateTime? START_DATETIME  { get; set; }

       public DateTime? END_DATETIME  { get; set; }


    }
}

