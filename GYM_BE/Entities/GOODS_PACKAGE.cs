using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES
{
    [Table("GOODS_PACKAGE")]
    public class GOODS_PACKAGE : BASE_ENTITY 
    {
        public string? CODE { get; set; }
        public decimal? MONEY { get; set; }
        public double? PERIOD { get; set; }
        public long? SHIFT_ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public bool? IS_PRIVATE { get; set; }
        public bool? IS_ACTIVE { get; set; }
    }
}
