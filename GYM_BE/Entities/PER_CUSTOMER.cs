using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES;
[Table("PER_CUSTOMER")]

public class PER_CUSTOMER : BASE_ENTITY
{
    public string? AVATAR { get; set; }
    public string? CODE { get; set; }
    public long? CUSTOMER_CLASS_ID { get; set; }
    public string? FIRST_NAME { get; set; }
    public string? LAST_NAME { get; set; }
    public string? FULL_NAME { get; set; }
    public string? ID_NO { get; set; }
    public string? BIRTH_DATE { get; set; }
    public long? GENDER_ID { get; set; }
    public string? ADDRESS { get; set; }
    public string? PHONE_NUMBER { get; set; }
    public string? EMAIL { get; set; }
    public long? NATIVE_ID { get; set; }
    public long? RELIGION_ID { get; set; }
    public long? BANK_ID { get; set; }
    public long? BANK_BRANCH { get; set; }
    public string? BANK_NO { get; set; }
    public bool? IS_GUEST_PASS { get; set; }
    public string? JOIN_DATE { get; set; }
    public string? HEIGHT { get; set; }
    public string? WEIGHT { get; set; }
    public long? CARD_ID { get; set; }
    public string? EXPIRE_DATE { get; set; }
    public long? GYM_PACKAGE_ID { get; set; }
    public long? PER_PT_ID { get; set; }
    public long? PER_SALE_ID { get; set; }
    public long? STATUS_ID { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
    

}
