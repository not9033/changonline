using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangOnline.Models
{
    [Table("Province")]
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        public required string NameTh { get; set; }
        public required string NameEn { get; set; }
    }
}
