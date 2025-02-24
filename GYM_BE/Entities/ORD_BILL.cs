using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("ORD_BILL")]
    public class ORD_BILL : BASE_ENTITY
    {
        public long? TYPE_TRANSFER { get; set; }

        public long? CUSTOMER_ID { get; set; }

        public long? PER_SELL_ID { get; set; }

        public long? MONEY_HAVE_PAY { get; set; }

        public long? TOTAL_MONEY { get; set; }

        public long? PAY_METHOD { get; set; }

        public decimal? DISC_PERCENT { get; set; }

        public string? CODE { get; set; }
        public long? PK_REF { get; set; }
        public decimal? PERCENT_VAT { get; set; }
        public long? VOUCHER_ID { get; set; }
        public bool? IS_CONFIRM { get; set; }
        public bool? PRINTED { get; set; }
        public int? PRINT_NUMBER { get; set; }

    }
}

