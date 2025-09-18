var builder = WebApplication.CreateBuilder(args);

// Define el nombre de la política CORS para usarla más adelante
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 1. Añade el servicio CORS a la colección de servicios
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            // Permite el origen de tu aplicación Angular local
            // Cuando vayas a producción, puedes añadir el dominio público de Angular aquí
            builder.WithOrigins("http://localhost:4200","http://localhost:3000","https://superset-embed-opencity.onrender.com")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// 2. Aplica la política CORS ANTES de UseAuthorization y MapControllers
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();