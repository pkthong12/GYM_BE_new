namespace GYM_BE.DTO
{
    public class SysUserCreateUpdateRequest
    {
        public required string Id { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public string? Password { get; set; }
        public string? RePassword { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public long? GroupId { get; set; }
        public string? GroupName { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsRoot { get; set; }
        public bool? IsLock { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Decentralization { get; set; }
        public List<long>? DecentralizationList { get; set; }
    }
}
