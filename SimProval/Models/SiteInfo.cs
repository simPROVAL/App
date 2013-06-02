using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SimProval.Models
{
    public enum StructureTypes {Farm, Residential, LightIndustrial};
    
    public class SiteInfo
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public StructureTypes Structure { get; set; }
        
        public double Height { get; set; }

    }
    
    public class SiteCoord
    {
        public double latitude;
        public double longitude;
    }

    public class SiteSurvey
    {
        public SiteCoord Coord;

        public string Region;
        public double Vsit;

        public double WindDirection; //Md;
        public double Shielding; //Ms; 
        public double MzCat; // TerrainCategory; 
        public double Topographic; // Mt
    }
}