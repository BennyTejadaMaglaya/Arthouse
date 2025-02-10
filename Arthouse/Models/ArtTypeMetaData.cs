using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtHouse.Models
{
    public class ArtTypeMetaData
    {
        [Display(Name = "Art Type")]
        [Required(ErrorMessage = "You cannot leave the art type name blank.")]
        [StringLength(25, ErrorMessage = "Art type cannot be more than 25 characters long.")]
        public string Type { get; set; } = "";
    }
}
