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


        [McpServerTool, Description(@"
Get comprehensive financial data for a project. 
Returns a list containing:
- proj_f_tot_amt: Total project funding amount
- projectStatus: Current status (e.g., At Risk, On Track)
- budgetRevenue: Planned budget revenue
- forecastRevenue: Forecasted revenue
- actualRevenue: Realized actual revenue
- variance: Calculated variance between plan and actuals
")]
public static async Task<string> GetProjectFinancialsAsync(
    string projectID,
    string type,
    string? status = null)
{
    try
    {
        // The implementation assumes the service parses the JSON response 
        // containing the list of financial objects provided.
        return await PlanningService.GetProjectFinancialsAsync(projectID, type, status);
    }
    catch (Exception ex)
    {
        return $"Sorry, I couldn't fetch the financial details for project {projectID}. ({ex.Message})";
    }
}



        [McpServerTool,
Description(@"
Create the next Budget version from the latest approved/final EAC version.

Input:
- projId: Project Identifier

Process:
- Finds the latest EAC version where FinalVersion = true OR IsApproved = true
- Creates the next Budget version
- Returns the newly created version details and project summary

Examples:
- Create budget version for project P1001
- Generate next budget from latest EAC for project ABC123
- Create next budget version for project P200
")]
        public static async Task<string> CreateBudgetFromLatestEacAsync(
   string projId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(projId))
                {
                    return "Project Id is required.";
                }

                return await PlanningService
                    .CreateBudgetFromLatestEacAsync(projId);
            }
            catch (Exception ex)
            {
                return $"Sorry, I couldn't create the budget version for project {projId}. ({ex.Message})";
            }
        }

        [McpServerTool,
 Description(@"
Get project planning statistics.

Returns:
- Budget Count
- EAC Count
- Final Version
- Latest Approved EAC
- Project Status
- Project detail like revenue, funding, forecast reveue, actual revenue, etc..

Example:
- Show stats for project P1001
- Get project version summary for ABC123
")]
        public static async Task<string> GetProjectStatsAsync(
    string projId)
        {
            try
            {
                return await PlanningService
                    .GetProjectStatsAsync(projId);
            }
            catch (Exception ex)
            {
                return $"Unable to get project statistics. ({ex.Message})";
            }
        }

        [McpServerTool,
  Description(@"
Update project status.

Required:
- projectId

Optional:
- planType
- version
- status

If planType or version are not provided, the system will use the latest Final/Approved version for the project.

Examples:
- Approve project P100
- Conclude project P100
- Approve latest EAC version for project P100
- Update project P100 status to Approved
")]
        public static async Task<string>
 UpdateProjectPlanStatusAsync(
     string projectId,
     string? status = null,
     string? planType = null,
     int? version = null)
        {
            return await PlanningService
                .UpdateProjectPlanStatusAsync(
                    projectId,
                    status,
                    planType,
                    version);
        }


        [McpServerTool,
Description(@"
Get complete employee performance summary.

Inputs:
- EmployeeId

Returns:
- Employee Profile
- Forecast Hours
- Forecast Cost
- Burden Cost
- Actual Hours
- Actual Cost
- Revenue
- Project Count
")]
        public static async Task<string>
GetEmployeePerformanceAsync(
    string employeeId)
        {
            try
            {
                return await PlanningService
                    .GetEmployeePerformanceAsync(
                        employeeId);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}