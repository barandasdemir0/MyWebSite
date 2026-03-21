using BusinessLayer.Container;
using Scalar.AspNetCore;
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
builder.Services.CorsPolicy();


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

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await DataSeeder.SeedAsync(scope.ServiceProvider);

}


app.Run();
