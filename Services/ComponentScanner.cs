using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace RestauranteTuristicoApp.Services;

public class ComponentScanner
{
    private readonly Assembly _assembly;

    public ComponentScanner()
    {
        _assembly = typeof(Program).Assembly;
    }

    public bool ExisteComponente(string nombreComponente)
    {
        if (string.IsNullOrWhiteSpace(nombreComponente)) return false;

        return _assembly.GetTypes().Any(t =>
            typeof(IComponent).IsAssignableFrom(t) &&
            t.Name.Equals(nombreComponente, StringComparison.OrdinalIgnoreCase));
    }
}