#pragma checksum "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5309b50e785b83aace95f8daf75b3daa12d541c5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ProductsDatabase_CreateDatabase), @"mvc.1.0.view", @"/Views/ProductsDatabase/CreateDatabase.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5309b50e785b83aace95f8daf75b3daa12d541c5", @"/Views/ProductsDatabase/CreateDatabase.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3409a79fc3eba7cbb335e235f9af3aefb12acb32", @"/Views/_ViewImports.cshtml")]
    public class Views_ProductsDatabase_CreateDatabase : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<System.Boolean>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(" \r\n");
#nullable restore
#line 8 "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml"
 if( Model )
{
    

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-info\" style=\"width: 100%\"> Структура данных успешно создана</div>\r\n    <div><a href=\"/Home/Index\"> На главную </a></div>\r\n");
#nullable restore
#line 13 "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml"
}
else
{


    

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml"
     if (db.Database.CanConnect())
    {            

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <div class=""alert alert-info"" style=""width: 100%""> Структура данных была создана ранее</div>
        <div><a href=""/ProductsDatabase/DeleteDatabase""> Удалить структуру </a></div>
        <div><a href=""/ProductsXml/InitXml""> Импорт первичных данных </a></div>
        <div><a href=""/ProductsSearch/Index""> На страницу поиска </a></div>
        <div><a href=""/Home/Index""> На главную </a></div>
");
#nullable restore
#line 26 "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <div class=""alert alert-danger"" style=""width: 100%""> Не удалось создать структуру данных , необходимо преверить соединение с источником.</div>
        <div><a href=""/ProductsDatabase/Config""> Перейти к настройки соединения с источником </a></div>
        <div><a href=""/Home/Index""> На главную </a>  </div>
");
#nullable restore
#line 32 "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "D:\NetProjects\Mvc_Apteka\Views\ProductsDatabase\CreateDatabase.cshtml"
       
      

}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public AppDbContext db { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<System.Boolean> Html { get; private set; }
    }
}
#pragma warning restore 1591
