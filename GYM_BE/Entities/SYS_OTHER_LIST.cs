using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("SYS_OTHER_LIST")]

    public class SYS_OTHER_LIST : BASE_ENTITY
    {
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public long? TYPE_ID { get; set; }
        public int? ORDERS { get; set; }
        public string? NOTE { get; set; }
        public bool? IS_ACTIVE { get; set; }
    }
}
