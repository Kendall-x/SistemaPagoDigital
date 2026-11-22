using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using Microsoft.AspNetCore.Identity;
using SistemaPagoDigital.Models;
using SistemaPagoDigital.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CuentaService>();
builder.Services.AddScoped<RecargaService>();

builder.Services
    .AddIdentity<Usuario, IdentityRole<int>>()
    .AddEntityFrameworkStores<AplicacionDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opciones =>
{
    opciones.User.RequireUniqueEmail = true;
    opciones.Password.RequiredLength = 8;
    opciones.Password.RequireDigit = true;
    opciones.Password.RequireUppercase = true;
    opciones.Password.RequireLowercase = true;
    opciones.Password.RequireNonAlphanumeric = true;
});

builder.Services.ConfigureApplicationCookie(opciones =>
{
    opciones.LoginPath = "/Account/Login";
    opciones.AccessDeniedPath = "/Account/AccessDenied";
});

var cadenaConexion = builder.Configuration.GetConnectionString("ConexionMySQL")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión ConexionMySQL."
    );

builder.Services.AddDbContext<AplicacionDbContext>(opciones =>
    opciones.UseMySQL(cadenaConexion)
);
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
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

 using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole<int>>>();

    string[] roles = { "Cliente", "Conductor" };

    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(rol));
        }
    }
}

app.Run();
