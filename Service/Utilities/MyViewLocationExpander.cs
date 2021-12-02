using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace Service.Utilities
{
    public class MyViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
             "~/Views/{1}/{0}.cshtml",
             "~/Views/Email/{0}.cshtml",
            "~/Views/Shared/{0}.cshtml",
        }; // add `.Union(viewLocations)` to add default locations
        }
    }
}