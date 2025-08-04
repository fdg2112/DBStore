FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DBStore.sln", "."]
COPY ["src/DBStore.Domain/DBStore.Domain.csproj", "src/DBStore.Domain/"]
COPY ["src/DBStore.Infrastructure/DBStore.Infrastructure.csproj", "src/DBStore.Infrastructure/"]
COPY ["src/DBStore.Application/DBStore.Application.csproj", "src/DBStore.Application/"]
COPY ["src/DBStore.Api/DBStore.Api.csproj", "src/DBStore.Api/"]
RUN dotnet restore "src/DBStore.Api/DBStore.Api.csproj"
COPY . .
WORKDIR "/src/src/DBStore.Api"
RUN dotnet publish "DBStore.Api.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DBStore.Api.dll"]
