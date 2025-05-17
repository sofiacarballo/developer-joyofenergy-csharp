using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class PricePlanService : IPricePlanService
    {
        private readonly List<PricePlan> _pricePlans;
        private readonly IMeterReadingService _meterReadingService;

        public PricePlanService(List<PricePlan> pricePlan, IMeterReadingService meterReadingService)
        {
            _pricePlans = pricePlan;
            _meterReadingService = meterReadingService;
        }

        private decimal CalculateAverageReading(List<ElectricityReading> electricityReadings)
        {
            var newSummedReadings = electricityReadings.Select(readings => readings.Reading).Aggregate((reading, accumulator) => reading + accumulator); 

            return newSummedReadings / electricityReadings.Count(); 
        }

        private decimal CalculateTimeElapsed(List<ElectricityReading> electricityReadings)
        {
            var first = electricityReadings.Min(reading => reading.Time);
            var last = electricityReadings.Max(reading => reading.Time);

            return (decimal)(last - first).TotalHours;
        }
        public decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
        {
            var average = CalculateAverageReading(electricityReadings);
            var timeElapsed = CalculateTimeElapsed(electricityReadings);
            var averagedCost = average/timeElapsed;
            return Math.Round(averagedCost * pricePlan.UnitRate, 3);
        }
        
        public decimal CalculateCost(List<ElectricityReading> electricityReadings, string pricePlanName)
        {
            var pricePlan = _pricePlans.FirstOrDefault(plan => plan.PlanName == pricePlanName);
            
            return CalculateCost(electricityReadings, pricePlan);
        }
        
        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);

            if (!electricityReadings.Any())
            {
                return new Dictionary<string, decimal>();
            }
            return _pricePlans.ToDictionary(plan => plan.PlanName, plan => CalculateCost(electricityReadings, plan));
        }
    }
}
