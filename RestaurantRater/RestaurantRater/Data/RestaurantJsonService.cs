using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using RestaurantRater.Models;

namespace RestaurantRater.Data
{
    public class RestaurantJsonService : IRestaurantDataService
    {
        private string storagePath;
        private List<Restaurant> restaurants = new List<Restaurant>();

        public RestaurantJsonService(string storagePath)
        {
            this.storagePath = storagePath;

            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);

            RefreshCache();
        }

        #region IRestaurantDataService implementation

        public IReadOnlyList<Restaurant> GetRestaurants()
        {
            return restaurants;
        }

        public void RefreshCache()
        {
            restaurants.Clear();

            var filenames = Directory.GetFiles(storagePath, "*" + Constants.RESTAURANT_FILE_EXTENSION);
            foreach (var filename in filenames)
            {
                var restaurantString = File.ReadAllText(filename);
                var restaurant = JsonConvert.DeserializeObject<Restaurant>(restaurantString);
                restaurants.Add(restaurant);
            }
        }

        public void SaveRestaurant(Restaurant restaurant)
        {
            var newRestaurant = false;

            if (!restaurant.Id.HasValue)
            {
                restaurant.Id = getNextId();
                newRestaurant = true;
            }

            var restaurantString = JsonConvert.SerializeObject(restaurant);
            File.WriteAllText(getFilename(restaurant.Id.Value), restaurantString);

            if (newRestaurant)
                restaurants.Add(restaurant);
        }

        public Restaurant GetRestaurant(int id)
        {
            return restaurants.Find(x => x.Id == id);
        }

        public void DeleteRestaurant(int id)
        {
            if (File.Exists(getFilename(id)))
                File.Delete(getFilename(id));

            var restaurant = restaurants.Find(x => x.Id == id);
            restaurants.Remove(restaurant);
        }

        public void RateRestaurant(int id, int newRating)
        {
            if (restaurants.SingleOrDefault(x => x.Id == id) == null ||
                !isValidRating(newRating))
                return;

            var restaurant = restaurants.Find(x => x.Id == id);
            restaurant.NumberOfRatings++;
            restaurant.TotalRating += newRating;

            SaveRestaurant(restaurant);
        }

        #endregion

        private int getNextId()
        {
            if (restaurants.Count == 0)
                return 1;

            return restaurants.Max(x => x.Id.Value) + 1;
        }

        private string getFilename(int id)
        {
            return Path.Combine(storagePath, id.ToString() + Constants.RESTAURANT_FILE_EXTENSION);
        }

        private bool isValidRating(int rating)
        {
            if (rating < 0 || rating > 5)
                return false;

            return true;
        }
    }
}