#pragma checksum "C:\Users\Josee\source\repos\ladoctora.index\ladoctora.index\Views\Shared\_Layout.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b6a347a57d29730819f4ccd48befbf4b91bb4071"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Layout), @"mvc.1.0.view", @"/Views/Shared/_Layout.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Josee\source\repos\ladoctora.index\ladoctora.index\Views\_ViewImports.cshtml"
using ladoctora.index;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Josee\source\repos\ladoctora.index\ladoctora.index\Views\_ViewImports.cshtml"
using ladoctora.index.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b6a347a57d29730819f4ccd48befbf4b91bb4071", @"/Views/Shared/_Layout.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e6f8b1d8cdd4f127afc31bc53d8df52511813235", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Layout : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b6a347a57d29730819f4ccd48befbf4b91bb40713298", async() => {
                WriteLiteral("\r\n\r\n    <meta charset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">\r\n    <meta name=\"description\"");
                BeginWriteAttribute("content", " content=\"", 195, "\"", 205, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n    <meta name=\"author\"");
                BeginWriteAttribute("content", " content=\"", 232, "\"", 242, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n\r\n    <title>");
#nullable restore
#line 11 "C:\Users\Josee\source\repos\ladoctora.index\ladoctora.index\Views\Shared\_Layout.cshtml"
      Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</title>

    <!-- Bootstrap -->
    <link href=""css/bootstrap.min.css"" rel=""stylesheet"">

    <!-- Google Web Fonts -->
    <link href=""https://fonts.googleapis.com/css?family=Yanone+Kaffeesatz:400,200,300,700"" rel=""stylesheet"" type=""text/css"">
    <link href=""https://fonts.googleapis.com/css?family=Roboto:400,100,100italic,300,300italic,400italic,500,500italic,700,700italic,900,900italic"" rel=""stylesheet"" type=""text/css"">

    <!-- Template CSS Files  -->
    <link href=""/font-awesome/css/font-awesome.min.css"" rel=""stylesheet"">
    <link href=""/js/plugins/owl-carousel/owl.carousel.css"" rel=""stylesheet"">
    <link href=""/js/plugins/owl-carousel/owl.theme.css"" rel=""stylesheet"">
    <link href=""/css/style.css"" rel=""stylesheet"">
    <link href=""/css/responsive.css"" rel=""stylesheet"">
    <link rel=""stylesheet"" href=""https://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css"" />

");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b6a347a57d29730819f4ccd48befbf4b91bb40715975", async() => {
                WriteLiteral("\r\n\r\n    ");
#nullable restore
#line 31 "C:\Users\Josee\source\repos\ladoctora.index\ladoctora.index\Views\Shared\_Layout.cshtml"
Write(RenderBody());

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
    <footer class=""main-footer"">
        <!-- Nest Container Starts -->
        <div class=""container text-xs-center text-sm-center text-md-left"">
            <!-- Nested Row Starts -->
            <div class=""row"">
                <!-- Copyright Starts -->
                <div class=""col-md-4 col-sm-12"">
                    <div class=""copyright"">
                        <h6 class=""text-uppercase"">DRA YARAESY FERMIN.</span></h6>
                        <p class=""text-uppercase text-weight-normal sub"">Medicina General</p>

                        <p class=""text-weight-light"">&copy; 2020</p>
                    </div>
                </div>
                <!-- Copyright Ends -->
                <!-- Newsletter Starts -->
                <!-- Newsletter Ends -->
                <!-- Address Starts -->
                <div class=""col-md-4 col-sm-12"">
                    <h6 class=""text-uppercase"">Dirección de atención</h6>
                    <ul class=""list-unstyled foot-address text-weig");
                WriteLiteral(@"ht-light"">
                        <li class=""clearfix"">
                            <i class=""fa fa-map-marker float-md-left""></i>
                            Medic<b>Dent</b>, Av. Portugal 140, Santiago, Región Metropolitana
                        </li>
                        <li>
                            <i class=""fa fa-envelope""></i>
                            <a href=""mailto:info@ladoctora.cl"">info@ladoctora.cl</a>
                        </li>
                    </ul>
                </div>
                <!-- Address Ends -->
            </div>
            <!-- Nested Row Ends -->
        </div>
        <!-- Nested Container Ends -->
    </footer>
    <!-- Footer Ends -->
    <!-- Template JS Files -->
    <script src=""/js/jquery-3.3.1.min.js""></script>
    <script src=""https://code.jquery.com/ui/1.10.1/jquery-ui.js""></script>
    <script src=""/js/popper.min.js""></script>
    <script src=""/js/bootstrap.min.js""></script>
    <script src=""/js/plugins/owl-carousel/owl.carous");
                WriteLiteral("el.js\"></script>\r\n    <script src=\"/js/custom.js\"></script>\r\n    <script src=\"https://cdn.jsdelivr.net/npm/sweetalert2@10\"></script>\r\n    ");
#nullable restore
#line 78 "C:\Users\Josee\source\repos\ladoctora.index\ladoctora.index\Views\Shared\_Layout.cshtml"
Write(RenderSection("Scripts", required: false));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
