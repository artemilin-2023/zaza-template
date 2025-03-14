FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HackBack.API/HackBack.API.csproj", "HackBack.API/"]
COPY ["HackBack.Contracts/HackBack.Contracts.csproj", "HackBack.Contracts/"]
COPY ["HackBack.Infrastructure/HackBack.Infrastructure.csproj", "HackBack.Infrastructure/"]
COPY ["HackBack.Domain/HackBack.Domain.csproj", "HackBack.Domain/"]
COPY ["HackBack.Common/HackBack.Common.csproj", "HackBack.Common/"]
COPY ["HackBack.Application/HackBack.Application.csproj", "HackBack.Application/"]
RUN dotnet restore "./HackBack.API/HackBack.API.csproj"
COPY . .
WORKDIR "/src/HackBack.API"
RUN dotnet build "./HackBack.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HackBack.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackBack.API.dll"]