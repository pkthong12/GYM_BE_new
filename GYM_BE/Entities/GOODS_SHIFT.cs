using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES
{
    [Table("GOODS_SHIFT")]

    public class GOODS_SHIFT : BASE_ENTITY 
    {
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public int? TOTAL_DAYS { get; set; }
        public string? HOURS_START { get; set; }
        public string? HOURS_END { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string? NOTE { get; set; }
    }
}
