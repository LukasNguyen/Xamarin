
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace POIApp
{
    public class POIListViewAdapter : BaseAdapter<PointOfInterest>
    {
        private readonly Activity _context;

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

            return view;
        }

        #endregion
    }
}

