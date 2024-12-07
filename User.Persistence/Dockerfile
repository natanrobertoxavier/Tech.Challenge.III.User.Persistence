FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["User.Persistence.Api/User.Persistence.Api.csproj", "User.Persistence.Api/"]
RUN dotnet restore "User.Persistence.Api/User.Persistence.Api.csproj"
COPY . .

RUN dotnet build "User.Persistence.Api/User.Persistence.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.Persistence.Api/User.Persistence.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Persistence.Api.dll"]
