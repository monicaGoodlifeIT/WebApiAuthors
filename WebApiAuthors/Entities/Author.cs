using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Entities
{
    public class Author : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Name { get; set; }

        [Display(Name = "Edad")]
        [Range(18, 120, ErrorMessage = "La {0} del autor debe estar entre {1} y {2}.")]
        [NotMapped]
        public int Age { get; set; }

        //[Display(Name = "Tarjeta de Crédito")]
        //[CreditCard(ErrorMessage = "La numeración de la {0} no es válida")] // --> SOLO valida la numeración de la tarjeta 
        //[NotMapped]
        //public string? CreditCard { get; set; }

        //[Url(ErrorMessage = "La {0} ingresada, no es una URL fully-qualified, ni https, ni ftp válida.")]
        //[NotMapped] 
        //public string? Url { get; set; }

        // Propiedad de Navegación
        public List<Book>? Books { get; set; }

        // Interfaz de validación
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();

                if (firstLetter != firstLetter.ToUpper())
                {
                    // yield añade el valor del return en la lista IEnumerable del método
                    yield return new ValidationResult("La primera letra debe ser mayúscula - Modelo", 
                        new string[] {nameof(Name)});
                }
            }
        }
    }
}