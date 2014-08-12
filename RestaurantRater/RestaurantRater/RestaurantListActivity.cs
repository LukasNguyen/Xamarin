using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RestaurantRater
{
    [Activity(Label = "Restaurants", MainLauncher = true, ConfigurationChanges = (Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize))]
    public class RestaurantListActivity : Activity
    {
        private ListView restaurantListView;
        private RestaurantListViewAdapter restaurantListViewAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RestaurantList);

            restaurantListView = FindViewById<ListView>(Resource.Id.restaurantListView);
            restaurantListViewAdapter = new RestaurantListViewAdapter(this);
            restaurantListView.Adapter = restaurantListViewAdapter;
        }
    }
}


