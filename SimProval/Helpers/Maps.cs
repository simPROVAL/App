using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Spatial;
using GoogleMapsApi;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.Geocoding.Request;

namespace SimProval.Helpers
{
    using Models;

    public static class Maps
    {
        public static SiteCoord GetLocForAddress(string address) //SiteViewModel
        {

            GeocodingRequest geocodeRequest = new GeocodingRequest()
            {
                Address = address + ", " + ", Australia"
            };

            GeocodingEngine geocodingEngine = new GeocodingEngine();
            GeocodingResponse geocode = geocodingEngine.GetGeocode(geocodeRequest);

            SiteCoord sc = new SiteCoord();
            sc.latitude = geocode.Results.FirstOrDefault().Geometry.Location.Latitude;
            sc.longitude = geocode.Results.FirstOrDefault().Geometry.Location.Longitude;

            return sc;

        }

        public static string GetRegion(SiteCoord coord)
        {
            return "A";
            // check longitude
            // check distance from coastline

        }

        public static double GetShielding(SiteCoord coord)
        {
            // Get boundary of this property
            // Get the size of surrounding blocks
            // Perform calc on assumption of a house on each block

            return (double)DesignWindCalculation.FS;
        }


        public static DesignWindCalculation.TerrainCategories GetTerrainCategory(SiteCoord Coord)
        {
            // Check Zoning for Rural or otherwise
            // 

            return DesignWindCalculation.TerrainCategories.TC2_5;
        }

        public static double TerrainCategoryAsDouble(DesignWindCalculation.TerrainCategories tc)
        {
            switch (tc)
            {
                case DesignWindCalculation.TerrainCategories.TC1: return 1.0;
                case DesignWindCalculation.TerrainCategories.TC2: return 2.0;
                case DesignWindCalculation.TerrainCategories.TC2_5: return 2.5;
                case DesignWindCalculation.TerrainCategories.TC3: return 3;

                default: return 2.5; //DesignWindCalculation.TerrainCategories.TC2_5 
            };

        }
    }
}