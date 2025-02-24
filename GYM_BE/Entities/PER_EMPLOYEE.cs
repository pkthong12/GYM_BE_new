using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("PER_EMPLOYEE")]
    public class PER_EMPLOYEE:BASE_ENTITY
    {
        public string? CODE{get;set;}
        public string? FULL_NAME{get;set;}
        public long? GENDER_ID{get;set;}
        public string? BIRTH_DATE{get;set;}
        public string? ID_NO{get;set;}
        public long? STAFF_GROUP_ID{get;set;}
        public string? PHONE_NUMBER{get;set;}
        public string? EMAIL{get;set;}
        public string? ADDRESS{get;set;}
        public string? NOTE{get;set;}
        public long? STATUS_ID { get; set; }
    }
}
