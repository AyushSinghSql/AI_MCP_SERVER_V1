using System.Text.Json;
using System.ComponentModel;
using ModelContextProtocol.Server;
using ServerMCP.Models;

namespace MCPServer.MCPTools
{
    [McpServerToolType]
    public static class PlanningTool
    {
        static readonly HttpClient httpClient = new();

        [McpServerTool, Description("Get the Employee Schedule for empl_id")]
        public static async Task<string> GetEmployeeScheduleAsync(string empl_id)
        {
            //var url = $"https://planning-api-dev.onrender.com/Forecast/GetEmployeeScheduleAsync/{Uri.EscapeDataString(empl_id)}";

            try
            {
                if (empl_id.ToUpper() == "ALL")
                {
                    empl_id = null;
                }

                return await PlanningService.GetEmployeeScheduleAsync(empl_id);
            }
            catch (Exception ex)
            {
                return $"Sorry, I couldn't fetch the weather for {empl_id}. ({ex.Message})";
            }
        }

        //        "Generate variance report for project ABC123 budget version 1 vs EAC version 3"
        //"Show BUD vs EAC variance for project P100"
        //"Create financial variance dashboard for version 2 and version 5"
        //"Variance report for all projects"
        [McpServerTool, Description("Generate Budget vs EAC variance report for a project")]
        public static async Task<string> GenerateVarianceReportAsync(
            string proj_id,
            string Type1,
            int Type1_version,
            string Type2,
            int Type2_version)
        {
            try
            {
                //-----------------------------------------
                // HANDLE NULL / ALL
                //-----------------------------------------

                if (!string.IsNullOrWhiteSpace(proj_id) &&
                    proj_id.ToUpper() == "ALL")
                {
                    proj_id = null;
                }

                //-----------------------------------------
                // VALIDATION
                //-----------------------------------------

                if (Type1_version <= 0)
                {
                    return "Budget version is required.";
                }

                if (Type2_version <= 0)
                {
                    return "EAC version is required.";
                }

                //-----------------------------------------
                // CALL SERVICE
                //-----------------------------------------

                return await PlanningService.VarianceAsync(
                     proj_id,
            Type1,
            Type1_version,
            Type2,
            Type2_version);
            }
            catch (Exception ex)
            {
                return
                    $"Sorry, I couldn't generate the variance report. ({ex.Message})";
            }
        }

        // [McpServerTool, Description("Get Revenue Analysis For Project By Year")]
        // public static async Task<string> GetRevenueAnalysisForProjectByTypeAndVersionAsync(string planId, string Year)
        // {
        //     try
        //     {
        //         return await PlanningService.GetRevenueAnalysisForProjectByTypeAndVersionAsync(planId, Year);
        //     }
        //     catch (Exception ex)
        //     {
        //         return $"Sorry, I couldn't fetch the revenue analysys for {planId}. ({ex.Message})";
        //     }
        // }

        [McpServerTool, Description("Get Revenue Analysis For Project By Year")]
public static async Task<string>
GetRevenueAnalysisForProjectByTypeAndVersionAsync(
    string planId,
    string? Year = null)
{
    try
    {
        Year = string.IsNullOrWhiteSpace(Year)
            ? DateTime.Now.Year.ToString()
            : Year;

        return await PlanningService
            .GetRevenueAnalysisForProjectByTypeAndVersionAsync(
                planId,
                Year);
    }
    catch (Exception ex)
    {
        return $"Sorry, I couldn't fetch the revenue analysis for {planId}. ({ex.Message})";
    }
}

        ///[McpServerTool, Description("Update forecast using ProjectId, PlanType, and Version instead of PlId")]
        [McpServerTool, Description(@"
Use this tool to update forecasted hours and amount using ProjectId, PlanType, and Version.

Inputs:
- ProjectId: Project identifier
- PlanType: Type of plan (e.g., Forecast, Budget)
- Version: Plan version
- SourceYear → TargetYear
- SourcePeriod → TargetPeriod
- Percentage: Optional increase
- PeriodType: Monthly / Quarterly / HalfYearly

Rules:
- Monthly: Month to Month (1–12)
- Quarterly: Q1–Q4
- HalfYearly: H1–H2
- Matches by Employee, PLC, Month, and DctId
- Updates only if target is empty
- Inserts if record does not exist

Examples:
- Copy Jan to Feb with 2%
- Move Q1 to Q2
- Shift H1 to H2 with 5%
")]
        public static async Task<string> UpdateForecastByProjectAsync(ForecastShiftByProjectRequest request)
        {
            var url = "https://planning-api-dev.onrender.com/api/ForecastReport/UpdateForecastByProject";
            url = "https://localhost:58652/api/ForecastReport/UpdateForecastByProject";



            try
            {
                using var httpClient = new HttpClient();

                var response = await httpClient.PostAsJsonAsync(url, request);

                if (!response.IsSuccessStatusCode)
                    return $"Failed: {response.StatusCode}";

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}