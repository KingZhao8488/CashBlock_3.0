using CashBlockApp.Filters;
using CashBlockApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Agregar el filtro de autorización global
    options.Filters.Add<AuthorizationFilter>();
});

// Agregar HttpContextAccessor para acceder a la sesión
builder.Services.AddHttpContextAccessor();

// Configurar DbContext con MySQL
builder.Services.AddDbContext<CashblockContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("conexion"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb")
    )
);

// Habilitar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 minutos de sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // En producción: capturar excepciones no controladas y re-ejecutar a /error/500
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}
else
{
    // En desarrollo mostrar página de depuración
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Middleware para manejo de errores

// IMPORTANTE: Middleware de sesión y autorización deben estar antes de la re-ejecución de status code pages
// para que, si la página de error necesita acceso a session o autorización, estén disponibles.
app.UseSession();

// Si en el futuro agregas autenticación por cookies u otro scheme,
// registra los servicios en builder.Services y activa UseAuthentication() aquí:
// app.UseAuthentication();

app.UseAuthorization();

// Manejo de códigos de estado (404, 401, etc.)
// Re-ejecuta la pipeline a /error/{code} (tu ErrorController debe tener rutas tipo [Route("error/{code}")])
// Este middleware debe ir antes del mapeo de endpoints.
app.UseStatusCodePagesWithReExecute("/error/{0}");

// Rutas MVC: ruta por defecto y soporte para routing por atributos
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"
);

// MapControllers permite rutas definidas con atributos ([Route(...)]) — útil para ErrorController
app.MapControllers();

app.Run();
