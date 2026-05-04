using BusinessLayer.Container;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApiLayer.Hubs;
using WebApiLayer.Middleware;
using WebApiLayer.Seed;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}); 

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


#region benim yazdığım eklediğim şeyler bitince buradan taşı 

builder.Services.AddDatabaseLayers(builder.Configuration);
builder.Services.ContainerDependencies();
builder.Services.AddThirdPartyServices(builder.Configuration);
builder.Services.AddIdentityAndJwt(builder.Configuration);
builder.Services.CorsPolicy(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Services.AddSignalR();



#endregion



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHsts();
}
app.UseForwardedHeaders();


app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRateLimiter();

app.HealthCheckEndpoints();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApiExceptionMiddleware>();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

await app.SeedDatabaseAsync();


app.Run();
