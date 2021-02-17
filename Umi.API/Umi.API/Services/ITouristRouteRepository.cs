using System;
using System.Collections.Generic;
using Umi.API.Dtos;
using Umi.API.Models;

namespace Umi.API.Services
{
    public interface ITouristRouteRepository
    {
        // from DB, get all routes
        IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string ratingOpt, int? ratingValue);
        TouristRoute GetTouristRoute(Guid id);

        bool TouristRouteExists(Guid id);

        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid id);
        
        TouristRoutePicture GetPicture(int id);

        void AddTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoute(TouristRoute touristRoute);

        bool Save();

        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture);
    }
}