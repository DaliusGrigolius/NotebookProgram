using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NotebookProgram.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult HandleError([FromServices] IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                var exeptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

                return Problem(
                    detail: exeptionHandlerFeature.Error.StackTrace,
                    title: exeptionHandlerFeature.Error.Message);
            }
            else
            {
                return Problem("Unhandled error occured. Contact support, please");
            }
        }
    }
}
