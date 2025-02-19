using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private static string SMART_METER_ID = "smart-meter-id";

        private MeterReadingService meterReadingService;

        public MeterReadingServiceTest()
        {
            meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new() { Time = DateTime.Now, Reading = 25m },
                new() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
            });

            var electricityReadings = meterReadingService.GetReadings(SMART_METER_ID);

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
            
            meterReadingService.StoreReadings(SMART_METER_ID, readings);

            var result = meterReadingService.GetWeeklyReadings(SMART_METER_ID, currentDate);
            
            Assert.Equal(3, result.Count);
        }
    }
}
