namespace GYM_BE.Core.Dto
{
    public class FormatedResponse
    {
        public string MessageCode { get; set; } = string.Empty;
        public EnumErrorType ErrorType { get; set; }
        public EnumStatusCode StatusCode { get; set; } = EnumStatusCode.StatusCode200;

        public object? InnerBody { get; set; }
    }
}
