FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Radancy.Api/Radancy.Api.csproj", "Radancy.Api/"]
RUN dotnet restore "Radancy.Api/Radancy.Api.csproj"
COPY . .
WORKDIR "/src/Radancy.Api"
RUN dotnet build "Radancy.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radancy.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Radancy.Api.dll"]
