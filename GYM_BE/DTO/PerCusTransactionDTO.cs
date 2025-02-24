namespace GYM_BE.DTO
{
    public class PerCusTransactionDTO : BaseDTO
    {
       public long? CustomerId  { get; set; }
       public string? CustomerCode  { get; set; }
       public long? TransForm  { get; set; }
       public string? Code  { get; set; }
       public string? TransDate  { get; set; }
       public string? TransDateString  { get; set; }
        public string? FullName { get; set; }
        public string? BirthDateString { get; set; }
        public string? GenderName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IdNo { get; set; }
        public long? CustomerClassId { get; set; }
        public string? CustomerClassName { get; set; }
    }
}

