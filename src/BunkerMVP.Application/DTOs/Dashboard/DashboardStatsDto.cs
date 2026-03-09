namespace BunkerMVP.Application.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalRequests { get; set; }
    public int OpenRequests { get; set; }
    public int PendingQuotes { get; set; }
    public int TotalOrders { get; set; }
}
