using JOIEnergy.Services;
using Moq;
using JOIEnergy.Domain;
using System.Collections.Generic;

namespace JOIEnergy.Tests
{
    public class PricePlanServiceTest
    {
        private PricePlanService _pricePlanService;
        private readonly Mock<IMeterReadingService> _mockMeterReadingService;
        private List<PricePlan> _pricePlans;

        public PricePlanServiceTest()
        {
            _mockMeterReadingService = new Mock<IMeterReadingService>();
            _pricePlanService = new PricePlanService(_pricePlans, _mockMeterReadingService.Object);

            _mockMeterReadingService.Setup(service => service.GetReadings(It.IsAny<string>())).Returns(new List<ElectricityReading>(){new ElectricityReading(),
                new ElectricityReading()});
        }
    }
}
