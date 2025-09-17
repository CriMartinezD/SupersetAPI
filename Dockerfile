# Dockerfile corregido:
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia el archivo de proyecto, ya que está en la raíz del contexto de build (el directorio actual)
# Cambiamos el destino a solo la carpeta de trabajo /src
COPY ["SupersetAPI.csproj", "."]

# 2. Restaura las dependencias (dentro de /src)
RUN dotnet restore "SupersetAPI.csproj"

# 3. Copia el resto del código
COPY . .

# 4. Publica la aplicación
RUN dotnet publish "SupersetAPI.csproj" -c Release -o /app/publish

# 5. Imagen de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT 

ENTRYPOINT ["dotnet", "SupersetAPI.dll"]