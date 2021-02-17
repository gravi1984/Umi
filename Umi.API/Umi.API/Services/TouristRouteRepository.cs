using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<TouristRoute> GetTouristRoutes(
            string keyword,
        string ratingOpt, 
        int? ratingValue
        )
        {
            // defer execution Linq -> SQL
            // Include: TouristRoute join Picture
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t=>t.TouristRoutePictures);
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
            return result.ToList();

            // EF: include to join 2 table by FK
            // include vs join: Eager Load
            // Lazy Load

        }

        public TouristRoute GetTouristRoute(Guid id)
        {
            return _context.TouristRoutes.Include(t=>t.TouristRoutePictures).FirstOrDefault(n => n.Id == id);
        }

        public bool TouristRouteExists(Guid id)
        {
            return _context.TouristRoutes.Any(t => t.Id == id);
        }

        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid id)
        {

            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == id).ToList();
        }

        public TouristRoutePicture GetPicture(int id)
        {
            return _context.TouristRoutePictures.FirstOrDefault(p => p.Id == id);
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

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
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
    }
}