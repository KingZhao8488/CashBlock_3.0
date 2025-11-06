using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        if (statusCode == 404)
            return View("NotFound");  // Buscará Views/Error/NotFound.cshtml

        return View("ServerError");   // Views/Error/ServerError.cshtml
    }

    [Route("Error/500")]
    public IActionResult ServerError()
    {
        return View("ServerError");
    }
}

