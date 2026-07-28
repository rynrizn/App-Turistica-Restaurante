using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using RestauranteTuristicoApp.Models;
using RestauranteTuristicoApp.Services;
using RestauranteTuristicoApp.Components;
var builder = WebApplication.CreateBuilder(args);

// 1. Configurar DbContextFactory (Provee IDbContextFactory y AppWebContext simultáneamente)
builder.Services.AddDbContextFactory<AppWebContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Servicios de Soporte (Toast y Timeout)
builder.Services.AddScoped<ToastService>();
builder.Services.AddHostedService<ReservaTimeoutService>();

// 3. Servicios del Módulo de Autenticación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
    provider.GetRequiredService<CustomAuthStateProvider>());

// 4. Servicios de Negocio de la Aplicación
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ComponentScanner>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ReporteService>();
builder.Services.AddScoped<ResenaService>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Seed inicial de contraseñas para usuarios de prueba
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppWebContext>>();
        using var db = dbFactory.CreateDbContext();
        var usuarios = db.Usuarios.ToList();
        bool changes = false;
        foreach (var u in usuarios)
        {
            if (u.Email == "admin@restaurante.com")
            {
                u.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123");
                changes = true;
            }
            else if (u.Email == "cmendoza@restaurante.com" || u.Email == "lfernandez@restaurante.com")
            {
                u.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Empleado123");
                changes = true;
            }
            else if (u.Email == "mateo.guerrero@gmail.com" || u.Email == "sofia.paz@outlook.com" || u.Email == "asilva_turismo@yahoo.es")
            {
                u.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Cliente123");
                changes = true;
            }
        }
        if (changes) db.SaveChanges();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error actualizando contraseñas semilla: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();