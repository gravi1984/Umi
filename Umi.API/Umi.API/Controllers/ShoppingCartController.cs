using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umi.API.Dtos;
using Umi.API.Helper;
using Umi.API.Models;
using Umi.API.Services;

namespace Umi.API.Controllers
{
    
    [ApiController]
    [Route("api/shoppingCart")]
    public class ShoppingCartController : ControllerBase
    {

        // HttpContext: get User under current context
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
                
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetShoppingCart()
        {
            
            // 1. get current User
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            // 2. user userId get shoppingCart
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserId(userId);
            
            // 0. init shopingCart when User register -> Auth controller
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));

        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItems([FromBody] AddShoppingCartItemDto addShoppingCartItemDto)
        {
            // 1. get Cart
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserId(userId);
            
            // 2. add Item to Cart
            var touristRoute =
                await _touristRouteRepository.GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);

            if (touristRoute == null)
            {
                return NotFound("Tourist route not exist. ");
            }

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCardId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent
            };

            await _touristRouteRepository.AddShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            // 3. return 200

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            // 1. get lineItem
            var lineItem = await _touristRouteRepository.GetShoppingCartItemById(itemId);
            if (lineItem == null)
            {
                return NotFound("can't found the item in shopping cart");
            }

            // 2. delete lineItem
            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            // 3. return 200
            return NoContent();
        }
        

        // [HttpDelete("items/({itemIds})")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // public async Task<IActionResult> RemoveShoppingCartItems(
        //     [ModelBinder(BinderType = typeof(ArrayModelBinder))] 
        //     [FromRoute] IEnumerable<int> itemIds)
        // {
        //
        //     var lineItems = await _touristRouteRepository.GetShoppingCartItemsByIds(itemIds);
        //
        //     _touristRouteRepository.DeleteShoppingCartItems(lineItems);
        //     await _touristRouteRepository.SaveAsync();
        //
        //     return NoContent();
        //
        // }

    }
}