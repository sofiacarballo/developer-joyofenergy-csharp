using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.Controllers;

[Route("consumptions")]
public class ConsumptionController(IUsageCalculationService usageCalculationService) : Controller
{
    [HttpGet("get-weekly-consumption/{smartMeterId}")]
    public ObjectResult GetWeeklyConsumption(string smartMeterId)
    {
        return new OkObjectResult(usageCalculationService.GetWeeklyConsumption(smartMeterId));
    }
}