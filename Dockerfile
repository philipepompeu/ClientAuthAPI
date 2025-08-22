# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o .sln e os .csproj necessários
COPY ClientAuthAPI.sln ./
COPY src/ClientAuthAPI/ClientAuthAPI.csproj ./src/ClientAuthAPI/
COPY src/ClientAuthAPI.Application/ClientAuthAPI.Application.csproj ./src/ClientAuthAPI.Application/
COPY src/ClientAuthAPI.Domain/ClientAuthAPI.Domain.csproj ./src/ClientAuthAPI.Domain/
COPY src/ClientAuthAPI.Infrastructure.Mongo/ClientAuthAPI.Infrastructure.Mongo.csproj ./src/ClientAuthAPI.Infrastructure.Mongo/
COPY src/ClientAuthAPI.Infrastructure.Security/ClientAuthAPI.Infrastructure.Security.csproj ./src/ClientAuthAPI.Infrastructure.Security/

# Restaura as dependências
RUN dotnet restore ./src/ClientAuthAPI/ClientAuthAPI.csproj

# Copia o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish ./src/ClientAuthAPI/ClientAuthAPI.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Define a URL de escuta e expõe a porta
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando de execução
ENTRYPOINT ["dotnet", "ClientAuthAPI.dll"]

# Healthcheck opcional
HEALTHCHECK --interval=45s --timeout=9s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
