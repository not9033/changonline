using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangOnline.Models
{
    [Table("District")]
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public required string NameTh { get; set; }
        public required string NameEn { get; set; }
    }
}
