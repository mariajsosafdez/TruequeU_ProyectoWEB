using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models.DTO
{
    public class ChangeStatusDTO
    {
        [Required(ErrorMessage = "El Id de publicación es obligatorio")]
        public Guid ListingId { get; set; }

        [Required(ErrorMessage = "El nuevo estado es obligatorio")]
        [EnumDataType(typeof(Status), ErrorMessage = "El estado proporcionado no es válido para TruequeU")]
        public Status NuevoEstado { get; set; }
    }
}
