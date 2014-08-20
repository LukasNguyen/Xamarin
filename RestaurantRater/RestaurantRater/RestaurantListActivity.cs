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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.RestaurantListMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNewRestaurant:
                    // TODO: open a detail activity
                    //StartActivity(typeof(POIDetailActivity));
                    Console.WriteLine("new restaurant clicked!");
                    return true;
                case Resource.Id.actionRefreshRestaurants:
                    Data.RestaurantDataService.Service.RefreshCache();
                    restaurantListViewAdapter.NotifyDataSetChanged();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}


