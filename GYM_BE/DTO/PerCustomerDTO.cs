namespace GYM_BE.DTO
{
    public class PerCustomerDTO : BaseDTO
    {
        public string? Avatar { get; set; }
        public string? Code { get; set; }
        public long? CustomerClassId { get; set; }
        public string? CustomerClassName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? BirthDate { get; set; }
        public string? BirthDateString { get; set; }
        public long? GenderId { get; set; }
        public string? GenderName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IdNo { get; set; }
        public string? Email { get; set; }
        public long? NativeId { get; set; }
        public string? NativeName { get; set; }
        public long? ReligionId { get; set; }
        public string? ReligionName { get; set; }
        public long? BankId { get; set; }
        public string? BankName { get; set; }
        public long? BankBranch { get; set; }
        public string? BankBranchName { get; set; }
        public string? BankNo { get; set; }
        public bool? IsGuestPass { get; set; }
        public string? JoinDate { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public long? CardId { get; set; }
        public string? ExpireDate { get; set; }
        public long? GymPackageId { get; set; }
        public long? PerPtId { get; set; }
        public string? PerPtName { get; set; }
        public long? PerSaleId { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public long? StatusId { get; set; }
    }
}
