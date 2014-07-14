using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace POIApp
{
	public class POIJsonService : IPOIDataService
	{
		private string _storagePath;
		private List<PointOfInterest> _pois = new List<PointOfInterest>();

		public POIJsonService(string storagePath)
		{
			_storagePath = storagePath;

			if (!Directory.Exists(_storagePath))
				Directory.CreateDirectory(_storagePath);

			RefreshCache();
		}

		#region IPOIDataService implementation

		public void RefreshCache()
		{
			_pois.Clear();

			var filenames = Directory.GetFiles(_storagePath, "*.json");
			foreach (var filename in filenames)
			{
				var poiString = File.ReadAllText(filename);
				var poi = JsonConvert.DeserializeObject<PointOfInterest>(poiString);
				_pois.Add(poi);
			}
		}

		public PointOfInterest GetPOI(int id)
		{
			return _pois.Find(p => p.Id == id);
		}

		public void SavePOI(PointOfInterest poi)
		{
			var newPoi = false;

			if (!poi.Id.HasValue)
			{
				poi.Id = getNextId();
				newPoi = true;
			}

			var poiString = JsonConvert.SerializeObject(poi);
			File.WriteAllText(getFilename(poi.Id.Value), poiString);

			if (newPoi)
				_pois.Add(poi);
		}

		public void DeletePOI(PointOfInterest poi)
		{
			File.Delete(getFilename(poi.Id.Value));
			_pois.Remove(poi);
		}

		public System.Collections.Generic.IReadOnlyList<PointOfInterest> POIs
		{
			get { return _pois; }
		}

		#endregion

		private int getNextId()
		{
			if (_pois.Count == 0)
				return 1;

			return _pois.Max(x => x.Id.Value) + 1;
		}

		private string getFilename(int id)
		{
			return Path.Combine(_storagePath, "poi" + id.ToString() + ".json");
		}
	}
}

