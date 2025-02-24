namespace GYM_BE.DTO
{
    public class CardCheckInDTO : BaseDTO
    {
        public long? CardInfoId { get; set; }
        public string? CardCode { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public DateTime? DayCheckIn { get; set; }
        public string? DayCheckInString { get; set; }
        public string? CustomerName { get; set; }
        public string? CodeCus { get; set; }//ms kh
        public string? GenderName { get; set; }
        public string? CardTypeName { get; set; }//loai the
        public string? TimeStartString { get; set; }
        public string? TimeEndString { get; set; }
        public string? TimeStartShiftString { get; set; }
        public string? TimeEndShiftString { get; set; }
        public string? ShiftName { get; set; }

    }
}
