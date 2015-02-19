using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace _2_1_galleriet
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"
            ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
              "~/Scripts/app.js"
          ));

            bundles.Add(new StyleBundle("~/Content").Include(
                "~/Content/site.css"
            ));


        }
    }
}