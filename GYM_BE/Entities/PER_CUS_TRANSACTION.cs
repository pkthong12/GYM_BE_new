using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("PER_CUS_TRANSACTION")]
    public class PER_CUS_TRANSACTION : BASE_ENTITY
    {
       public long? CUSTOMER_ID  { get; set; }

       public long? TRANS_FORM  { get; set; }

       public string? CODE  { get; set; }

       public string? TRANS_DATE  { get; set; }


    }
}

