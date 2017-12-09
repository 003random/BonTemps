using System.Web;
using System.Web.Optimization;

namespace BonTemps
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/defaultscripts").Include(
                "~/asset/js/jquery.ui.min.js",
                "~/asset/js/plugins/jquery.validate.min.js",
                "~/asset/js/bootstrap.min.js",
                "~/asset/js/plugins/moment.min.js",
                "~/asset/js/plugins/chart.min.js",
                "~/asset/js/plugins/jquery.knob.js",
                "~/asset/js/plugins/ion.rangeSlider.min.js",
                "~/asset/js/plugins/bootstrap-material-datetimepicker.js",
                "~/asset/js/plugins/jquery.nicescroll.js",
                "~/asset/js/plugins/jquery.mask.min.js",
                "~/asset/js/plugins/select2.full.min.js",
                "~/asset/js/plugins/nouislider.min.js",
                "~/asset/js/plugins/icheck.min.js",
                "~/asset/js/plugins/alertify.min.js",
                "~/asset/js/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/asset/js/jquery.min.js"
               ));

            bundles.Add(new StyleBundle("~/Content/defaultcss").Include(
                "~/asset/css/bootstrap.min.css",
                "~/asset/css/plugins/font-awesome.min.css",
                "~/asset/css/plugins/animate.min.css",
                "~/asset/css/plugins/nouislider.min.css",
                "~/asset/css/plugins/select2.min.css",
                "~/asset/css/plugins/ionrangeslider/ion.rangeSlider.css",
                "~/asset/css/plugins/ionrangeslider/ion.rangeSlider.skinFlat.css",
                "~/asset/css/plugins/bootstrap-material-datetimepicker.css",
                "~/asset/css/plugins/simple-line-icons.css",
                "~/asset/css/plugins/icheck/skins/flat/aero.css",
                "~/asset/css/plugins/alertify.min.css",
                "~/asset/css/plugins/alertify-default.css",
                "~/asset/css/plugins/semantic.min.css",
                "~/asset/css/style.css"
            ));
        }
    }
}
