namespace GYM_BE.Core.Dto
{
    public class PaginationDTO<TDTO> where TDTO : class
    {
        public TDTO? Filter { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; }
        public int Page { get; set; } = 1;
    }
}
