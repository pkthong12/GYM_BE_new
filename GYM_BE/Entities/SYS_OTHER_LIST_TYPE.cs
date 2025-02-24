using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("SYS_OTHER_LIST_TYPE")]
    public class SYS_OTHER_LIST_TYPE:BASE_ENTITY
    {
        public string? CODE { get; set; }
        public string? NAME {get;set;}
        public int? ORDERS {get;set;}
        public string? NOTE {get;set;}
        public bool? IS_SYSTEM {get;set;}
        public bool? IS_ACTIVE {get;set;}
    }
}
