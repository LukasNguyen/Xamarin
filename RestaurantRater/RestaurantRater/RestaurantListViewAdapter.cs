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

            view.FindViewById<TextView>(Resource.Id.ratingTextView).Text = String.Format("{0:0,0.0}", (restaurant.TotalRating / restaurant.NumberOfRatings));

            return view;
        }

        #endregion
    }
}

