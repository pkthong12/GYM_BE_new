namespace GYM_BE.ENTITIES
{
    public class CARD_EXPIRE_REPORT
    {
        public long STT { get; set; }
        public string? CARD_CODE { get; set; }
        public string? CARD_TYPE { get; set; }
        public string? EMPLOYEE_CODE { get; set; }
        public string? FULL_NAME { get; set; }
        public string? GENDER { get; set; }
        public string? PHONE_NUMBER { get; set; }
        public string? EFFECTED_DATE { get; set; }
        public string? EXPIRED_DATE { get; set; }
        public long DAY_LEFT { get; set; }
    }
}
