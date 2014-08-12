using System;

namespace RestaurantRater.Models
{
    public class Restaurant
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int TotalRating { get; set; }

        public int NumberOfRatings { get; set; }

        public string WebsiteUrl { get; set; }
    }
}

