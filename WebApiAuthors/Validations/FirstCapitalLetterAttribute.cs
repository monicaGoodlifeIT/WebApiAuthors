using System.ComponentModel.DataAnnotations;

namespace WebApiAuthors.Validations
{
    public class FirstCapitalLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Valida si el valor del objeto es nulo
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                // Si el valor es nulo o vacío, se deja pasar, se establece como success
                // para no duplicar la validación ya establecida con REQUIRED, centrándose
                // en validar si la primera letra del nombre es mayúscula.
                return ValidationResult.Success;
            }

            // Valida la primera letra en mayúscula
            var firstLetter = value.ToString()[0].ToString();

            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula - Atributo");
            }

            return ValidationResult.Success;
        }
    }
}
