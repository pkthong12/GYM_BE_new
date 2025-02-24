namespace GYM_BE.DTO
{
    public class GoodsEquipmentDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? EquipmentType { get; set; }
        public string? EquipmentTypeName { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public long? StatusId { get; set; }
        public string? Status { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public decimal? Cost { get; set; }
        public string? Address { get; set; }
        public long? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? Note { get; set; }
    }
}
