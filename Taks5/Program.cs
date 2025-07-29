using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taks5;
using Taks5.Repositories;
using Taks5.Service;
using Taks5.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.
    UseSqlServer(builder.Configuration.GetConnectionString("Default")));
#region Automapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Dashboard}/{id?}")
    .WithStaticAssets();


app.Run();
