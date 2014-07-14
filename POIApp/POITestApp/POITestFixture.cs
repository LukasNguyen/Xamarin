using System;
using System.IO;
using NUnit.Framework;

using POIApp;

namespace POITestApp
{
	[TestFixture]
	public class POITestFixture
	{
		private IPOIDataService _poiService;
		private string _storagePath;

		[SetUp]
		public void Setup()
		{
			_storagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			_poiService = new POIJsonService(_storagePath);

			foreach (var filename in Directory.EnumerateFiles(_storagePath, "*.json"))
				File.Delete(filename);
		}

		[TearDown]
		public void Tear()
		{
			foreach (var filename in Directory.EnumerateFiles(_storagePath, "*.json"))
				File.Delete(filename);
		}

		[Test]
		public void CreatePOI()
		{
			var newPoi = new PointOfInterest
			{
				Name = "New POI",
				Description = "POI to test creating a new POI",
				Address = "100 Main St.\nAnywhere, TX 75069"
			};

			_poiService.SavePOI(newPoi);

			var testId = newPoi.Id.GetValueOrDefault();

			_poiService.RefreshCache();

			var poi = _poiService.GetPOI(testId);
			Assert.IsNotNull(poi);
			Assert.AreEqual(poi.Name, "New POI");
		}

		[Test]
		public void UpdatePOI()
		{
			var newPoi = new PointOfInterest
			{
				Name = "Update POI",
				Description = "POI being saved so we can test update",
				Address = "100 Main St.\nAnywhere, TX 75069"
			};

			_poiService.SavePOI(newPoi);

			var testId = newPoi.Id.GetValueOrDefault();

			_poiService.RefreshCache();

			var newDescription = "Updated description for Update POI";
			var updatePoi = _poiService.GetPOI(testId);
			updatePoi.Description = newDescription;
			_poiService.SavePOI(updatePoi);

			_poiService.RefreshCache();

			var poi = _poiService.GetPOI(testId);
			Assert.IsNotNull(poi);
			Assert.AreEqual(newDescription, poi.Description);
		}

		[Test]
		public void DeletePOI()
		{
			var newPoi = new PointOfInterest
			{
				Name = "Delete POI",
				Description = "POI being saved so we can test delete",
				Address = "100 Main St.\nAnywhere, TX 75069"
			};

			_poiService.SavePOI(newPoi);

			var testId = newPoi.Id.GetValueOrDefault();

			_poiService.RefreshCache();

			var deletePoi = _poiService.GetPOI(testId);
			Assert.IsNotNull(deletePoi);
			_poiService.DeletePOI(deletePoi);

			_poiService.RefreshCache();

			var poi = _poiService.GetPOI(testId);
			Assert.IsNull(poi);
		}
	}
}

