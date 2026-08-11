//using System;
//using System.Net.Http;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;

//public static class PlanningService
//{
//    private static readonly HttpClient httpClient = new HttpClient();

//    //---------------------------------------------------------
//    // CONFIGURATION
//    //---------------------------------------------------------
//    public static IConfiguration? Configuration { get; set; }

//    private static string GetBaseUrl()
//    {
//        var baseUrl = Configuration?["ApiSettings:PlanningApiUrl"];
//        if (string.IsNullOrWhiteSpace(baseUrl))
//        {
//            throw new Exception("PlanningApiUrl is missing in configuration.");
//        }
//        return baseUrl.TrimEnd('/');
//    }

//    //---------------------------------------------------------
//    // 1. GET BUDGET ASSUMPTIONS
//    //---------------------------------------------------------
//    public static async Task<string> GetBudgetAssumptionsAsync(
//        string? entityId = null,
//        string? propertyId = null,
//        string? buildingId = null,
//        string? unitId = null,
//        string? leaseId = null,
//        string? tenantId = null,
//        int pageNumber = 0,
//        int pageSize = 10)
//    {
//        try
//        {
//            var baseUrl = GetBaseUrl();

//            var queryBuilder = new StringBuilder();
//            queryBuilder.Append($"?pageNumber={pageNumber}&pageSize={pageSize}");

//            if (!string.IsNullOrWhiteSpace(entityId))
//                queryBuilder.Append($"&entityId={Uri.EscapeDataString(entityId)}");

//            if (!string.IsNullOrWhiteSpace(propertyId))
//                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

//            if (!string.IsNullOrWhiteSpace(buildingId))
//                queryBuilder.Append($"&buildingId={Uri.EscapeDataString(buildingId)}");

//            if (!string.IsNullOrWhiteSpace(unitId))
//                queryBuilder.Append($"&unitId={Uri.EscapeDataString(unitId)}");

//            if (!string.IsNullOrWhiteSpace(leaseId))
//                queryBuilder.Append($"&leaseId={Uri.EscapeDataString(leaseId)}");

//            if (!string.IsNullOrWhiteSpace(tenantId))
//                queryBuilder.Append($"&tenantId={Uri.EscapeDataString(tenantId)}");

//            var url = $"{baseUrl}/api/AI/budget-assumptions{queryBuilder}";

//            using var client = new HttpClient();
//            using var response = await client.GetAsync(url);

//            var content = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//            {
//                return $"API Error ({response.StatusCode}): {content}";
//            }

//            return content;
//        }
//        catch (Exception ex)
//        {
//            return $"Error calling GetBudgetAssumptions API: {ex.Message}";
//        }
//    }

//    //---------------------------------------------------------
//    // 2. GET MARKET RENT UNITS
//    //---------------------------------------------------------
//    public static async Task<string> GetMarketRentUnitsAsync(
//        string? propertyId = null,
//        decimal? minRent = null,
//        decimal? maxRent = null,
//        string? unitType = null,
//        string? unitStatus = null,
//        decimal? minArea = null,
//        decimal? maxArea = null,
//        int pageNumber = 0,
//        int pageSize = 10)
//    {
//        try
//        {
//            var baseUrl = GetBaseUrl();

//            var queryBuilder = new StringBuilder();
//            queryBuilder.Append($"?pageNumber={pageNumber}&pageSize={pageSize}");

//            if (!string.IsNullOrWhiteSpace(propertyId))
//                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

//            if (minRent.HasValue)
//                queryBuilder.Append($"&minRent={minRent.Value}");

//            if (maxRent.HasValue)
//                queryBuilder.Append($"&maxRent={maxRent.Value}");

//            if (!string.IsNullOrWhiteSpace(unitType))
//                queryBuilder.Append($"&unitType={Uri.EscapeDataString(unitType)}");

//            if (!string.IsNullOrWhiteSpace(unitStatus))
//                queryBuilder.Append($"&unitStatus={Uri.EscapeDataString(unitStatus)}");

//            if (minArea.HasValue)
//                queryBuilder.Append($"&minArea={minArea.Value}");

//            if (maxArea.HasValue)
//                queryBuilder.Append($"&maxArea={maxArea.Value}");

//            var url = $"{baseUrl}/api/AI/market-rent{queryBuilder}";

//            using var client = new HttpClient();
//            using var response = await client.GetAsync(url);

//            var content = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//            {
//                return $"API Error ({response.StatusCode}): {content}";
//            }

//            return content;
//        }
//        catch (Exception ex)
//        {
//            return $"Error calling GetMarketRentUnits API: {ex.Message}";
//        }
//    }

//    //---------------------------------------------------------
//    // 3. GET EXPIRING LEASES
//    //---------------------------------------------------------
//    public static async Task<string> GetExpiringLeasesAsync(
//    string? propertyId = null,
//    int value = 6,
//    string timeUnit = "months",
//    int pageNumber = 0,
//    int pageSize = 10)
//    {
//        try
//        {
//            var baseUrl = GetBaseUrl();

//            var queryBuilder = new StringBuilder();
//            queryBuilder.Append($"?value={value}&timeUnit={Uri.EscapeDataString(timeUnit)}&pageNumber={pageNumber}&pageSize={pageSize}");

//            if (!string.IsNullOrWhiteSpace(propertyId))
//                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

//            var url = $"{baseUrl}/api/AI/expiring-leases{queryBuilder}";

//            using var client = new HttpClient();
//            using var response = await client.GetAsync(url);

//            var content = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//            {
//                return $"API Error ({response.StatusCode}): {content}";
//            }

//            return content;
//        }
//        catch (Exception ex)
//        {
//            return $"Error calling GetExpiringLeases API: {ex.Message}";
//        }
//    }

//    //---------------------------------------------------------
//    // 4. GET VACANT UNITS
//    //---------------------------------------------------------
//    public static async Task<string> GetVacantUnitsAsync(
//        string? propertyId = null,
//        int pageNumber = 0,
//        int pageSize = 10)
//    {
//        try
//        {
//            var baseUrl = GetBaseUrl();

//            var queryBuilder = new StringBuilder();
//            queryBuilder.Append($"?pageNumber={pageNumber}&pageSize={pageSize}");

//            if (!string.IsNullOrWhiteSpace(propertyId))
//                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

//            var url = $"{baseUrl}/api/AI/vacant-units{queryBuilder}";

//            using var client = new HttpClient();
//            using var response = await client.GetAsync(url);

//            var content = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//            {
//                return $"API Error ({response.StatusCode}): {content}";
//            }

//            return content;
//        }
//        catch (Exception ex)
//        {
//            return $"Error calling GetVacantUnits API: {ex.Message}";
//        }
//    }

//    // Optional helper method to pass responses through if needed
//    public static async Task<string> CallGeminiAsync(string data)
//    {
//        return data;
//    }
//}

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public static class PlanningService
{
    private static readonly HttpClient httpClient = new HttpClient();

    //---------------------------------------------------------
    // CONFIGURATION
    //---------------------------------------------------------
    public static IConfiguration? Configuration { get; set; }

    private static string GetBaseUrl()
    {
        var baseUrl = Configuration?["ApiSettings:PlanningApiUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new Exception("PlanningApiUrl is missing in configuration.");
        }
        return baseUrl.TrimEnd('/');
    }

    //---------------------------------------------------------
    // 1. GET BUDGET ASSUMPTIONS
    //---------------------------------------------------------
    public static async Task<string> GetBudgetAssumptionsAsync(
        string? entityId = null,
        string? propertyId = null,
        string? buildingId = null,
        string? unitId = null,
        string? leaseId = null,
        string? tenantId = null,
        int pageNumber = 0,
        int pageSize = 10)
    {
        try
        {
            var baseUrl = GetBaseUrl();

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!string.IsNullOrWhiteSpace(entityId))
                queryBuilder.Append($"&entityId={Uri.EscapeDataString(entityId)}");

            if (!string.IsNullOrWhiteSpace(propertyId))
                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

            if (!string.IsNullOrWhiteSpace(buildingId))
                queryBuilder.Append($"&buildingId={Uri.EscapeDataString(buildingId)}");

            if (!string.IsNullOrWhiteSpace(unitId))
                queryBuilder.Append($"&unitId={Uri.EscapeDataString(unitId)}");

            if (!string.IsNullOrWhiteSpace(leaseId))
                queryBuilder.Append($"&leaseId={Uri.EscapeDataString(leaseId)}");

            if (!string.IsNullOrWhiteSpace(tenantId))
                queryBuilder.Append($"&tenantId={Uri.EscapeDataString(tenantId)}");

            var url = $"{baseUrl}/api/AI/budget-assumptions{queryBuilder}";

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            using var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "No budget assumptions matching your criteria are currently accessible."
                });
            }

            return content;
        }
        catch (Exception)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = "The budget service is temporarily unavailable. Please try again shortly."
            });
        }
    }

    //---------------------------------------------------------
    // 2. GET MARKET RENT UNITS
    //---------------------------------------------------------
    public static async Task<string> GetMarketRentUnitsAsync(
        string? propertyId = null,
        decimal? minRent = null,
        decimal? maxRent = null,
        string? unitType = null,
        string? unitStatus = null,
        decimal? minArea = null,
        decimal? maxArea = null,
        int pageNumber = 0,
        int pageSize = 10)
    {
        try
        {
            var baseUrl = GetBaseUrl();

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!string.IsNullOrWhiteSpace(propertyId))
                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

            if (minRent.HasValue)
                queryBuilder.Append($"&minRent={minRent.Value}");

            if (maxRent.HasValue)
                queryBuilder.Append($"&maxRent={maxRent.Value}");

            if (!string.IsNullOrWhiteSpace(unitType))
                queryBuilder.Append($"&unitType={Uri.EscapeDataString(unitType)}");

            if (!string.IsNullOrWhiteSpace(unitStatus))
                queryBuilder.Append($"&unitStatus={Uri.EscapeDataString(unitStatus)}");

            if (minArea.HasValue)
                queryBuilder.Append($"&minArea={minArea.Value}");

            if (maxArea.HasValue)
                queryBuilder.Append($"&maxArea={maxArea.Value}");

            var url = $"{baseUrl}/api/AI/market-rent{queryBuilder}";

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            using var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "No units were found matching your pricing or parameter criteria."
                });
            }

            return content;
        }
        catch (Exception)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = "Market rent information is temporarily resting. Please try again."
            });
        }
    }

    //---------------------------------------------------------
    // 3. GET EXPIRING LEASES
    //---------------------------------------------------------
    public static async Task<string> GetExpiringLeasesAsync(
    string? propertyId = null,
    int value = 6,
    string timeUnit = "months",
    int pageNumber = 0,
    int pageSize = 10)
    {
        try
        {
            var baseUrl = GetBaseUrl();

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"?value={value}&timeUnit={Uri.EscapeDataString(timeUnit)}&pageNumber={pageNumber}&pageSize={pageSize}");

            if (!string.IsNullOrWhiteSpace(propertyId))
                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

            var url = $"{baseUrl}/api/AI/expiring-leases{queryBuilder}";

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            using var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "No lease expiration records were located for the given timeframe."
                });
            }

            return content;
        }
        catch (Exception)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = "Unable to fetch lease schedules at this moment. Please adjust your range or try later."
            });
        }
    }

    //---------------------------------------------------------
    // 4. GET VACANT UNITS
    //---------------------------------------------------------
    public static async Task<string> GetVacantUnitsAsync(
        string? propertyId = null,
        int pageNumber = 0,
        int pageSize = 10)
    {
        try
        {
            var baseUrl = GetBaseUrl();

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!string.IsNullOrWhiteSpace(propertyId))
                queryBuilder.Append($"&propertyId={Uri.EscapeDataString(propertyId)}");

            var url = $"{baseUrl}/api/AI/vacant-units{queryBuilder}";

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            using var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "No vacant unit listings could be found for this filter."
                });
            }

            return content;
        }
        catch (Exception)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = "The vacancy service is temporarily unreachable. Please try again shortly."
            });
        }
    }

    public static async Task<string> GetMasterDataAsync(
            string masterType,
            string? searchFilter,
            int pageNumber,
            int pageSize)
    {
        // TODO: Implement your HTTP call or database query to fetch master data
        // Example calling your AiController/AiService endpoint:
        // var response = await _httpClient.GetAsync($"api/ai/master-data?masterType={masterType}&searchFilter={searchFilter}&pageNumber={pageNumber}&pageSize={pageSize}");
        // return await response.Content.ReadAsStringAsync();

        await Task.CompletedTask;
        return "{\"success\":true,\"data\":[]}";
    }

    // Optional helper method to pass responses through if needed
    public static async Task<string> CallGeminiAsync(string data)
    {
        return data;
    }
}