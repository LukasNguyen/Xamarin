using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace POIApp
{
    [Activity(Label = "POIs", MainLauncher = true)]
	public class POIListActivity : Activity
	{
        private ListView _poiListView;
        private POIListViewAdapter _listViewAdapter;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

            SetContentView(Resource.Layout.POIList);

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

        protected void OnPOIClicked(object sender, ListView.ItemClickEventArgs e)
        {
            var poi = POIData.Service.GetPOI((int)e.Id);
            Console.WriteLine("POICliecked: Name is {0}", poi.Name);

            var poiDetailIntent = new Intent(this, typeof(POIDetailActivity));
            poiDetailIntent.PutExtra("poiId", poi.Id.Value);
            StartActivity(poiDetailIntent);
        }
	}
}