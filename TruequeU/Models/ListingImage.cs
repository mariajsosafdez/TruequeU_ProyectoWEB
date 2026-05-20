using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models
{
    public class ListingImage
    {
        [Key]
        public Guid ImageID { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "La URL de la imagen es obligatoria")]
        public string Url { get; set; }
        [Required]
        public Guid ListingID { get; set; }//llave foránea apuntando a  el Id de la publicación

        [ForeignKey("ListingID")]
        public Listings? Listing { get; set; }
    }
}