using GYM_BE.Entities;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES
{
    [Table("GOODS_EQUIPMENT")]

    public class GOODS_EQUIPMENT : BASE_ENTITY 
    {
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public long? EQUIPMENT_TYPE { get; set; }
        public string? MANUFACTURER { get; set; }
        public DateTime? PURCHASE_DATE { get; set; }
        public long? STATUS_ID { get; set; }
        public DateTime? WARRANTY_EXPIRY_DATE { get; set; }
        public decimal? COST { get; set; }
        public string? ADDRESS { get; set; }
        public long? MANAGER_ID { get; set; }
        public string? NOTE { get; set; }
    }
}
