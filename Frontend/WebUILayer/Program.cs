using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
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
builder.Services.AddMemoryCache();
builder.Services.AddAuthorizationServices();
builder.Services.AddPerformanceOptimization();
builder.Services.AddWebOptimization();



#endregion







Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseWebOptimizer();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=31536000, immutable");
    }
});

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<MaintenanceMiddleware>();

app.UseStatusCodePagesWithReExecute("/ErrorPage/Index", "?code={0}");

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
