namespace JOIEnergy.Services;

public interface IUsageCalculationService
{
    decimal GetWeeklyConsumption(string smartMeterId);
}