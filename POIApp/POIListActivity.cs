using System;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace POIApp
{
    [Activity(Label = "POIs", MainLauncher = true, ConfigurationChanges = (Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize))]
    public class POIListActivity : Activity, ILocationListener
	{
        private ListView _poiListView;
        private POIListViewAdapter _listViewAdapter;
        private LocationManager _locationManager;

        #region Activity overrides

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

            SetContentView(Resource.Layout.POIList);

            _locationManager = GetSystemService(Context.LocationService) as LocationManager;
            _poiListView = FindViewById<ListView>(Resource.Id.poiListView);
            _listViewAdapter = new POIListViewAdapter(this);
            _poiListView.Adapter = _listViewAdapter;
            _poiListView.ItemClick += OnPOIClicked;
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.POIListViewMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNew:
                    StartActivity(typeof(POIDetailActivity));
                    return true;
                case Resource.Id.actionRefresh:
                    POIData.Service.RefreshCache();
                    _listViewAdapter.NotifyDataSetChanged();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            _locationManager.RemoveUpdates(this);
        }

        protected override void OnResume()
        {
            base.OnResume();

            _listViewAdapter.NotifyDataSetChanged();

            var criteria = new Criteria
            {
                Accuracy = Accuracy.NoRequirement,
                PowerRequirement = Power.NoRequirement
            };

            var provider = _locationManager.GetBestProvider(criteria, true);
            _locationManager.RequestLocationUpdates(provider, 20000, 100, this);
        }

        #endregion

        #region ILocationListener methods

        public void OnLocationChanged(Location location)
        {
            _listViewAdapter.CurrentLocation = location;
            _listViewAdapter.NotifyDataSetChanged();
        }

        public void OnProviderDisabled(string provider)
        { }

        public void OnProviderEnabled(string provider)
        { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        { }

        #endregion

        protected void OnPOIClicked(object sender, ListView.ItemClickEventArgs e)
        {
            var poi = POIData.Service.GetPOI((int)e.Id);
            Console.WriteLine("POIClicked: Name is {0}", poi.Name);

            var poiDetailIntent = new Intent(this, typeof(POIDetailActivity));
            poiDetailIntent.PutExtra("poiId", poi.Id.Value);
            StartActivity(poiDetailIntent);
        }
	}
}