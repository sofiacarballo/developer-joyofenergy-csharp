using System;

namespace JOIEnergy.Services;

public class UsageCalculationService : IUsageCalculationService
{
    private readonly IAccountService _accountService;
    private readonly IMeterReadingService _meterReadingService;
    private readonly IPricePlanService _pricePlanService;

    public UsageCalculationService(IAccountService accountService, IMeterReadingService meterReadingService, IPricePlanService pricePlanService)
    {
        _accountService = accountService;
        _meterReadingService = meterReadingService;
        _pricePlanService = pricePlanService;
    }
    public decimal GetWeeklyConsumption(string smartMeterId)
    {
        var planId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
        var readings = _meterReadingService.GetWeeklyReadings(smartMeterId, DateTime.Now);
        var cost = _pricePlanService.CalculateCost(readings, planId);

        return cost;
    }
}