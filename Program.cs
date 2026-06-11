using Microsoft.EntityFrameworkCore;
using ServerMCP.Data;
using ServerMCP.Models;

public class Program
{
    public static void Main(string[] args)
    {
        // Create the WebApplication builder
        var builder = WebApplication.CreateBuilder(args);

        PlanningService.Configuration = builder.Configuration;

        // Add MCP Server
        builder.Services.AddMcpServer()
            .WithHttpTransport()
            .WithToolsFromAssembly();

        // Get connection string
        var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Optional: Add EF Core DbContext
        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlite(connStr));

        var app = builder.Build();

        // Map MCP
        app.MapMcp();

        // Run migrations (optional)
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            // var context = services.GetRequiredService<ApplicationDbContext>();
            // context.Database.Migrate();
        }

        // Run the application
        app.Run();
    }
}
