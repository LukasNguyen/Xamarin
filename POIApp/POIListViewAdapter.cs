
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace POIApp
{
    public class POIListViewAdapter : BaseAdapter<PointOfInterest>
    {
        private readonly Activity _context;

        public Location CurrentLocation { get; set; }

        public POIListViewAdapter(Activity context)
        {
            _context = context;
        }

        #region implemented abstract members of BaseAdapter

        public override PointOfInterest this[int index]
        {
            get { return POIData.Service.POIs[index]; }
        }

        public override int Count
        {
            get { return POIData.Service.POIs.Count; }
        }

        public override long GetItemId(int position)
        {
            return POIData.Service.POIs[position].Id.Value;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.POIListItem, null);

            var poi = POIData.Service.POIs[position];
            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = poi.Name;
            if (string.IsNullOrWhiteSpace(poi.Address))
                view.FindViewById<TextView>(Resource.Id.addressTextView).Visibility = ViewStates.Gone;
            else
                view.FindViewById<TextView>(Resource.Id.addressTextView).Text = poi.Address;

            if (CurrentLocation != null && poi.Latitude.HasValue && poi.Longitude.HasValue)
            {
                var poiLocation = new Location(String.Empty);
                poiLocation.Latitude = poi.Latitude.Value;
                poiLocation.Longitude = poi.Longitude.Value;
                float distance = CurrentLocation.DistanceTo(poiLocation) * 0.000621371F; // this magic number converts from meters to miles

                view.FindViewById<TextView>(Resource.Id.distanceTextView).Text = String.Format("{0:0,0.00} miles", distance);
            }
            else
                view.FindViewById<TextView>(Resource.Id.distanceTextView).Text = "??";

            return view;
        }

        #endregion
    }
}

