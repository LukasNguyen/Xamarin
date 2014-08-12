using System.Collections.Generic;

using RestaurantRater.Models;

namespace RestaurantRater.Data
{
    public interface IRestaurantDataService
    {
        IReadOnlyList<Restaurant> GetRestaurants();

        void RefreshCache();

        void SaveRestaurant(Restaurant restaurant);

        Restaurant GetRestaurant(int id);

        void DeleteRestaurant(int id);

        void RateRestaurant(int id, int newRating);
    }
}

