using System.ComponentModel.DataAnnotations;
using Umi.API.Dtos;


namespace Umi.API.ValidationAttributes

{
    public class TouristRouteTitleMustBeDifferentFromDescription : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForCreationDto) validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult(
                    "description cannot be same with title",
                    new[] {"TouristRouteForCreationDto"}
                );
            }
            
            return ValidationResult.Success;
        }
    }
}