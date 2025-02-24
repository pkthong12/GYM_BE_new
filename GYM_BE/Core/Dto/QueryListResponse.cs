namespace GYM_BE.Core.Dto
{
    public class QueryListResponse<T> where T : class
    {
        public string? MessageCode { get; set; }

        public IQueryable<T>? List { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public int? Count { get; set; }

        public int? Page { get; set; }

        public int? PageCount { get; set; }
    }
}
