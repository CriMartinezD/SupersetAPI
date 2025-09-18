var builder = WebApplication.CreateBuilder(args);

// Define el nombre de la pol�tica CORS para usarla m�s adelante
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 1. A�ade el servicio CORS a la colecci�n de servicios
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            // Permite el origen de tu aplicaci�n Angular local
            // Cuando vayas a producci�n, puedes a�adir el dominio p�blico de Angular aqu�
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

// 2. Aplica la pol�tica CORS ANTES de UseAuthorization y MapControllers
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();