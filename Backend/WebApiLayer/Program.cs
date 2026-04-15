using BusinessLayer.Container;
using Scalar.AspNetCore;
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
builder.Services.AddThirdPartyServices();
builder.Services.AddIdentityAndJwt(builder.Configuration);
builder.Services.AddEmailRateLimiter();
builder.Services.CorsPolicy(builder.Configuration);


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

app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseMiddleware<ApiExceptionMiddleware>();
app.HealthCheckEndpoints();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();


app.Run();
