using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }
        public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
        {
            MeterAssociatedReadings = meterAssociatedReadings;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId) {
            if (MeterAssociatedReadings.ContainsKey(smartMeterId)) {
                return MeterAssociatedReadings[smartMeterId];
            }
            return new List<ElectricityReading>();
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings) {
            if (!MeterAssociatedReadings.ContainsKey(smartMeterId)) {
                MeterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }

            electricityReadings.ForEach(electricityReading => MeterAssociatedReadings[smartMeterId].Add(electricityReading));
        }

        public List<ElectricityReading> GetWeeklyReadings(string smartMeterId, DateTime currentDate)
        {
            if (MeterAssociatedReadings.ContainsKey(smartMeterId)) {
                return MeterAssociatedReadings[smartMeterId]
                    .Where(reading => reading.Time >= currentDate.AddDays(-7) && reading.Time <= currentDate).ToList();
            }
            return new List<ElectricityReading>();
        }
    }
}
