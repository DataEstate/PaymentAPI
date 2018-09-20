using System;
using System.Collections.Generic;

namespace DataEstate.Payment.Models.Pages
{
    public class PageModel
    {
        public string Title = "";

        public string Subtitle = "";

        public string BrandName = "";

        //Styles, maybe move later. All Data Estate Styles at the moment. Maybe we can set it to configs. 
        public string LogoUrl { get; set; }

        public string PrimaryFont = "Arial, Helvetica, sans-serif";

        public string BrandHeaderColor = "#0061A1";

        public string BrandHeaderFontColor = "#ffffff";

        public string BrandFontColor = "#444444";
    }
}
