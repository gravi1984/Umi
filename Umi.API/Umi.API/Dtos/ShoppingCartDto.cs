using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umi.API.Models;

namespace Umi.API.Dtos
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public ICollection<LineItemDto> ShoppingCartItems { get; set; }
        // Microsoft: LineItem product -> Price -> Cart -> Order
    }
}