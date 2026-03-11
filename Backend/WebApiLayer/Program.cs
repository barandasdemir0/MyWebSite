using BusinessLayer.Container;
using Scalar.AspNetCore;

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

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
