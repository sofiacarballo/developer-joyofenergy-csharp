using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private const string SmartMeterId = "smart-meter-id";

        private readonly MeterReadingService _meterReadingService;

        public MeterReadingServiceTest()
        {
            _meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(_meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>() {
                new() { Time = DateTime.Now, Reading = 25m },
                new() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
            });

            var electricityReadings = _meterReadingService.GetReadings(SmartMeterId);

            Assert.Equal(3, electricityReadings.Count);
        }

        [Fact]
        public void GivenAPeriodThenMeterReadingShouldReturnWeeklyConsumption()
        {
            var currentDate = DateTime.Now; 
            var readings = new List<ElectricityReading>()
            {
                new() { Time = DateTime.Now.AddDays(-10), Reading = 30m },
                new() { Time = DateTime.Now.AddDays(-7), Reading = 20m },
                new() { Time = DateTime.Now.AddDays(-5), Reading = 10m },
                new() { Time = currentDate, Reading = 10m },
            };
            
            _meterReadingService.StoreReadings(SmartMeterId, readings);

            var result = _meterReadingService.GetWeeklyReadings(SmartMeterId, currentDate);
            
            Assert.Equal(3, result.Count);
            Assert.DoesNotContain(new ElectricityReading { Time = DateTime.Now.AddDays(-10), Reading = 30m }, result);
        }
    }
}
