using System.Web.Http;
using System.Web.UI.WebControls;

class WebApiConfig
{
    public static void Register(HttpConfiguration configuration)
    {
        configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
            new { id = System.Web.Http.RouteParameter.Optional });
        configuration.EnableCors();
    }
}