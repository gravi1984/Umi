using System;
using System.Collections.Generic;
using Umi.API.Dtos;
using Umi.API.Models;
using System.Threading.Tasks;

namespace Umi.API.Services
{
    public interface ITouristRouteRepository
    {
        // from DB, get all routes
        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOpt, int? ratingValue);
        Task<TouristRoute> GetTouristRouteAsync(Guid id);

        Task<bool> TouristRouteExistsAsync(Guid id);

        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid id);
        
        Task<TouristRoutePicture> GetPictureAsync(int id);
        


        void AddTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoute(TouristRoute touristRoute);
        

        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture);

        Task<ShoppingCart> GetShoppingCartByUserId(string userId);
        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);

        Task<bool> SaveAsync();
    }
}