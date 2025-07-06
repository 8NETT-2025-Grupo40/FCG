# -------------------------------
# Stage 1: Imagem base de runtime
# -------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Usar usuário não-root por segurança
USER app
WORKDIR /app

# Configura o Kestrel (ASP.NET Core) para escutar em todas as interfaces na porta 5264
ENV ASPNETCORE_URLS=http://+:5264
# Documenta no Docker que o container expõe a porta 5264
EXPOSE 5264

# -------------------------------
# Stage 2: Build
# -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copia a solução e os arquivos .csproj para aproveitar cache do Docker
COPY ["FCG.sln", "./"]
COPY ["FCG.API/FCG.API.csproj", "FCG.API/"]
COPY ["FCG.Application/FCG.Application.csproj", "FCG.Application/"]
COPY ["FCG.Domain/FCG.Domain.csproj", "FCG.Domain/"]
COPY ["FCG.Infrastructure/FCG.Infrastructure.csproj", "FCG.Infrastructure/"]

# Restaura as dependências do projeto API
RUN dotnet restore "FCG.API/FCG.API.csproj"

# Copia todo o restante do código
COPY . .

# Publica a API em modo Release
WORKDIR "/src/FCG.API"
RUN dotnet publish "FCG.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    # Sem host específico de plataforma, isso reduz o tamanho da imagem
    /p:UseAppHost=false

# -------------------------------
# Stage 3: Imagem final de runtime
# -------------------------------
FROM base AS final
WORKDIR /app

# Copia os artefatos publicados da fase de build
COPY --from=build /app/publish .

# Ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "FCG.API.dll"]
