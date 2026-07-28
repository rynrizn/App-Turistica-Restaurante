using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestauranteTuristicoApp.Models;

public partial class AppWebContext : DbContext
{
    public AppWebContext()
    {
    }

    public AppWebContext(DbContextOptions<AppWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetalleReserva> DetalleReservas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FormasPago> FormasPagos { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<ReservaMesa> ReservaMesas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<ConfiguracionRestaurante> ConfiguracionRestaurante { get; set; }

    public virtual DbSet<VHistorialRestaurante> VHistorialRestaurantes { get; set; }

    public virtual DbSet<Resena> Resenas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfiguracionRestaurante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("configuracion_restaurante_pkey");
            entity.ToTable("configuracion_restaurante");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave).HasMaxLength(100).HasColumnName("clave");
            entity.Property(e => e.Valor).HasMaxLength(255).HasColumnName("valor");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "categorias_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<DetalleReserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detalle_reservas_pkey");

            entity.ToTable("detalle_reservas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.ReservaId).HasColumnName("reserva_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleReservas)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_detalle_producto");

            entity.HasOne(d => d.Reserva).WithMany(p => p.DetalleReservas)
                .HasForeignKey(d => d.ReservaId)
                .HasConstraintName("fk_detalle_reserva");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("facturas_pkey");

            entity.ToTable("facturas");

            entity.HasIndex(e => e.NumeroFactura, "facturas_numero_factura_key").IsUnique();

            entity.HasIndex(e => e.ReservaId, "facturas_reserva_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descuento)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("descuento");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado_pago");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_emision");
            entity.Property(e => e.FormaPagoId).HasColumnName("forma_pago_id");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(20)
                .HasColumnName("numero_factura");
            entity.Property(e => e.ReservaId).HasColumnName("reserva_id");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.FormaPago).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.FormaPagoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_factura_forma_pago");

            entity.HasOne(d => d.Reserva).WithOne(p => p.Factura)
                .HasForeignKey<Factura>(d => d.ReservaId)
                .HasConstraintName("fk_factura_reserva");
        });

        modelBuilder.Entity<FormasPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("formas_pago_pkey");

            entity.ToTable("formas_pago");

            entity.HasIndex(e => e.Nombre, "formas_pago_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inventario_pkey");

            entity.ToTable("inventario");

            entity.HasIndex(e => e.ProductoId, "inventario_producto_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");

            entity.HasOne(d => d.Producto).WithOne(p => p.Inventario)
                .HasForeignKey<Inventario>(d => d.ProductoId)
                .HasConstraintName("fk_inventario_producto");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("menu_item_pkey");

            entity.ToTable("menu_item");

            entity.HasIndex(e => e.Ruta, "menu_item_ruta_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .HasColumnName("icono");
            entity.Property(e => e.NombreComponente)
                .HasMaxLength(100)
                .HasColumnName("nombre_componente");
            entity.Property(e => e.NombrePantalla)
                .HasMaxLength(100)
                .HasColumnName("nombre_pantalla");
            entity.Property(e => e.RolRequerido)
                .HasMaxLength(100)
                .HasColumnName("rol_requerido");
            entity.Property(e => e.Ruta)
                .HasMaxLength(150)
                .HasColumnName("ruta");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mesas_pkey");

            entity.ToTable("mesas");

            entity.HasIndex(e => e.NumeroMesa, "mesas_numero_mesa_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CapacidadPersonas).HasColumnName("capacidad_personas");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.NumeroMesa).HasColumnName("numero_mesa");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Disponible)
                .HasDefaultValue(true)
                .HasColumnName("disponible");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .HasColumnName("imagen_url");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_producto_categoria");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reservas_pkey");

            entity.ToTable("reservas");

            entity.HasIndex(e => e.CodigoReserva, "reservas_codigo_reserva_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadPersonas).HasColumnName("cantidad_personas");
            entity.Property(e => e.CodigoReserva)
                .HasMaxLength(20)
                .HasColumnName("codigo_reserva");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaEntrada)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_entrada");
            entity.Property(e => e.FechaReserva).HasColumnName("fecha_reserva");
            entity.Property(e => e.FechaSalida)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_salida");
            entity.Property(e => e.HoraReserva).HasColumnName("hora_reserva");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reserva_usuario");
        });

        modelBuilder.Entity<ReservaMesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reserva_mesas_pkey");

            entity.ToTable("reserva_mesas");

            entity.HasIndex(e => new { e.ReservaId, e.MesaId }, "uq_reserva_mesa").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MesaId).HasColumnName("mesa_id");
            entity.Property(e => e.ReservaId).HasColumnName("reserva_id");

            entity.HasOne(d => d.Mesa).WithMany(p => p.ReservaMesas)
                .HasForeignKey(d => d.MesaId)
                .HasConstraintName("fk_rm_mesa");

            entity.HasOne(d => d.Reserva).WithMany(p => p.ReservaMesas)
                .HasForeignKey(d => d.ReservaId)
                .HasConstraintName("fk_rm_reserva");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "roles_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usuario_rol");
        });

        modelBuilder.Entity<VHistorialRestaurante>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_historial_restaurante");

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CategoriaProducto)
                .HasMaxLength(100)
                .HasColumnName("categoria_producto");
            entity.Property(e => e.Cliente).HasColumnName("cliente");
            entity.Property(e => e.ClienteEmail)
                .HasMaxLength(150)
                .HasColumnName("cliente_email");
            entity.Property(e => e.CodigoReserva)
                .HasMaxLength(20)
                .HasColumnName("codigo_reserva");
            entity.Property(e => e.FacturaId).HasColumnName("factura_id");
            entity.Property(e => e.FechaEmision)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_emision");
            entity.Property(e => e.FechaEntrada)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_entrada");
            entity.Property(e => e.FechaSalida)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_salida");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(30)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(20)
                .HasColumnName("numero_factura");
            entity.Property(e => e.NumeroMesa).HasColumnName("numero_mesa");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Producto)
                .HasMaxLength(100)
                .HasColumnName("producto");
            entity.Property(e => e.SubtotalLinea).HasColumnName("subtotal_linea");
            entity.Property(e => e.TiempoEstadia).HasColumnName("tiempo_estadia");
            entity.Property(e => e.TotalFactura)
                .HasPrecision(10, 2)
                .HasColumnName("total_factura");
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("resenas_pkey");
            entity.ToTable("resenas");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.FechaResena)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_resena");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");

            entity.HasOne(d => d.Usuario).WithMany()
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("fk_resenas_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
