using System;
using System.Collections.Generic;

namespace MCPServer
{
    public class RevenueAnalysisResponse
    {
        public List<EmployeeForecastSummary>? EmployeeForecastSummary { get; set; }
        public List<DirectCostForecastSummary>? DirectCOstForecastSummary { get; set; }
        public List<MonthlyRevenueSummary>? MonthlyRevenueSummary { get; set; }

        public decimal? TotalCost { get; set; }
        public decimal? Fees { get; set; }
        public decimal? TotalGna { get; set; }
        public decimal? TotalOverhead { get; set; }
        public decimal? TotalFringe { get; set; }
        public decimal? TotalBurden { get; set; }
        public decimal? TotalBurdenCost { get; set; }
        public decimal? TnmRevenue { get; set; }
        public decimal? CpffRevenue { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? AdjustedRevenue { get; set; }
        public decimal? AtRiskAmt { get; set; }
        public decimal? FundingValue { get; set; }
        public string? RevenueFormula { get; set; }
        public string? Proj_Id { get; set; }
        public string? Type { get; set; }
        public int? Version { get; set; }
    }

    public class EmployeeForecastSummary
    {
        public string? OrgID { get; set; }
        public string? AccID { get; set; }
        public string? EmplId { get; set; }
        public string? Name { get; set; }
        public decimal? TotalForecastedHours { get; set; }
        public decimal? PerHourRate { get; set; }
        public decimal? TotalForecastedCost { get; set; }
        public decimal? Burden { get; set; }
        public decimal? TotalBurdonCost { get; set; }
        public decimal? Fringe { get; set; }
        public decimal? Overhead { get; set; }
        public decimal? Gna { get; set; }
        public decimal? TnmRevenue { get; set; }
        public decimal? CpffRevenue { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Fees { get; set; }
        public string? PlcCode { get; set; }
        public EmplSchedule? EmplSchedule { get; set; }
    }

    public class EmplSchedule
    {
        public string? EmpId { get; set; }
        public string? PlcCode { get; set; }
        public bool? IsRev { get; set; }
        public List<PayrollSalary>? PayrollSalary { get; set; }
    }

    public class PayrollSalary
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public decimal? Salary { get; set; }
        public decimal? SalRatio { get; set; }
        public decimal? Hours { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Fringe { get; set; }
        public decimal? Overhead { get; set; }
        public decimal? Gna { get; set; }
        public decimal? Burden { get; set; }
        public decimal? TotalBurdenCost { get; set; }
        public decimal? CcffRevenue { get; set; }
        public decimal? TnmRevenue { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Fees { get; set; }
    }

    public class DirectCostForecastSummary
    {
        public string? OrgID { get; set; }
        public string? AccID { get; set; }
        public string? EmplId { get; set; }
        public string? Name { get; set; }
        public decimal? TotalForecastedHours { get; set; }
        public decimal? PerHourRate { get; set; }
        public decimal? TotalForecastedCost { get; set; }
        public decimal? Burden { get; set; }
        public decimal? TotalBurdonCost { get; set; }
        public decimal? Fringe { get; set; }
        public decimal? Overhead { get; set; }
        public decimal? Gna { get; set; }
        public decimal? TnmRevenue { get; set; }
        public decimal? CpffRevenue { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Fees { get; set; }
        public DirectCostSchedule? DirectCostSchedule { get; set; }
    }

    public class DirectCostSchedule
    {
        public int? DctId { get; set; }
        public string? AcctId { get; set; }
        public bool? IsRev { get; set; }
        public List<Forecast>? Forecasts { get; set; }
    }

    public class Forecast
    {
        public decimal? ForecastedAmt { get; set; }
        public decimal? ActualAmt { get; set; }
        public int? ForecastId { get; set; }
        public string? ProjId { get; set; }
        public int? PlId { get; set; }
        public string? EmplId { get; set; }
        public int? DctId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? TotalBurdenCost { get; set; }
        public decimal? Fees { get; set; }
        public decimal? Burden { get; set; }
        public decimal? CcffRevenue { get; set; }
        public decimal? TnmRevenue { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Cost { get; set; }
        public decimal? ForecastedCost { get; set; }
        public decimal? Fringe { get; set; }
        public decimal? Overhead { get; set; }
        public decimal? Gna { get; set; }
        public decimal? Materials { get; set; }
        public decimal? ForecastedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public string? AcctId { get; set; }
        public string? OrgId { get; set; }
        public decimal? HrlyRate { get; set; }
    }

    public class MonthlyRevenueSummary
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Cost { get; set; }
        public decimal? OtherDifrectCost { get; set; }
    }

}
