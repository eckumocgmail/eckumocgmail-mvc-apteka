#pragma checksum "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a8a55e453e82fd8c6c60d04876fb28ea5a4ab77a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Products_Table), @"mvc.1.0.view", @"/Views/Products/Table.cshtml")]
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
#line 1 "D:\NetProjects\Mvc_Apteka\Views\_ViewImports.cshtml"
using Mvc_Apteka;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\NetProjects\Mvc_Apteka\Views\_ViewImports.cshtml"
using Mvc_Apteka.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
using Mvc_Apteka.Entities;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a8a55e453e82fd8c6c60d04876fb28ea5a4ab77a", @"/Views/Products/Table.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3409a79fc3eba7cbb335e235f9af3aefb12acb32", @"/Views/_ViewImports.cshtml")]
    public class Views_Products_Table : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<ProductInfo>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@" 
 
    <table id=""ProductsTable"" class=""table""  style=""display: flex; flex-direction: column; flex-wrap: nowrap; position: static; width: 100%; height: 100%;"">
        <caption>
            <h3>Медицинские препараты</h3>
        </caption>
        <thead style=""position: static; font-weight: bold;""  id=""thead"">
            <tr style=""display: flex; flex-direction: row; flex-wrap: nowrap; width: 100%; "">
                <td colspan=""3"" style=""flex: 12;"">
               
                    <!-- строка поиска -->
                    <div>
                        <div class=""input-group flex-nowrap"">
                            <span class=""input-group-text"">$</span>
                            <input type=""text"" class=""form-control"" placeholder=""поиск"" />
                        </div>
                    </div>


                </td>
            </tr>
            <tr style=""display: flex; flex-direction: row; flex-wrap: nowrap; width: 100%;"" class=""bg-info text-white""> 
                ");
            WriteLiteral(@"<td style=""flex: 8;"">Наименование</td>
                <td style=""flex: 2;"">Цена</td>
                <td style=""flex: 2; width: 140px;"">Кол-во</td>
            </tr>
        </thead>
        
        <tbody id=""ProductsTableBody"" style=""overflow-y: auto; display: flex; flex-direction: column; flex-wrap: nowrap; position: static; width: 100%;"">
");
#nullable restore
#line 37 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
             foreach(ProductInfo product in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                <tr style=""display: flex; flex-direction: row; flex-wrap: nowrap; width: 100%;""
                    onmouseleave=""this.classList.remove('bg-primary'); this.classList.remove('text-white');"" 
                    onmouseenter=""this.classList.add('bg-primary'); this.classList.add('text-white');""");
            BeginWriteAttribute("onclick", "\r\n                    onclick=\"", 1946, "\"", 2034, 3);
            WriteAttributeValue("", 1977, "document.location.href=\'/Products/Info?ID=", 1977, 42, true);
#nullable restore
#line 42 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
WriteAttributeValue("", 2019, product.ID, 2019, 13, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2032, "\';", 2032, 2, true);
            EndWriteAttribute();
            WriteLiteral(">          \r\n                    <td style=\"flex: 8;\">");
#nullable restore
#line 43 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
                                     Write(product.ProductName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td style=\"flex: 2;\">");
#nullable restore
#line 44 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
                                     Write(product.ProductPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td style=\"flex: 2; width: 140px;\">");
#nullable restore
#line 45 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
                                                   Write(product.ProductCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 47 "D:\NetProjects\Mvc_Apteka\Views\Products\Table.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        </tbody>
 
    </table>

<script>
    (function(){
        function updateTableSize(){
            ProductsTable.style.height = (screen.height-ProductsTableBody.offsetTop-200)+'px';
        }
        updateTableSize();
        window.addEventListener('resize', function(){updateTableSize(); },true);
    })();
</script>

 ");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<ProductInfo>> Html { get; private set; }
    }
}
#pragma warning restore 1591