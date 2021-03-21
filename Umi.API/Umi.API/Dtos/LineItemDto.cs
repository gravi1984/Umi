using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Umi.API.Models;

namespace Umi.API.Dtos
{
    public class LineItemDto
    {
        public int Id { get; set; }
        
        public Guid TouristRouteId { get; set; }
        
        public TouristRouteDto TouristRouteDto { get; set; } 
        
        public Guid? ShoppingCardId { get; set; } 
        // public Guid? OrderId { get; set; }
        
        public decimal OriginalPrice { get; set; }
        
        public double? DiscountPresent { get; set; }

    }
}