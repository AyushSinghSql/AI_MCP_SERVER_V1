using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using ServerMCP.Models;

public static class PlanningService
{
    private static readonly HttpClient httpClient =
        new HttpClient();

    //---------------------------------------------------------
    // CONFIGURATION
    //---------------------------------------------------------

    public static IConfiguration? Configuration
    {
        get;
        set;
    }

    //---------------------------------------------------------
    // EMPLOYEE SCHEDULE
    //---------------------------------------------------------

    public static async Task<string>
        GetEmployeeScheduleAsync(
        string empl_id)
    {
        try
        {
            //-------------------------------------------------
            // READ URL FROM CONFIG
            //-------------------------------------------------

            var baseUrl =
                Configuration?["ApiSettings:PlanningApiUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new Exception(
                    "PlanningApiUrl is missing in configuration.");
            }

            //-------------------------------------------------
            // BUILD URL
            //-------------------------------------------------

            //var url =
            //    $"{baseUrl}/Forecast/GetEmployeeScheduleAsyncV1/" +
            //    $"{Uri.EscapeDataString(empl_id)}";

            string url = string.IsNullOrWhiteSpace(empl_id)
    ? $"{baseUrl}/Forecast/GetEmployeeScheduleAsyncV1"
    : $"{baseUrl}/Forecast/GetEmployeeScheduleAsyncV1/{Uri.EscapeDataString(empl_id)}";

            //-------------------------------------------------
            // CALL API
            //-------------------------------------------------

            using var response =
                await httpClient.PostAsync(url, null);

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content
                .ReadAsStringAsync();

            //-------------------------------------------------
            // AI PROMPT
            //-------------------------------------------------

            //var prompt =
            //    "Convert the following structured employee " +
            //    "schedule data into friendly human-readable " +
            //    "HTML format.";

            var prompt = @"
                You are a senior workforce and financial operations analyst preparing an executive-level Employee Utilization Report for CFOs, PMO leadership, and executive management.

                Convert the provided employee schedule and utilization data into a professional, presentation-ready HTML executive dashboard report.

                REPORT OBJECTIVES:
                - Executive presentation quality
                - Workforce planning insights
                - Resource utilization visibility
                - Capacity planning analysis
                - Financial operations support
                - PMO decision-making support

                REQUIRED REPORT SECTIONS:

                1. Executive Summary
                   - Overall workforce utilization
                   - Resource capacity overview
                   - Underutilized resources
                   - Overallocated resources
                   - Staffing risks
                   - Forecast staffing outlook
                   - Key workforce observations

                2. Workforce KPI Dashboard
                   Display KPI cards for:
                   - Total Employees
                   - Average Utilization %
                   - Total Scheduled Hours
                   - Available Capacity
                   - Billable Utilization
                   - Non-Billable Utilization
                   - Overallocated Employees
                   - Underutilized Employees
                   - Forecasted Capacity
                   - Labor Efficiency %

                3. Employee Utilization Analysis
                   - Employee-wise utilization trends
                   - Monthly utilization analysis
                   - Capacity vs scheduled hours
                   - Billable vs non-billable analysis
                   - Resource efficiency commentary

                4. Project Allocation Analysis
                   - Employee allocation by project
                   - Top resource-consuming projects
                   - Resource distribution trends
                   - Cross-project staffing insights

                5. Workforce Capacity Planning
                   - Available capacity
                   - Excess workload analysis
                   - Staffing shortages
                   - Hiring or reallocation recommendations
                   - Future utilization forecast

                6. Schedule & Workload Insights
                   - Monthly work schedule analysis
                   - Working hours trends
                   - Peak workload periods
                   - Resource balancing opportunities

                7. Risk & Operational Analysis
                   - Burnout risks
                   - Idle resource risks
                   - Staffing imbalance
                   - Project delivery risks
                   - Resource dependency concerns

                8. Executive Recommendations
                   - Staffing optimization recommendations
                   - Resource balancing actions
                   - Hiring recommendations
                   - Utilization improvement strategies
                   - Cost optimization opportunities

                VISUAL REQUIREMENTS:

                Use modern executive dashboard styling with:
                - KPI cards
                - professional tables
                - trend indicators
                - utilization heatmaps
                - responsive dashboard layout
                - executive financial styling

                Add charts wherever appropriate using Chart.js.

                REQUIRED CHARTS:
                1. Monthly Utilization Trend (Line Chart)
                2. Employee Utilization Comparison (Bar Chart)
                3. Billable vs Non-Billable Hours (Pie/Doughnut Chart)
                4. Capacity vs Scheduled Hours (Bar Chart)
                5. Project Resource Allocation (Stacked Bar Chart)
                6. Utilization Distribution (Horizontal Bar Chart)

                TECHNICAL REQUIREMENTS:
                - Return complete valid HTML
                - Include Chart.js CDN
                - Include all CSS styling
                - Include chart canvas elements
                - Include JavaScript for charts
                - Use responsive design
                - Use professional corporate dashboard styling
                - Avoid markdown
                - Avoid raw JSON wording
                - Avoid technical API terminology

                DESIGN STYLE:
                - Executive dashboard appearance
                - CFO/PMO presentation quality
                - Modern workforce analytics dashboard
                - Clean corporate UI
                - Professional spacing and typography
                - Presentation-ready layout

                IMPORTANT:
                Return ONLY valid HTML content.
                ";

            //-------------------------------------------------
            // CALL GEMINI
            //-------------------------------------------------

            return await CallGeminiAsync(
                json);
        }
        catch (Exception ex)
        {
            return
                $"Unable to fetch employee schedule. " +
                $"({ex.Message})";
        }
    }


  
//---------------------------------------------------------
// VARIANCE REPORT
//---------------------------------------------------------

public static async Task<string> VarianceAsync(
    string projId,
    string sourceType,
    int sourceVersion,
    string compareType,
    int compareVersion)
    {
        try
        {
            //-------------------------------------------------
            // READ URL FROM CONFIG
            //-------------------------------------------------

            var baseUrl =
                Configuration?["ApiSettings:PlanningApiUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new Exception(
                    "PlanningApiUrl is missing in configuration.");
            }

            //-------------------------------------------------
            // BUILD API URL
            //-------------------------------------------------

            string url =
                $"{baseUrl}/Forecast/GenerateVarianceReportDataForUI" +
                $"?projId={Uri.EscapeDataString(projId)}" +
                $"&sourceType={Uri.EscapeDataString(sourceType)}" +
                $"&sourceVersion={sourceVersion}" +
                $"&compareType={Uri.EscapeDataString(compareType)}" +
                $"&compareVersion={compareVersion}";

            //-------------------------------------------------
            // CALL API
            //-------------------------------------------------

            using var response =
                await httpClient.PostAsync(url, null);

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content.ReadAsStringAsync();

            //-------------------------------------------------
            // BUILD AI PROMPT
            //-------------------------------------------------

            var prompt = $@"
You are a senior financial planning and project controls analyst preparing an executive-level Variance Analysis Dashboard Report for CFOs, PMO leadership, project executives, and finance management.

Convert the provided variance data into a professional executive dashboard HTML report.

COMPARISON CONTEXT:
- Base Scenario: {sourceType} Version {sourceVersion}
- Comparison Scenario: {compareType} Version {compareVersion}

REPORT OBJECTIVES:
- Executive financial visibility
- Variance analysis
- Forecast performance tracking
- Cost control insights
- Revenue forecasting visibility
- Workforce planning analysis
- PMO decision-making support

REQUIRED REPORT SECTIONS:

1. Executive Summary
   - Overall project financial health
   - Variance overview
   - Major cost deviations
   - Revenue forecast changes
   - Labor utilization variance
   - Key financial risks
   - Executive-level observations

2. Financial KPI Dashboard
   Display KPI cards for:
   - Total Source Cost
   - Total Comparison Cost
   - Cost Variance
   - Variance %
   - Total Source Revenue
   - Total Comparison Revenue
   - Revenue Variance
   - Forecasted Hours Variance
   - Actual Hours Variance
   - Gross Margin Impact

3. Cost Variance Analysis
4. Revenue Variance Analysis
5. Workforce & Hours Analysis
6. Monthly Trend Analysis
7. Risk & Operational Analysis
8. Executive Recommendations

VISUAL REQUIREMENTS:

Use modern executive dashboard styling with:
- KPI cards
- professional tables
- variance indicators
- trend arrows
- responsive dashboard layout
- executive financial styling

Add charts wherever appropriate using Chart.js.

REQUIRED CHARTS:
1. Cost Comparison (Bar Chart)
2. Monthly Variance Trend (Line Chart)
3. Revenue Variance Analysis (Bar Chart)
4. Forecasted vs Actual Hours (Line Chart)
5. Variance Distribution (Pie/Doughnut Chart)
6. Monthly Financial Performance (Stacked Bar Chart)

TECHNICAL REQUIREMENTS:
- Return complete valid HTML
- Include Chart.js CDN
- Include all CSS styling
- Include chart canvas elements
- Include JavaScript for charts
- Use responsive design
- Use professional corporate dashboard styling
- Avoid markdown
- Avoid raw JSON wording
- Avoid technical API terminology

DESIGN STYLE:
- Executive financial dashboard appearance
- CFO presentation quality
- Modern PMO analytics dashboard
- Clean corporate UI
- Professional spacing and typography
- Presentation-ready layout

IMPORTANT:
Return ONLY valid HTML content.
";

            //-------------------------------------------------
            // CALL GEMINI
            //-------------------------------------------------

            return await CallGeminiAsync(
                json);
        }
        catch (Exception ex)
        {
            return
                $"Unable to generate variance report. ({ex.Message})";
        }
    }



//    //---------------------------------------------------------
//    // VARIANCE REPORT
//    //---------------------------------------------------------

//    public static async Task<string> VarianceAsync(
//        string projId,
//        int budgetVersion,
//        int eacVersion)
//    {
//        try
//        {
//            //-------------------------------------------------
//            // READ URL FROM CONFIG
//            //-------------------------------------------------

//            var baseUrl =
//                Configuration?["ApiSettings:PlanningApiUrl"];

//            if (string.IsNullOrWhiteSpace(baseUrl))
//            {
//                throw new Exception(
//                    "PlanningApiUrl is missing in configuration.");
//            }

//            //-------------------------------------------------
//            // BUILD URL
//            //-------------------------------------------------

//            string url =
//                $"{baseUrl}/Forecast/GenerateVarianceReportForBudgetVsEACV2" +
//                $"?projId={Uri.EscapeDataString(projId)}" +
//                $"&budgetVersion={budgetVersion}" +
//                $"&eacVersion={eacVersion}";

//            //-------------------------------------------------
//            // CALL API
//            //-------------------------------------------------

//            using var response =
//                await httpClient.PostAsync(url, null);

//            response.EnsureSuccessStatusCode();

//            var json =
//                await response.Content.ReadAsStringAsync();

//            //-------------------------------------------------
//            // AI PROMPT
//            //-------------------------------------------------

//            var prompt = @"
//You are a senior financial planning and project controls analyst preparing an executive-level Variance Analysis Dashboard Report for CFOs, PMO leadership, project executives, and finance management.

//Convert the provided Budget vs EAC variance data into a professional executive dashboard HTML report.

//REPORT OBJECTIVES:
//- Executive financial visibility
//- Budget variance analysis
//- Forecast performance tracking
//- Cost control insights
//- Revenue forecasting visibility
//- Workforce planning analysis
//- PMO decision-making support

//REQUIRED REPORT SECTIONS:

//1. Executive Summary
//   - Overall project financial health
//   - Budget vs EAC variance overview
//   - Major cost deviations
//   - Revenue forecast changes
//   - Labor utilization variance
//   - Key financial risks
//   - Executive-level observations

//2. Financial KPI Dashboard
//   Display KPI cards for:
//   - Total Budget Cost
//   - Total EAC Cost
//   - Cost Variance
//   - Variance %
//   - Total Budget Revenue
//   - Total EAC Revenue
//   - Revenue Variance
//   - Forecasted Hours Variance
//   - Actual Hours Variance
//   - Gross Margin Impact

//3. Cost Variance Analysis
//   - Monthly cost variance trends
//   - Budget vs EAC comparison
//   - Positive and negative variances
//   - Major variance contributors
//   - Forecast accuracy commentary

//4. Revenue Variance Analysis
//   - Revenue forecast changes
//   - Revenue leakage risks
//   - Margin impact analysis
//   - Revenue trend insights

//5. Workforce & Hours Analysis
//   - Forecasted hours variance
//   - Actual hours variance
//   - Resource utilization changes
//   - Labor efficiency insights
//   - Capacity impact analysis

//6. Monthly Trend Analysis
//   - Month-over-month variance
//   - Cost trend patterns
//   - Revenue trend patterns
//   - Burn rate analysis
//   - Forecast movement trends

//7. Risk & Operational Analysis
//   - Budget overrun risks
//   - Margin erosion risks
//   - Resource overutilization risks
//   - Schedule impact risks
//   - Financial forecasting risks

//8. Executive Recommendations
//   - Cost optimization actions
//   - Resource balancing recommendations
//   - Revenue improvement opportunities
//   - Forecast correction strategies
//   - Financial governance recommendations

//VISUAL REQUIREMENTS:

//Use modern executive dashboard styling with:
//- KPI cards
//- professional tables
//- variance indicators
//- trend arrows
//- responsive dashboard layout
//- executive financial styling

//Add charts wherever appropriate using Chart.js.

//REQUIRED CHARTS:
//1. Budget vs EAC Cost Comparison (Bar Chart)
//2. Monthly Cost Variance Trend (Line Chart)
//3. Revenue Variance Analysis (Bar Chart)
//4. Forecasted vs Actual Hours (Line Chart)
//5. Variance Distribution (Pie/Doughnut Chart)
//6. Monthly Financial Performance (Stacked Bar Chart)

//TECHNICAL REQUIREMENTS:
//- Return complete valid HTML
//- Include Chart.js CDN
//- Include all CSS styling
//- Include chart canvas elements
//- Include JavaScript for charts
//- Use responsive design
//- Use professional corporate dashboard styling
//- Avoid markdown
//- Avoid raw JSON wording
//- Avoid technical API terminology

//DESIGN STYLE:
//- Executive financial dashboard appearance
//- CFO presentation quality
//- Modern PMO analytics dashboard
//- Clean corporate UI
//- Professional spacing and typography
//- Presentation-ready layout

//IMPORTANT:
//Return ONLY valid HTML content.
//";

//            //-------------------------------------------------
//            // CALL GEMINI
//            //-------------------------------------------------

//            return await CallGeminiAsync(
//                prompt,
//                json);
//        }
//        catch (Exception ex)
//        {
//            return
//                $"Unable to generate variance report. ({ex.Message})";
//        }
//    }

    //---------------------------------------------------------
    // REVENUE ANALYSIS
    //---------------------------------------------------------

    public static async Task<string>
        GetRevenueAnalysisForProjectByTypeAndVersionAsync(
        string proj_Id,
        string type)
    {
        try
        {
            //-------------------------------------------------
            // READ URL FROM CONFIG
            //-------------------------------------------------

            var baseUrl =
                Configuration?["ApiSettings:PlanningApiUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new Exception(
                    "PlanningApiUrl is missing in configuration.");
            }

            //-------------------------------------------------
            // BUILD URL
            //-------------------------------------------------

            var url =
                $"{baseUrl}/Forecast/CalculateCostAI" +
                $"?proj_Id={Uri.EscapeDataString(proj_Id)}" +
                $"&type={Uri.EscapeDataString(type)}" +
                $"&year={Uri.EscapeDataString(type)}";

            //-------------------------------------------------
            // CALL API
            //-------------------------------------------------

            using var response =
                await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content
                .ReadAsStringAsync();


            return await CallGeminiAsync(
                json);
        }
        catch (Exception ex)
        {
            return
                $"Unable to fetch revenue analysis. " +
                $"({ex.Message})";
        }
    }


    public static async Task<string> GetProjectFinancialsAsync(string projectID, string type, string? status = null)
    {
        try
        {
            var baseUrl = Configuration?["ApiSettings:PlanningApiUrl"];

            // Construct URL with query parameters
            var query = $"projectID={Uri.EscapeDataString(projectID)}&type={Uri.EscapeDataString(type)}";
            if (!string.IsNullOrWhiteSpace(status))
            {
                query += $"&status={Uri.EscapeDataString(status)}";
            }

            var url = $"{baseUrl}/api/AI/GetProjectFinacials?{query}";

            // Using local HttpClient as per requested pattern
            using var httpClient = new HttpClient();

            // Changing to GetAsync as requested
            using var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Pass the raw JSON to the Gemini AI integration method
            return await CallGeminiAsync(json);
        }
        catch (Exception ex)
        {
            return $"Error calling GetProjectFinancials API: {ex.Message}";
        }
    }

    public static async Task<string>
     CreateBudgetFromLatestEacAsync(
     string projId)
    {
        var baseUrl =
               Configuration?["ApiSettings:PlanningApiUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new Exception(
                "PlanningApiUrl is missing in configuration.");
        }
        var url =
            $"{baseUrl}/api/AI/project-eac-next-version?projId={Uri.EscapeDataString(projId)}";

        using var client = new HttpClient();

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string>
GetProjectStatsAsync(string projId)
    {
        try
        {
            var baseUrl =
               Configuration?["ApiSettings:PlanningApiUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new Exception(
                    "PlanningApiUrl is missing in configuration.");
            }
            var url =
                $"{baseUrl}/api/AI/project-stats?projId={Uri.EscapeDataString(projId)}";

            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);

            var content =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return content;
            }

            return content;
        }
        catch (Exception ex)
        {
            return $"Error getting project stats: {ex.Message}";
        }
    }

    public static async Task<string>
 UpdateProjectPlanStatusAsync(
     string projectId,
     string status,
     string? planType = null,
     int? version = null)
    {
        try
        {
            var baseUrl =
                Configuration?["ApiSettings:PlanningApiUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new Exception(
                    "PlanningApiUrl is missing in configuration.");
            }

            var url =
                $"{baseUrl}/api/AI/change-project-status";

            var request =
                new ChangeProjectStatusRequest
                {
                    ProjectId = projectId,
                    PlanType = planType,
                    Version = version,
                    Status = status
                };

            using var httpClient = new HttpClient();

            var response =
                await httpClient.PutAsJsonAsync(
                    url,
                    request);

            var content =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return content;
            }

            return content;
        }
        catch (Exception ex)
        {
            return $"Error updating project status: {ex.Message}";
        }
    }

    public static async Task<string>
GetEmployeePerformanceAsync(
    string employeeId)
    {
        try
        {
            var baseUrl =
                Configuration?["ApiSettings:PlanningApiUrl"];

            var url =
                $"{baseUrl}/api/AI/employee-performance" +
                $"?employeeId={Uri.EscapeDataString(employeeId)}";

            using var httpClient = new HttpClient();

            var response =
                await httpClient.GetAsync(url);

            return await response.Content
                .ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static async Task<string> CallGeminiAsync(string data)
    {
        //HttpClient _httpClient = new HttpClient();
        //var apiKey = "AIzaSyBV7sOv9COGYVOJBU2kQ14BLm43xD1MQfM";
        //var model = "gemini-1.5-flash";
        //var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite-preview:generateContent?key={apiKey}";
        ////var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

        //var requestBody = new
        //{
        //    contents = new[]
        //    {
        //    new
        //    {
        //        parts = new[]
        //        {
        //            new { text = prompt },
        //            new { text = data }
        //        }
        //    }
        //}
        //};

        //var response = await _httpClient.PostAsJsonAsync(url, requestBody);

        //if (!response.IsSuccessStatusCode)
        //{
        //    var error = await response.Content.ReadAsStringAsync();
        //    throw new Exception($"Gemini API Error: {error}");
        //}

        //var json = await response.Content.ReadAsStringAsync();

        //using var doc = JsonDocument.Parse(json);

        //var text = doc.RootElement
        //    .GetProperty("candidates")[0]
        //    .GetProperty("content")
        //    .GetProperty("parts")[0]
        //    .GetProperty("text")
        //    .GetString();

        return data ?? "";
    }

}