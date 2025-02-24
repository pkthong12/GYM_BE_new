using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES;
[Table("CARD_CHECK_IN")]
public class CARD_CHECK_IN : BASE_ENTITY
{
    public long? CARD_INFO_ID { get;  set; }
    public DateTime? TIME_START { get; set; }
    public DateTime? TIME_END { get; set; }
    public DateTime? DAY_CHECK_IN { get; set; }
}
