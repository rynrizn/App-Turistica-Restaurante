using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using RestauranteTuristicoApp.Models;
using RestauranteTuristicoApp.Services;
using RestauranteTuristicoApp.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar DbContextFactory (Provee IDbContextFactory y AppWebContext simultáneamente)
builder.Services.AddDbContextFactory<AppWebContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Servicios de MudBlazor
builder.Services.AddMudServices();

// 3. Servicios del Módulo de Autenticación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
    provider.GetRequiredService<CustomAuthStateProvider>());

// 4. Servicios de Negocio de la Aplicación
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ComponentScanner>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

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