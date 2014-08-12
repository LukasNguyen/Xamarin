using System;
using System.IO;

using NUnit.Framework;

using RestaurantRater.Data;
using RestaurantRater.Models;

namespace RestaurantRaterTest
{
    [TestFixture]
    public class TestsSample
    {
        private string storagePath;
        private IRestaurantDataService restaurantDataService;

        [SetUp]
        public void Setup()
        {
            storagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            restaurantDataService = new RestaurantJsonService(storagePath);

            foreach (var filename in Directory.EnumerateFiles(storagePath, "*" + Constants.RESTAURANT_FILE_EXTENSION))
                File.Delete(filename);
        }

		
        [TearDown]
        public void Tear()
        {
            foreach (var filename in Directory.EnumerateFiles(storagePath, "*" + Constants.RESTAURANT_FILE_EXTENSION))
                File.Delete(filename);
        }

        [Test]
        public void CreateRestaurant_GoodRequest_Created()
        {
            var restaurant = createTestRestaurant();

            restaurantDataService.SaveRestaurant(restaurant);

            Assert.IsTrue(restaurant.Id.HasValue);
            Assert.IsTrue(restaurant.Id.Value > 0);

            restaurantDataService.RefreshCache();

            var result = restaurantDataService.GetRestaurant(restaurant.Id.Value);

            Assert.AreEqual("TestRestaurant", result.Name);
            Assert.AreEqual("123 That St", result.Address);
            Assert.AreEqual(1.0, result.Latitude);
            Assert.AreEqual(-1.0, result.Longitude);
            Assert.AreEqual(10, result.TotalRating);
            Assert.AreEqual(10, result.NumberOfRatings);
            Assert.AreEqual("www.google.com", result.WebsiteUrl);
        }

        [Test]
        public void DeleteRestaurant_GoodRequest_Deleted()
        {
            var restaurant = createTestRestaurant();

            restaurantDataService.SaveRestaurant(restaurant);

            Assert.IsTrue(restaurant.Id.HasValue);
            Assert.IsTrue(restaurant.Id.Value > 0);

            restaurantDataService.RefreshCache();

            var restaurantToDelete = restaurantDataService.GetRestaurant(restaurant.Id.Value);

            Assert.IsNotNull(restaurantToDelete);

            var testId = restaurantToDelete.Id.Value;
            restaurantDataService.DeleteRestaurant(restaurantToDelete.Id.Value);
            restaurantDataService.RefreshCache();

            var deletedRestaurant = restaurantDataService.GetRestaurant(testId);
            Assert.IsNull(deletedRestaurant);
        }

        [Test]
        public void UpdateRestaurant_GoodRequest_Updated()
        {
            var restaurant = createTestRestaurant();

            restaurantDataService.SaveRestaurant(restaurant);

            var testId = restaurant.Id.Value;

            restaurantDataService.RefreshCache();

            var restaurantToUpdate = restaurantDataService.GetRestaurant(testId);
            restaurantToUpdate.Address = "newAddress";
            restaurantDataService.SaveRestaurant(restaurantToUpdate);

            restaurantDataService.RefreshCache();

            var updatedRestaurant = restaurantDataService.GetRestaurant(testId);
            Assert.IsNotNull(updatedRestaurant);
            Assert.AreEqual("newAddress", updatedRestaurant.Address);
        }

        [Test]
        public void RateRestaurant_GoodRequest_Rated()
        {
            var restaurant = createTestRestaurant();

            restaurantDataService.SaveRestaurant(restaurant);

            var testId = restaurant.Id.Value;
            var restaurantToRate = restaurantDataService.GetRestaurant(testId);

            restaurantDataService.RateRestaurant(testId, 4);
            restaurantDataService.RefreshCache();

            var ratedRestaurant = restaurantDataService.GetRestaurant(testId);
            Assert.IsNotNull(ratedRestaurant);
            Assert.AreEqual(11, ratedRestaurant.NumberOfRatings);
            Assert.AreEqual(14, ratedRestaurant.TotalRating);
        }

        [Test]
        public void RateRestaurant_InvalidNewRating_NotRated()
        {
            var restaurant = createTestRestaurant();

            restaurantDataService.SaveRestaurant(restaurant);

            var testId = restaurant.Id.Value;
            var restaurantToRate = restaurantDataService.GetRestaurant(testId);

            restaurantDataService.RateRestaurant(testId, 400);
            restaurantDataService.RefreshCache();

            var ratedRestaurant = restaurantDataService.GetRestaurant(testId);
            Assert.IsNotNull(ratedRestaurant);
            Assert.AreEqual(10, ratedRestaurant.NumberOfRatings);
            Assert.AreEqual(10, ratedRestaurant.TotalRating);
        }

        private Restaurant createTestRestaurant()
        {
            var restaurant = new Restaurant
            {
                Name = "TestRestaurant",
                Address = "123 That St",
                Latitude = 1.0,
                Longitude = -1.0,
                TotalRating = 10,
                NumberOfRatings = 10,
                WebsiteUrl = "www.google.com"
            };

            return restaurant;
        }
    }
}

