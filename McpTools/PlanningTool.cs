using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using ServerMCP.Models;

namespace MCPServer.MCPTools
{
    [McpServerToolType]
    public static class PlanningTool
    {
        // =========================================================================
        // 1. GET BUDGET ASSUMPTIONS
        // =========================================================================
        [McpServerTool, Description(@"
Fetches financial and budget assumption details (such as growth rates, escalation rates, inflation parameters, calculation methods, and effective date ranges) across the property organizational hierarchy.

When to Use:
- Use when the user asks about budget assumptions, calculation methods, financial parameters, or escalation rules.

Parameters & Logic:
- Optional Filters: entityId, propertyId, buildingId, unitId, leaseId, tenantId.
- Empty Inputs Rule: If NO IDs are provided (all parameters empty), the API returns ALL budget assumptions across the entire system.
- Match Rule: If specific IDs are provided and matched, returns specific assumption details for that level.
- Fallback Rule: If specific IDs are provided BUT NO MATCH IS FOUND, the API automatically falls back to returning the Global Default assumptions (where all IDs are null).

Examples:
- 'What are the budget assumptions for entity ENT001?'
- 'Show me all budget assumptions'
- 'Get calculation methods for unit U-102 and lease L-889'
")]
        public static async Task<string> GetBudgetAssumptionsAsync(
            [Description("Optional Entity Identifier")] string? entityId = null,
            [Description("Optional Property Identifier")] string? propertyId = null,
            [Description("Optional Building Identifier")] string? buildingId = null,
            [Description("Optional Unit Identifier")] string? unitId = null,
            [Description("Optional Lease Identifier")] string? leaseId = null,
            [Description("Optional Tenant Identifier")] string? tenantId = null,
            [Description("Page number for pagination (default 0 for all records)")] int pageNumber = 0,
            [Description("Page size for pagination (default 10)")] int pageSize = 10)
        {
            try
            {
                // Normalize "ALL" string from AI to null
                entityId = CleanInput(entityId);
                propertyId = CleanInput(propertyId);
                buildingId = CleanInput(buildingId);
                unitId = CleanInput(unitId);
                leaseId = CleanInput(leaseId);
                tenantId = CleanInput(tenantId);

                return await PlanningService.GetBudgetAssumptionsAsync(
                    entityId, propertyId, buildingId, unitId, leaseId, tenantId, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                return $"Error fetching budget assumptions: ({ex.Message})";
            }
        }

        // =========================================================================
        // 2. GET MARKET RENT UNITS
        // =========================================================================
        [McpServerTool, Description(@"
Queries and filters property units based on market rent pricing, occupancy status, unit type, and square footage area constraints.

When to Use:
- Use when users ask to search or filter units by rental price ranges, unit availability, unit classifications, or area/size.

Parameters:
- propertyId: Optional property filter.
- minRent / maxRent: Numerical limits for market rental rates (e.g., maxRent = 4000).
- unitType: Type of unit (e.g., Office, Retail, Residential, Commercial).
- unitStatus: Current status (e.g., Vacant, Occupied, Under Maintenance).
- minArea / maxArea: Square footage or floor area limits.

Examples:
- 'Find all vacant units with market rent under $4000/month'
- 'Show commercial retail units between 1000 and 2500 sq ft'
- 'List occupied office units in property P-101'
")]
        public static async Task<string> GetMarketRentUnitsAsync(
            [Description("Optional Property Identifier")] string? propertyId = null,
            [Description("Optional Minimum Rent price filter")] decimal? minRent = null,
            [Description("Optional Maximum Rent price filter")] decimal? maxRent = null,
            [Description("Optional Unit Type classification (e.g., Office, Retail, Commercial)")] string? unitType = null,
            [Description("Optional Unit Status (e.g., Vacant, Occupied)")] string? unitStatus = null,
            [Description("Optional Minimum Area size in square feet")] decimal? minArea = null,
            [Description("Optional Maximum Area size in square feet")] decimal? maxArea = null,
            [Description("Page number (default 0)")] int pageNumber = 0,
            [Description("Page size (default 10)")] int pageSize = 10)
        {
            try
            {
                propertyId = CleanInput(propertyId);
                unitType = CleanInput(unitType);
                unitStatus = CleanInput(unitStatus);

                return await PlanningService.GetMarketRentUnitsAsync(
                    propertyId, minRent, maxRent, unitType, unitStatus, minArea, maxArea, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                return $"Error fetching market rent units: ({ex.Message})";
            }
        }

        // =========================================================================
        // 3. GET EXPIRING LEASES
        // =========================================================================
        [McpServerTool, Description(@"
Retrieves a list of lease agreements that are scheduled to expire within a specified time horizon (in months).

When to Use:
- Use when users inquire about upcoming lease expirations, lease renewal schedules, or tenant turnover risks within a given timeframe.

Parameters:
- propertyId: Optional filter for a specific property.
- monthsWithin: Number of months into the future to check for expiring leases (e.g., 3, 6, 12 months). Default is 6 months.

Examples:
- 'Show all leases expiring in the next 6 months'
- 'Which leases in property P-200 are expiring within 30 days?'
- 'Get lease expiration report for the next 12 months'
")]
        public static async Task<string> GetExpiringLeasesAsync(
            [Description("Optional Property Identifier")] string? propertyId = null,
            [Description("Number of upcoming months to check for lease expirations (default 6 months)")] int monthsWithin = 6,
            [Description("Page number (default 0)")] int pageNumber = 0,
            [Description("Page size (default 10)")] int pageSize = 10)
        {
            try
            {
                propertyId = CleanInput(propertyId);

                return await PlanningService.GetExpiringLeasesAsync(
                    propertyId, monthsWithin, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                return $"Error fetching expiring leases: ({ex.Message})";
            }
        }

        // =========================================================================
        // 4. GET VACANT UNITS
        // =========================================================================
        [McpServerTool, Description(@"
Retrieves a standardized report of all currently un-leased / vacant units for a given property or across all properties.

When to Use:
- Use when the user asks for vacant unit availability reports, un-occupied unit listings, or general vacancy statistics.

Parameters:
- propertyId: Optional property identifier to filter vacant spaces.

Examples:
- 'Show all vacant units for property P-100'
- 'List all currently available vacant spaces'
- 'Get vacancy listing'
")]
        public static async Task<string> GetVacantUnitsAsync(
            [Description("Optional Property Identifier")] string? propertyId = null,
            [Description("Page number (default 0)")] int pageNumber = 0,
            [Description("Page size (default 10)")] int pageSize = 10)
        {
            try
            {
                propertyId = CleanInput(propertyId);

                return await PlanningService.GetVacantUnitsAsync(
                    propertyId, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                return $"Error fetching vacant units: ({ex.Message})";
            }
        }

        // =========================================================================
        // HELPER UTILITIES
        // =========================================================================
        private static string? CleanInput(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            return value.Trim();
        }
    }
}