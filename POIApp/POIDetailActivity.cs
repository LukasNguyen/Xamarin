
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
    [Activity(Label = "POI Detail")]			
    public class POIDetailActivity : Activity
    {
        private EditText _nameEditText;
        private EditText _descEditText;
        private EditText _addrEditText;
        private EditText _latEditText;
        private EditText _longEditText;
        private ImageView _poiImageView;

        private PointOfInterest _poi;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.POIDetail);

            _nameEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            _descEditText = FindViewById<EditText>(Resource.Id.descEditText);
            _addrEditText = FindViewById<EditText>(Resource.Id.addrEditText);
            _latEditText = FindViewById<EditText>(Resource.Id.latEditText);
            _longEditText = FindViewById<EditText>(Resource.Id.longEditText);
            _poiImageView = FindViewById<ImageView>(Resource.Id.poiImageView);

            if (Intent.HasExtra("poiId"))
            {
                var poiId = Intent.GetIntExtra("poiId", -1);
                _poi = POIData.Service.GetPOI(poiId);
            }
            else
            {
                _poi = new PointOfInterest();
            }

            UpdateUI();
        }

        protected void UpdateUI()
        {
            _nameEditText.Text = _poi.Name;
            _descEditText.Text = _poi.Description;
            _addrEditText.Text = _poi.Address;
            _latEditText.Text = _poi.Latitude.ToString();
            _longEditText.Text = _poi.Longitude.ToString();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.POIDetailMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionSave:
                    SavePOI();
                    return true;

                case Resource.Id.actionDelete:
                    DeletePOI();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected void SavePOI()
        {

        }

        protected void DeletePOI()
        {

        }
    }
}

