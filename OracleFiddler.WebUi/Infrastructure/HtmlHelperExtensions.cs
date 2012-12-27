using System.Web.Mvc;

namespace OracleFiddler.WebUi.Infrastructure
{
    public static class HtmlHelperExtensions
    {
         public static MvcHtmlString ActiveIf<T>(this HtmlHelper<T> helper, bool condition)
         {
             return condition ? MvcHtmlString.Create("class=\"active\"") : MvcHtmlString.Empty;
         }
    }
}