namespace GYM_BE.DTO
{
    public class GoodsListDTO : BaseDTO
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public long? ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }

        public string? Supplier { get; set; }

        public decimal? ImportPrice { get; set; }

        public decimal? Price { get; set; }

        public long? Quantity { get; set; }

        public long? MeasureId { get; set; }
        public string? MeasureName { get; set; }

        public DateTime? ReceivingDateTime { get; set; }
        public string? ReceivingDate { get; set; }

        public DateTime? ExpireDateTime { get; set; }
        public string? ExpireDate { get; set; }

        public string? Location { get; set; }

        public long? Status { get; set; }
        public string? StatusName { get; set; }

        public string? Note { get; set; }

        public string? BatchNo { get; set; }
        public string? WarrantyInfor { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }

        public long? ManagerId { get; set; }
        public string? ManagerName { get; set; }


    }
}

