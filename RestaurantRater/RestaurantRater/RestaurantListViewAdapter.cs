using System;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

using RestaurantRater.Data;
using RestaurantRater.Models;

namespace RestaurantRater
{
    public class RestaurantListViewAdapter : BaseAdapter<Restaurant>
    {
        private readonly Activity context;

        public RestaurantListViewAdapter(Activity context)
        {
            this.context = context;

            // TODO: remove this after we get a menu to add actual items!
            var current = RestaurantDataService.Service.GetRestaurants();
            foreach (var curRest in current)
            {
                RestaurantDataService.Service.DeleteRestaurant(curRest.Id.Value);
            }
            var restaurant = new Restaurant
                {
                    Name = "TestRestaurant",
                    Address = "123 That St",
                    Latitude = 1.0,
                    Longitude = -1.0,
                    TotalRating = 10,
                    NumberOfRatings = 4,
                    WebsiteUrl = "www.google.com"
                };
            RestaurantDataService.Service.SaveRestaurant(restaurant);
        }

        #region implemented abstract members of BaseAdapter

        public override Restaurant this[int index]
        {
            get { return RestaurantDataService.Service.GetRestaurants()[index]; }
        }

        public override int Count
        {
            get { return RestaurantDataService.Service.GetRestaurants().Count(); }
        }

        public override long GetItemId(int position)
        {
            return RestaurantDataService.Service.GetRestaurants()[position].Id.Value;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.RestaurantListItem, null);

            var restaurant = RestaurantDataService.Service.GetRestaurants()[position];
            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = restaurant.Name;
            if (string.IsNullOrWhiteSpace(restaurant.Address))
                view.FindViewById<TextView>(Resource.Id.addressTextView).Visibility = ViewStates.Gone;
            else
                view.FindViewById<TextView>(Resource.Id.addressTextView).Text = restaurant.Address;

            view.FindViewById<TextView>(Resource.Id.ratingTextView).Text = String.Format("{0:0.0}", ((double)restaurant.TotalRating / (double)restaurant.NumberOfRatings));

            return view;
        }

        #endregion
    }
}

