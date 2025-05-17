using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public interface IPricePlanService
    {
        Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId);

        decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan);
        decimal CalculateCost(List<ElectricityReading> electricityReadings, string pricePlanName);
    }
}