# Restaurante Turístico "Sabor & Tradición" - Sistema Web

Aplicación web patrimonial y gastronómica de Tarija, Bolivia. Desarrollada en **.NET 10 (Blazor Server)** con **PostgreSQL** y **Bootstrap 5 nativo** (cero dependencias externas de MudBlazor o scripts invasivos).

---

## 🔐 Credenciales de Usuarios de Prueba

Para verificar y probar todos los módulos y flujos de la plataforma (Administración, Operaciones de Empleados y Experiencia del Cliente), puede iniciar sesión con cualquiera de los siguientes usuarios precargados:

| Rol | Usuario / Nombre | Correo Electrónico | Contraseña |
| :--- | :--- | :--- | :--- |
| **Administrador** | Administrador | `admin@restaurante.com` | `Admin123` |
| **Empleado 1 (Mesero)** | Carlos Mendoza | `cmendoza@restaurante.com` | `Empleado123` |
| **Empleado 2 (Caja)** | Lucía Fernandez | `lfernandez@restaurante.com` | `Empleado123` |
| **Cliente 1** | Mateo Guerrero | `mateo.guerrero@gmail.com` | `Cliente123` |
| **Cliente 2** | Sofia Valeria Paz | `sofia.paz@outlook.com` | `Cliente123` |
| **Cliente 3** | Alejandro Silva | `asilva_turismo@yahoo.es` | `Cliente123` |

---

## 🏛️ Funcionalidades Principales

1. **Gestión Gastronómica**: Menú interactivo, categorías, control de inventario y pre-orden de platillos tradicionales y vinos de altura.
2. **Reservas en Tiempo Real**: Sistema guiado en 4 pasos, selección de mesas con control de capacidad y regla de expiración automática tras 1 hora y 5 minutos (ej. reserva 16:00 → expiración 17:05).
3. **Perfil de Cliente y Comunidad ("Mis Reseñas")**:
   - Historial de reservaciones con cancelación.
   - Configuración de datos de usuario.
   - Pestaña **"Mis Reseñas"**: Calificación interactiva por estrellas (★ 1 a 5) y publicación de testimonios visibles en la página de inicio.
4. **Operaciones del Salón (Empleados)**:
   - **Gestión de Mesas**: Tablero interactivo del salón con botón para **Adicionar Nuevas Mesas** en tiempo real.
   - **Atención de Pedidos & Cobro**: Cobro con formas de pago (Efectivo, Tarjeta, QR, Transferencia) y aplicación de descuentos.
   - **Moderación de Reseñas**: Pantalla `/gestion/resenas` para mostrar u ocultar reseñas de la comunidad en un solo clic.
5. **Dashboard Administrativo**: Indicadores KPI en tiempo real (facturación, reservas hoy, platillos top) accesible para Administradores y Empleados.

---

## 🚀 Ejecución del Proyecto

```bash
# Compilación limpia
dotnet build

# Ejecución local
dotnet run
```
