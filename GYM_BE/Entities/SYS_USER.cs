using GYM_BE.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES
{
    [Table("SYS_USER")]
    public class SYS_USER
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string ID { get; set; }
        public string? USERNAME { get; set; }
        public string? FULLNAME { get; set; }
        public string? PASSWORDHASH { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? GROUP_ID { get; set; }
        public bool? IS_ADMIN { get; set; }
        public bool? IS_ROOT { get; set; }
        public bool? IS_LOCK { get; set; }
        public string? AVATAR { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string? UPDATED_BY { get; set; }
        public string? DECENTRALIZATION { get; set; }
    }
}
