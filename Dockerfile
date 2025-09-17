# Usa la imagen SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo de proyecto (SupersetAPI.csproj) y restaura las dependencias
# Asegúrate que la ruta al .csproj sea correcta
COPY ["SupersetAPI/SupersetAPI.csproj", "SupersetAPI/"]
RUN dotnet restore "SupersetAPI/SupersetAPI.csproj"

# Copia el resto del código y construye la aplicación
COPY . .
WORKDIR /src/SupersetAPI
RUN dotnet publish "SupersetAPI.csproj" -c Release -o /app/publish

# Usa la imagen ASPNET para ejecutar la aplicación (es más pequeña)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render usa la variable de entorno $PORT. ASP.NET debe escuchar en ella.
ENV ASPNETCORE_URLS=http://+:$PORT 

ENTRYPOINT ["dotnet", "SupersetAPI.dll"]