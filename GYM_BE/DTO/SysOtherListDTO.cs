namespace GYM_BE.DTO
{
    public class SysOtherListDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? TypeId { get; set; }
        public string? TypeName { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
