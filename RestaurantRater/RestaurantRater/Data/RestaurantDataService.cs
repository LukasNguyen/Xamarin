using System.IO;

using Android.OS;

namespace RestaurantRater.Data
{
    public class RestaurantDataService
    {
        public static readonly IRestaurantDataService Service = new RestaurantJsonService(Path.Combine(Environment.ExternalStorageDirectory.Path, "RestaurantRater"));
    }
}

