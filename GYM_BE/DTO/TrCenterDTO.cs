namespace GYM_BE.DTO
{
    public class TrCenterDTO : BaseDTO
    {
        public string CodeCenter { get; set; }
        public string NameCenter { get; set; }
        public string TrainingField { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Representative { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneContactPerson { get; set; }
        public string? Website { get; set; }
        public string? Note { get; set; }
        public string? AttachedFile { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }

    }
}
