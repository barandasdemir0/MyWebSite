namespace WebApiLayer.Middleware;

public static class ApiHealthCheckEndpointMiddleware
{
    public static void HealthCheckEndpoints(this IEndpointRouteBuilder services)
    {
        services.MapGet("/", () => Results.Ok(new
        {
            status = "healthy",
            service = "Baran Dasdemir Portfolio API",
            documentation = "/scalar/v1",
            timestamp = DateTime.UtcNow
        }));
    }
}
