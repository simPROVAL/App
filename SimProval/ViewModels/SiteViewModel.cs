using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimProval.ViewModels
{
    using Models;
    using Helpers;

    public class SiteViewModel
    {
     
        public int Id { get; set; }
        public string Address { get; set; }

        [DisplayFormat(DataFormatString = "{0:F3}")] 
        public double Height { get; set; }

        // Structure type is unnecessary prompt at this stage (minimise data entry)
        //public StructureTypes Structure { get; set; }
        //public SelectList StructureList { get; set; }


    }
}