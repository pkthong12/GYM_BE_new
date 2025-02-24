using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("TR_CENTER")]
    public class TR_CENTER : BASE_ENTITY
    {
        public string CODE_CENTER { get; set; }
        public string NAME_CENTER { get; set; }
        public string TRAINING_FIELD { get; set; }
        public string? ADDRESS { get; set; }
        public string? PHONE { get; set; }
        public string? REPRESENTATIVE { get; set; }
        public string? CONTACT_PERSON { get; set; }
        public string? PHONE_CONTACT_PERSON { get; set; }
        public string? WEBSITE { get; set; }
        public string? NOTE { get; set; }
        public string? ATTACHED_FILE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public bool? IS_ACTIVE { get; set; }
    }
}
