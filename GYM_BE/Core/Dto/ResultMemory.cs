namespace GYM_BE.Core.Dto
{
    public class ResultMemory
    {
        public byte[]? memoryStream;
        public EnumStatusCode StatusCode { get; set; } = EnumStatusCode.StatusCode200;
    }
}
