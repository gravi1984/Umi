using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umi.API.Models
{
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // int auto-increment attr 
        public int Id { get; set; }
        
        [ForeignKey("TouristRouteId")]
        public Guid TouristRouteId { get; set; } // FK to Product
        public TouristRoute TouristRoute { get; set; } 
        
        [ForeignKey("ShoppingCardId")]
        public Guid? ShoppingCardId { get; set; } 
        
        // public Guid? OrderId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        // declare Nullable field type? 
        [Range(0.0,1.0)]
        public double? DiscountPresent { get; set; }
        
        
        
        
    }
}