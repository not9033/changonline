using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangOnline.Models
{
    [Table("subdistrict")]
    public class Subdistrict
    {
        [Key]
        public int subdistrict_id { get; set; }
        public int district_id { get; set; }
        public required string name_th { get; set; }
        public required string name_en { get; set; }
        public string zipcode { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}
