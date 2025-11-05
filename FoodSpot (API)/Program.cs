using FoodSpot.Business.Services;
using FoodSpot.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Adiciona suporte para Controllers com Views (MVC)
builder.Services.AddControllersWithViews();

// Adiciona suporte para API Controllers
builder.Services.AddControllers();

// Configura o DbContext com a connection string do Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra o serviço de restaurante no DI Container
builder.Services.AddScoped<IRestaurantService, RestaurantService>();


 builder.Services.AddRazorPages();

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

app.UseRouting();

app.UseAuthorization();

// Configura as rotas para MVC Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapeia as rotas da API
app.MapControllers();


app.MapRazorPages();

app.Run();
