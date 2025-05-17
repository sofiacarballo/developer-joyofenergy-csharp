using System;
using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using Moq;
using Xunit;

namespace JOIEnergy.Tests;

public class UsageCalculationServiceTest
{
    private Mock<IMeterReadingService> _mockMeterReadingService;
    private Mock<IAccountService> _mockAccountService;
    private Mock<IPricePlanService> _mockPricePlanService;
    
    [Fact]
    public void GivenSmartMeterIdThenItShouldReturnWeeklyCost()
    {
        _mockMeterReadingService = new Mock<IMeterReadingService>();
        _mockAccountService = new Mock<IAccountService>();
        const string smartMeterId = "smart-meter-1";
        const string pricePlanId = "price-plan-1";

        _mockAccountService.Setup(x => x.GetPricePlanIdForSmartMeterId(smartMeterId))
            .Returns(pricePlanId);

        var electricityReadings = new List<ElectricityReading>()
        {
            new() { Reading = 20M, Time = DateTime.Now.AddDays(-5) },
            new() { Reading = 10M, Time = DateTime.Now },
        };
        
        _mockMeterReadingService.Setup(service => service.GetWeeklyReadings(smartMeterId, DateTime.Now))
            .Returns(electricityReadings);

        var pricePlan = new PricePlan()
        {
            PlanName = "test-plan-name",
            EnergySupplier = Supplier.TheGreenEco,
            PeakTimeMultiplier = new List<PeakTimeMultiplier>(),
            UnitRate = 10M
        };
        
        _mockPricePlanService = new Mock<IPricePlanService>();
        _mockPricePlanService.Setup(service => service.CalculateCost(electricityReadings, pricePlan.PlanName)).Returns(1);
        
        var usageCalculationService = new UsageCalculationService(_mockAccountService.Object, _mockMeterReadingService.Object, _mockPricePlanService.Object);
        
        var result = usageCalculationService.GetWeeklyConsumption(smartMeterId);

        Assert.Equal(1, result);
    }
}