FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/FCG.Api/FCG.Api.csproj", "src/FCG.Api/"]
COPY ["src/FCG.Application/FCG.Application.csproj", "src/FCG.Application/"]
COPY ["src/FCG.Domain/FCG.Domain.csproj", "src/FCG.Domain/"]
COPY ["src/FCG.Infrastructure/FCG.Infrastructure.csproj", "src/FCG.Infrastructure/"]
COPY ["src/submodule/FCG.Shared/FCG.Shared.csproj", "src/submodule/FCG.Shared/"]
RUN dotnet restore "src/FCG.Api/FCG.Api.csproj"
COPY . .
WORKDIR "/src/src/FCG.Api"
RUN dotnet build "FCG.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FCG.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FCG.Api.dll"]
