using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    public class BASE_ENTITY
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string? UPDATED_BY { get; set; }

    }
}
