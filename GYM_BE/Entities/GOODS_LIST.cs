using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("GOODS_LIST")]
    public class GOODS_LIST : BASE_ENTITY
    {
        public string? CODE { get; set; }

        public string? NAME { get; set; }

        public long? PRODUCT_TYPE_ID { get; set; }

        public string? SUPPLIER { get; set; }

        public decimal? IMPORT_PRICE { get; set; }

        public decimal? PRICE { get; set; }

        public long? QUANTITY  { get; set; }

        public long? MEASURE_ID  { get; set; }

        public string? RECEIVING_DATE { get; set; }

        public string? EXPIRE_DATE { get; set; }

        public DateTime? RECEIVING_DATETIME { get; set; }

        public DateTime? EXPIRE_DATETIME { get; set; }

        public string? LOCATION { get; set; }

        public long? STATUS  { get; set; }

        public string? NOTE  { get; set; }

        public string? BATCH_NO  { get; set; }
        public string? WARRANTY_INFOR { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? SOURCE { get; set; }

        public long? MANAGER_ID { get; set; }

    }
}

