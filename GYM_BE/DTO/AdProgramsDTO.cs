namespace API.DTO
{
    public class AdProgramsDTO
    {
        public int ProgramId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ProgramType { get; set; }
        public string? Description { get; set; }
        public string? StoreExecuteIn { get; set; }
        public string? StoreExecuteOut { get; set; }
        public string? TemplateName { get; set; }
        public string? TemplateTypeIn { get; set; }
        public string? TemplateTypeOut { get; set; }
        public string? TemplateUrl { get; set; }
    }
}
