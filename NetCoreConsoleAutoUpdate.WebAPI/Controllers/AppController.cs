using Microsoft.AspNetCore.Mvc;

namespace NetCoreConsoleAutoUpdate.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        public AppController()
        {
        }

        [HttpGet]
        [Route("GetVersion")]
        public dynamic GetVersion()
        {
            var request = base.Url.ActionContext.HttpContext.Request;
            return new
            {
                version = "1.0.0.1",//TODO: ª©¥»¦b NetCoreConsoleAutoUpdate.WpfApp.csproj¡A
                url = $"{request.Scheme}://{request.Host}/Apps/app.zip?_t={DateTime.Now.ToString("yyyyMMddHHmmssfff")}",
                mandatory = true,
            };
        }
    }
}