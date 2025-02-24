namespace GYM_BE.DTO
{
    public class PerEmployeeDTO:BaseDTO
    {
        public string? Code { get; set; }
        public string? FullName { get; set; }
        public long? GenderId { get; set; }
        public string? GenderName { get; set; }
        public string? BirthDate { get; set; }
        public string? IdNo { get; set; }
        public long? StaffGroupId { get; set; }
        public string? StaffGroupName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public long? StatusId { get; set; }
        public string? StatusName { get; set; }
    }
}
