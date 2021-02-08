// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Umi.API.Models;
//
// namespace Umi.API.Services
// {
//     public class MockTouristRouteRepository : ITouristRouteRepository
//     {
//         // Mock date, not in DB, in RAM
//         private List<TouristRoute> _routes;
//
//
//         public MockTouristRouteRepository()
//         {
//             if (_routes == null)
//             {
//                 Init();
//             }
//            
//         }
//
//         private void Init()
//         {
//             _routes = new List<TouristRoute>
//             {
//                 new TouristRoute
//                 {
//                     Id = Guid.NewGuid(),
//                     Title = "Iceland",
//                     Description = "North Pole fantastic",
//                     OriginalPrice = 9999.5M,
//                     DiscountPresent = 7999.9,
//                     CreateTime = DateTime.Parse("2020-02-10"),
//                     DepartureTime = new DateTime(2020, 07, 20),
//                     Features = "Summer in North Pole",
//                     Fees = "all inclusive",
//                     Notes = "need someone who is cold tolerant"
//                 },
//                 new TouristRoute
//                 {
//                     Id = Guid.NewGuid(),
//                     Title = "Brazil",
//                     Description = "Summer feast",
//                     OriginalPrice = 6999.5M,
//                     CreateTime = DateTime.Parse("2020-02-10"),
//                     DepartureTime = new DateTime(2020, 11, 20),
//                     Features = "Festival in South American",
//                     Fees = "all inclusive",
//                     Notes = "need someone who is hot tolerant"
//                 }
//             };
//         }
//         
//
//         public IEnumerable<TouristRoute> GetTouristRoutes()
//         {
//             // throw new NotImplementedException();
//
//             return _routes;
//         }
//
//         public TouristRoute GetTouristRoute(Guid touristRouteId)
//         {
//             // throw new NotImplementedException();
//
//             // TODO: LINQ is a MUST-HAVE
//             return _routes.FirstOrDefault(n => n.Id == touristRouteId);
//         }
//     }
// }