using System;
using System.Collections.Generic;

namespace Umi.API.Dtos
{
    // Dto: per FE page/requirement, granular
    public class TouristRouteForCreationDto
    {
        // auto created, not need for creation: public Guid Id { get; set; }
        
        public string Title { get; set; }
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
        // public ICollection<TouristRoutePictureDto> TouristRoutePictures { get; set; }
    }
}