using Microsoft.AspNetCore.Authentication.Cookies;
using WebUILayer.Extension;
using WebUILayer.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.





#region benim eklediklerim

builder.Services.AddCookieAuth();
builder.Services.AddApiService(builder.Configuration);
builder.Services.AddAutoValidate();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSessionTempData();

#endregion









var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseMiddleware<MaintenanceMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();





#region area için özel kod
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
