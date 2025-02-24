namespace GYM_BE.DTO
{
    public class SysOtherListTypeDTO:BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Orders { get; set; }
        public string? Note { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
