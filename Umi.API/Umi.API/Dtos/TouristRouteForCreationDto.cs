using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Umi.API.Dtos
{
    // Dto: per FE page/requirement, granular
    public class TouristRouteForCreationDto : IValidatableObject
    {
        // auto created, not need for creation: public Guid Id { get; set; }
        
        // data validation: built-in vs customized
        
        
        [Required(ErrorMessage = "title cannot be empty")]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
  
        // Data transfer: hide/transfer data to FE
        // public decimal OriginalPrice { get; set; }
        // public double? DiscountPresent { get; set; }
        public decimal Price { get; set; }
        
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }

        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        
        // enrich Model
        public double? Rating { get; set; }
        public string TravelDays { get; set; }
        public string TripType { get; set; }
        public string DepartureCity { get; set; }

        // 1-N: map list lectured in 5.1.0
        // Auto map from linked table; return DTO object!!
        // most be copied name from touristRouteModel!!
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; } =
            new List<TouristRoutePictureForCreationDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
            {
                yield return  new ValidationResult(
                    "description cannot be same with title",
                    new[] { "TouristRouteForCreationDto"}
                );
            }
            
        }
    }
}