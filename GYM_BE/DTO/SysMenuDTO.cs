namespace GYM_BE.DTO
{
    public class SysMenuDTO : BaseDTO
    {
        public long? Parent { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public List<SysMenuDTO>? ChildMenu { get; set; }

    }
}

