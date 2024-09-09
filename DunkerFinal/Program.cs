using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.DAL;
using Service;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
  opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddRepositoryLayer();
builder.Services.AddServiceLayer();

var app = builder.Build();




//app.UseMiddleware<GlobalExceptionHandler>();

//app.UseStatusCodePagesWithReExecute("/StatusCodeError/{0}");




app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});


app.Run();
