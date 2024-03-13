FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GestaoProdutos.API/GestaoProdutos.API.csproj", "src/GestaoProdutos.API/"]
COPY ["src/GestaoProdutos.Infra.Ioc/GestaoProdutos.Infra.Ioc.csproj", "src/GestaoProdutos.Infra.Ioc/"]
COPY ["src/GestaoProdutos.Application/GestaoProdutos.Application.csproj", "src/GestaoProdutos.Application/"]
COPY ["src/GestaoProdutos.Domain/GestaoProdutos.Domain.csproj", "src/GestaoProdutos.Domain/"]
COPY ["src/GestaoProdutos.Infra.Data/GestaoProdutos.Infra.Data.csproj", "src/GestaoProdutos.Infra.Data/"]
RUN dotnet restore "./src/GestaoProdutos.API/./GestaoProdutos.API.csproj"
COPY . .
WORKDIR "/src/src/GestaoProdutos.API"
RUN dotnet build "./GestaoProdutos.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GestaoProdutos.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestaoProdutos.API.dll"]