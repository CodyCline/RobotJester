using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotJester.Controllers;

//For handling all bad HTTP requests and serving a single error
public class StatusCodeController : Controller
{
	private readonly ILogger<StoreController> _logger;
  public StatusCodeController(ILogger<StoreController> logger)
  {
	_logger = logger;
  }
  // GET: /<controller>/
  [HttpGet("/StatusCode/{statusCode}")]
  public IActionResult Index(int statusCode)
  {
	var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
	_logger.LogInformation($"Unexpected Status Code: {statusCode}, OriginalPath: {reExecute.OriginalPath}");
	return View(statusCode);
  }
}