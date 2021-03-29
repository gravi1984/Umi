using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Umi.API.Models
{

    public enum OrderStateEnum
    {
        Pending,
        Processing,
        Completed,
        Declined,
        Cancelled,
        Refund
    }
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        
        public ICollection<LineItem> OrderItems { get; set; }
        
        public OrderStateEnum State { get; set; }
        
        public DateTime CreateDateUTC { get; set; }
        
        // 3rd payment callback response, give to FE
        public string TransactionMetadata { get; set; }
       
    }
}