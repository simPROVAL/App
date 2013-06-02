using SimProval.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimProval.Models
{
    public class ParamException : Exception { 
    };

    public class DesignWindCalculation
    {
        public const int ParamError = -999;

        public const double FS = 0.80;
        public const double PS = 0.90;
        public const double NS = 1.00;
        public enum TerrainCategories { TC1, TC2, TC2_5, TC3 };

        private double Vr;
        private TerrainCategories TC = TerrainCategories.TC2_5;
        private string region;

        private double Md = 1;
        private double Ms = 1;  // Shielding Factor
        private double MzCat;
        private double Mt = 1;
         
        public SiteCoord Coord = null;

        public int Importance = 2; 
        public string Region {
            get { return region; }
            set
            {
                region = value;

                if (Importance ==1)  // Farm
                {
                    switch (value)
                    {
                        case "A": Vr = 45.0; break;
                        case "B": Vr = 57.0; break;
                        case "C": Vr = 69.0; break;
                        case "D": Vr = 88.0; break;
                    }
                }
                else if (Importance == 2)  // Residential etc
                {
                    switch (value)
                    {
                        case "A": Vr = 43.0; break;
                        case "B": Vr = 52.0; break;
                        case "C": Vr = 64.0; break;
                        case "D": Vr = 79.0; break;
                    }

                }
                else
                {
                    Vr = ParamError; 
                }
            }
        }



        public double WindDirection { get {return Md;} set { Md = value;}}
        public double Shielding { get {return Ms;} set { Ms = value;}} 
        public TerrainCategories TerrainCategory { get {return TC;} set { TC = value;}} 
        public double Topographic { get {return Mt;} set { Mt = value;}} 

        public double Vsit
        {
            get
            {
                if (Vr == ParamError)
                    return ParamError;
                else
                    return Vr * Md * MzCat * Ms * Mt;

            }
        }

        public double For(Site site)
        {
            try
            {
                Coord = Maps.GetLocForAddress(site.Address);
                Region = Maps.GetRegion(Coord);

                Shielding = Maps.GetShielding(Coord);
                TerrainCategory = Maps.GetTerrainCategory(Coord);

                MzCat = TerrainCategoryToMultiplier(TerrainCategory, (double)site.Height);

                return Vsit;
            }
            catch(Exception e)
            {
                return ParamError;
            }
        }

        private double TerrainCategoryToMultiplier(TerrainCategories tc, double height)
        {
            if (tc == TerrainCategories.TC3)
                return 0.83; 
            else 
            if (height > 10)
            {
                throw new ParamException(); //("Buildings higher than 10 are not supported", 0);
            } else
            {
                if (height <= 3.000)
                {
                    switch (tc)
                    {
                        case TerrainCategories.TC1: return 0.99;
                        case TerrainCategories.TC2: return 0.91;
                        case TerrainCategories.TC2_5: return 0.87;
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TerrainCategories.TC1: return Interpolate(1.05, 1.12, height);
                        case TerrainCategories.TC2: return Interpolate(0.91, 1.00, height);
                        case TerrainCategories.TC2_5: return Interpolate(0.87, 0.92, height);
                    }

                }
            }
            return ParamError;
        }


        public double Interpolate(double from5, double to10, double h)
        {
            return ((to10 - from5) / 5.0) * (h - 5.0);
        }


        public SiteSurvey Result(ref Site s)
        {
            SiteSurvey survey = new SiteSurvey();

            survey.Coord = Coord;
            survey.Region = Region;
            survey.Shielding = Shielding;
            survey.MzCat = MzCat; // TerrainCategory;
            survey.Topographic = Topographic;

            survey.Vsit = Vsit;

            // for DB write back
            s.Structure = (int)StructureTypes.Residential;
            s.Region = Region;
            s.DesignSpeed = Vsit;

            s.Shielding = Shielding;           
            s.TerrainCategory = Maps.TerrainCategoryAsDouble(TerrainCategory);
            s.Topography = MzCat;

            s.Date = DateTime.Now;          

            // write out location data point
            string point = string.Format("POINT({0} {1})", Coord.longitude, Coord.latitude);
            s.Location = System.Data.Spatial.DbGeography.PointFromText(point, 4326);          

            return survey;
        }
    }
}