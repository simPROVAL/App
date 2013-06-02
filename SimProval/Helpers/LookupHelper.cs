using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimProval.Helpers
{
    public class LookupHelper
    {
        public static IEnumerable GetStructureTypeList()
        {
            return new List<LookupType> { new LookupType() { Value = "0", Text = "Domestic" }, new LookupType() { Value = "1", Text = "Farm" }, new LookupType() { Value = "2", Text = "Light Industrial" } };
        }
    }

    public class LookupType
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}