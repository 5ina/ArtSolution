using System.Web.Optimization;

namespace ArtSolution.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            
            //~/Bundles/vendor/css
            bundles.Add(
                new StyleBundle("~/admin/css")              
                    .Include("~/Content/bootstrap.css")
                    .Include("~/Content/themes/base/all.css")
                    .Include("~/Content/toastr.css")
                    .Include("~/admin-lte/plugins/select2/select2.css")
                    .Include("~/Scripts/sweetalert/sweet-alert.css")
                    .Include("~/Content/flags/famfamfam-flags.css")
                    .Include("~/Content/font-awesome.css")                  
                );

            //~/Resource/js/top (These scripts should be included in the head of the page)
            bundles.Add(
                new ScriptBundle("~/admin/js/top")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        "~/Scripts/modernizr-2.8.3.js"
                    )
                );

            //~/Resource/js/bottom (Included in the bottom for fast page load)
            bundles.Add(
                //new ScriptBundle("~/Resource/js/bottom")
                new ScriptBundle("~/admin/js")
                    .Include(
                        "~/Scripts/json2.js",
                        "~/Scripts/jquery-2.1.4.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/bootstrap.js",

                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/sweetalert/sweet-alert.js",
                        "~/Scripts/others/spinjs/spin.js",
                        "~/Scripts/others/spinjs/jquery.spin.js",
                        "~/Scripts/admin.common.js",


                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js"
                    )
                );

            //APPLICATION RESOURCES

            //~/admin/main
            bundles.Add(
                new StyleBundle("~/admin/main")
                    .Include("~/css/main.css")
                    .Include("~/admin-lte/css/AdminLTE.css")
                    .Include("~/admin-lte/css/skins/_all-skins.css")
                    .Include("~/Content/admin.css")
                );

            //~/js
            bundles.Add(
                new ScriptBundle("~/admin/script")
                    .Include("~/js/main.js")
                    .Include("~/admin-lte/js/app.js")
                    .Include("~/Scripts/admin.navigation.js")
                    .Include("~/admin-lte/skin.js")
                    .Include("~/Scripts/kendo/2014.1.318/kendo.web.js")
                    .Include("~/Scripts/kendo/2014.1.318/cultures/kendo.culture.zh-CN.min.js")
                    .Include("~/Scripts/kendo/2014.1.318/cultures/kendo.messages.zh-CN.js")
                );

            #region Login


            //~/css
            bundles.Add(
                new StyleBundle("~/admin/login/css")
                    .Include("~/Content/admin-login.css")
                    .Include("~/Content/bootstrap.css")
                    .Include("~/admin-lte/css/AdminLTE.css")
                    .Include("~/admin-lte/css/skins/_all-skins.css")
                    .Include("~/Content/flags/famfamfam-flags.css")
                    .Include("~/Content/font-awesome.css")
                );

            //~/js
            bundles.Add(
                new ScriptBundle("~/admin/login/js")
                    .Include(
                        "~/Scripts/jquery-2.1.4.js",
                        "~/Scripts/bootstrap.js",
                        "~/admin-lte/plugins/iCheck/icheck.js"
                        )
                );
            #endregion

            #region shop
            //CSS
            bundles.Add(
                new StyleBundle("~/shop/css")
                    /* <!-- CSS Global Compulsory --> */
                    .Include("~/Content/bootstrap.css")
                    .Include("~/Content/shop.style.css")
                    /* <!-- CSS Implementing Plugins --> */
                    .Include("~/Content/font-awesome.css")
                    .Include("~/Scripts/sweetalert/sweet-alert.css")
                    .Include("~/Content/plugins/scrollbar/src/perfect-scrollbar.css")
                    .Include("~/Content/plugins/owl-carousel/owl-carousel/owl.carousel.css")
                    .Include("~/Content/plugins/revolution-slider/rs-plugin/css/settings.css")

                    /* < !--Style Switcher-- >*/
                    .Include("~/Content/plugins/style-switcher.css")
                    /*< !--CSS Customization-- >*/
                    .Include("~/Content/custom.css")
                );


            //Js

            //bundles.Add(
            //    new ScriptBundle("~/shop/js")

            //        .Include(                     
            //            )
            //    );


            bundles.Add(new ScriptBundle("~/shop/js").Include(
                        //JS Global Compulsory
                        "~/Scripts/jquery-2.1.4.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery-migrate-3.0.0.js",
                        "~/Scripts/bootstrap.js",
                        //JS Implementing Plugins
                        //"~/Content/plugins/back-to-top.js",
                        "~/Content/plugins/scrollbar/src/jquery.mousewheel.js",
                        "~/Content/plugins/scrollbar/src/perfect-scrollbar.js",
                        "~/Content/plugins/jquery.parallax.js",
                        "~/Scripts/sweetalert/sweet-alert.js",
                        "~/Content/plugins/revolution-slider/rs-plugin/js/jquery.themepunch.tools.min.js",
                        "~/Content/plugins/revolution-slider/rs-plugin/js/jquery.themepunch.revolution.min.js",
                        // JS Customization
                        "~/Scripts/plugins/revolution-slider.js",
                        "~/Scripts/plugins/style-switcher.js"
                               ));
            #endregion

            BundleTable.EnableOptimizations = false;
        }
    }
}