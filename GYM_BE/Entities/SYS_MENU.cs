using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("SYS_MENU")]
    public class SYS_MENU : BASE_ENTITY
    {
        public long? PARENT { get; set; }
        public string? URL { get; set; }
        public string? NAME { get; set; }
        public string? ICON { get; set; }
        public bool? IS_HIDDEN { get; set; }


    }
}

