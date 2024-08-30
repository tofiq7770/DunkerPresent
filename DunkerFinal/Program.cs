using DunkerFinal.Extensions;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.DAL;
using Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
  opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddRepositoryLayer();
builder.Services.AddServiceLayer();

var app = builder.Build();






//if (app.Environment.IsDevelopment())
//{
//    //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
//    //developerExceptionPageOptions.SourceCodeLineCount = 1;
//    //app.UseDeveloperExceptionPage(developerExceptionPageOptions);

//    //app.UseExceptionHandler("/Home/Error");
//    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    //app.UseHsts();

//    app.UseMiddleware<GlobalExceptionHandler>();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}



app.UseMiddleware<GlobalExceptionHandler>();

app.UseStatusCodePagesWithReExecute("/StatusCodeError/{0}");




app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

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
