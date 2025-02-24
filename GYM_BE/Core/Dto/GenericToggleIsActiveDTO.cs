namespace GYM_BE.Core.Dto
{
    public class GenericToggleIsActiveDTO
    {
        public List<long> Ids { get; set; }

        public bool ValueToBind { get; set; }
    }
}
