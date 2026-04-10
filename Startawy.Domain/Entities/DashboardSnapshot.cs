namespace startawy.Core.Entities;

public class DashboardSnapshot : BaseEntity
{
    public string   UserId                  { get; set; } = string.Empty;
    public DateTime SnapshotDate            { get; set; } = DateTime.UtcNow;
    public decimal  Revenue                 { get; set; }
    public decimal  Expenses                { get; set; }
    public decimal  NetProfit               { get; set; }
    public decimal  CashBalance             { get; set; }
    public double   BurnRate                { get; set; }
    public int      CustomerCount           { get; set; }
    public double   CustomerAcquisitionCost { get; set; }
    public double   CustomerLifetimeValue   { get; set; }
    public double   ChurnRate               { get; set; }
    public string   PredictiveInsights      { get; set; } = string.Empty;
}
