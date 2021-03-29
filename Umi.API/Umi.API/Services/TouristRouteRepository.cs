using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Umi.API.Database;
using Umi.API.Models;

namespace Umi.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(
            string keyword,
            string ratingOpt,
            int? ratingValue
        )
        {
            // defer execution Linq -> SQL
            // Include: TouristRoute join Picture
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }

            if (ratingValue >= 0)
            {
                result = ratingOpt switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue)
                };
            }

            // if keyword is empty, return all list is fine.
            return await result.ToListAsync();

            // EF: include to join 2 table by FK
            // include vs join: Eager Load
            // Lazy Load
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid id)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<bool> TouristRouteExistsAsync(Guid id)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid id)
        {
            return await _context.TouristRoutePictures.Where(p => p.TouristRouteId == id).ToListAsync();
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int id)
        {
            return await _context.TouristRoutePictures.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }

            _context.TouristRoutes.Add(touristRoute);

            // _context.SaveChanges();
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }

            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }

            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture)
        {
            _context.TouristRoutePictures.Remove(touristRoutePicture);
        }


        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.User)
                .Include(s => s.ShoppingCartItems).ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }
        

        public async Task<LineItem> GetShoppingCartItemById(int lineItemId)
        {
            return await _context.LineItems.Where(li => li.Id == lineItemId).FirstOrDefaultAsync();
        }

        public async void DeleteShoppingCartItem (LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartItemsByIds(IEnumerable<int> lineItemIds)
        {
            return await _context.LineItems
                .Where(li => lineItemIds.Contains(li.Id))
                .ToListAsync();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }
    }
}