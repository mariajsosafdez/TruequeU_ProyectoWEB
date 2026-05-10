using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models.DTO
{
    public class ListingCreateDTO
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [MinLength(8, ErrorMessage = "El título debe tener al menos 8 caracteres")]
        [MaxLength(40, ErrorMessage = "Título demasiado largo (máximo 40)")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MinLength(20, ErrorMessage = "La descripción debe tener al menos 20 caracteres")]
        [MaxLength(500, ErrorMessage = "La descripción no debe tener más de 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Indique si el objeto es nuevo o usado")]
        public Condition Condicion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public Category Categoria { get; set; }
        public Location Ubicacion { get; set; }
    }
}
