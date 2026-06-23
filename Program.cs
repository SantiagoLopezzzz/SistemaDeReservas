using Microsoft.EntityFrameworkCore;
using sistemaReservas.Repositories;
using sistemaReservas.Services;
using SistemaReservas.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<ISedesRepository, SedesRepository>();
builder.Services.AddScoped<IAlojamientosRepository, AlojamientosRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IReservasRepository, ReservasRepository>();

// Servicios
builder.Services.AddScoped<ISedesService, SedesService>();
builder.Services.AddScoped<IAlojamientosService, AlojamientosService>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IReservasService, ReservasService>();

// Servicio de correo SMTP
builder.Services.AddScoped<IEmailService, EmailService>();

// MVC
builder.Services.AddControllersWithViews();

// Autenticacion con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// IMPORTANTE: UseAuthentication DEBE ir antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sedes}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();